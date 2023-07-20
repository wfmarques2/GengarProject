using AutoMapper;
using DijitaruVatigoGengar.Data.Dtos;
using DijitaruVatigoGengar.Data;
using DijitaruVatigoGengar.Models;
using Microsoft.AspNetCore.Mvc;
using DijitaruVatigoGengar.Enums;
using Microsoft.EntityFrameworkCore;

namespace DijitaruVatigoGengar.Controllers;

[ApiController]
[Route("Aprovações")]
public class ApprovedHoursController : ControllerBase
{
    private readonly DijitaruVatigoGengarContext _context;
    private readonly IMapper _mapper;

    public ApprovedHoursController(DijitaruVatigoGengarContext dbContext, IMapper mapper)
    {
        _context = dbContext;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult CreateApproved([FromBody] CreateApprovedHourDto approvedDto)
    {
        // Verificar se o aprovador existe no banco de dados
        var approver = _context.Collaborators.FirstOrDefault(c => c.Id == approvedDto.ApproverId);
        if (approver == null)
            return NotFound("Aprovador não encontrado.");

        // Verificar se o registro de horas pendentes existe no banco de dados e se ainda não foi aprovado ou rejeitado
        var pendingHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == approvedDto.PendingHourId && ph.StatusText == "pending");
        if (pendingHour == null)
            return NotFound("Hora pendente não encontrada ou já aprovada/rejeitada.");

        // Verificar se o colaborador é um Approver ou Admin
        if (approver.CollaboratorRole != Role.Approver && approver.CollaboratorRole != Role.Admin)
            return Forbid("Você não tem permissão para aprovar horas pendentes.");

        // Verificar se o colaborador não está aprovando suas próprias horas pendentes
        if (pendingHour.CollaboratorId == approver.Id)
            return BadRequest("Você não pode aprovar suas próprias horas pendentes.");

        if (approvedDto.StatusText.Equals("approved", StringComparison.OrdinalIgnoreCase) || approvedDto.StatusText.Equals("rejected", StringComparison.OrdinalIgnoreCase))
        {
            pendingHour.StatusText = approvedDto.StatusText;
        }
        else
        {
            return BadRequest("O valor do status fornecido não é válido. Use Approved ou Rejected.");
        }

        _context.SaveChanges();

        var readPendingHourDto = _mapper.Map<ReadPendingHourDto>(pendingHour);
        return Ok(readPendingHourDto);
    }

    [HttpGet]
    public IActionResult GetAllApprovedHours()
    {
        var approvedHours = _context.PendingHours
            .Where(ph => ph.StatusText.ToLower() == "approved")
            .ToList();

        var readApprovedHourDtos = _mapper.Map<List<ReadPendingHourDto>>(approvedHours);
        return Ok(readApprovedHourDtos);
    }

    [HttpGet("{id}")]
    public IActionResult GetApprovedHour(int id)
    {
        var approvedHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == id && ph.StatusText.Equals("approved"));
        if (approvedHour == null)
            return NotFound("Hora aprovada não encontrada.");

        var readApprovedHourDto = _mapper.Map<ReadPendingHourDto>(approvedHour);
        return Ok(readApprovedHourDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateApprovalStatus(int id, [FromBody] UpdateApprovedHourDto updateDto)
    {
        // Verificar se a hora aprovada existe no banco de dados
        var approvedHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == id && ph.StatusText.ToLower() == "approved");

        if (approvedHour != null)
        {
            // Se a hora aprovada existe, só pode ser alterada para "rejected" ou "pending"
            if (!updateDto.StatusText.Equals("rejected", StringComparison.OrdinalIgnoreCase) && !updateDto.StatusText.Equals("pending", StringComparison.OrdinalIgnoreCase))
                return BadRequest("O valor do status fornecido não é válido. Use Rejected ou Pending.");

            // Verificar se o colaborador não está aprovando suas próprias horas aprovadas
            var approverId = approvedHour.CollaboratorId;
            if (updateDto.ApproverId == approverId)
                return BadRequest("Você não pode aprovar suas próprias horas aprovadas.");

            // Atualizar o StatusText da hora aprovada com base no valor fornecido em updateDto
            approvedHour.StatusText = updateDto.StatusText.ToLower();
            _context.SaveChanges();

            var readApprovedHourDto = _mapper.Map<ReadPendingHourDto>(approvedHour);
            return Ok(readApprovedHourDto);
        }

        // Verificar se a hora pendente existe no banco de dados
        var pendingHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == id && (ph.StatusText.ToLower() == "pending" || ph.StatusText.ToLower() == "rejected"));

        if (pendingHour != null)
        {
            // Verificar se o colaborador não está alterando suas próprias horas pendentes
            var collaboratorId = pendingHour.CollaboratorId;
            if (updateDto.ApproverId == collaboratorId)
                return BadRequest("Você não pode alterar suas próprias horas pendentes.");

            if (pendingHour.StatusText.Equals("rejected", StringComparison.OrdinalIgnoreCase))
            {
                // Se a hora pendente for "rejected", só pode ser alterada para "approved" ou "pending"
                if (!updateDto.StatusText.Equals("approved", StringComparison.OrdinalIgnoreCase) && !updateDto.StatusText.Equals("pending", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("O valor do status fornecido não é válido. Use Approved ou Pending.");
            }
            else if (pendingHour.StatusText.Equals("pending", StringComparison.OrdinalIgnoreCase))
            {
                // Se a hora pendente for "pending", só pode ser alterada para "approved" ou "rejected"
                if (!updateDto.StatusText.Equals("approved", StringComparison.OrdinalIgnoreCase) && !updateDto.StatusText.Equals("rejected", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("O valor do status fornecido não é válido. Use Approved ou Rejected.");
            }

            // Atualizar o StatusText da hora pendente com base no valor fornecido em updateDto
            pendingHour.StatusText = updateDto.StatusText.ToLower();
            _context.SaveChanges();

            var readPendingHourDto = _mapper.Map<ReadPendingHourDto>(pendingHour);
            return Ok(readPendingHourDto);
        }

        // Se a hora não for encontrada, retornar uma mensagem informando que não existe
        return NotFound("Hora não encontrada.");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteApprovedHour(int id)
    {
        var approvedHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == id && ph.StatusText.Equals("approved", StringComparison.OrdinalIgnoreCase));
        if (approvedHour == null)
            return NotFound("Hora aprovada não encontrada.");

        _context.PendingHours.Remove(approvedHour);
        _context.SaveChanges();

        return NoContent();
    }
}

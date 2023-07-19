using AutoMapper;
using DijitaruVatigoGengar.Data.Dtos;
using DijitaruVatigoGengar.Data;
using DijitaruVatigoGengar.Models;
using Microsoft.AspNetCore.Mvc;
using DijitaruVatigoGengar.Enums;

namespace DijitaruVatigoGengar.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    public IActionResult CreateApproval([FromBody] CreateApprovedHourDto approvalDto)
    {
        // Verificar se o aprovador existe no banco de dados
        var approver = _context.Collaborators.FirstOrDefault(c => c.Id == approvalDto.ApproverId);
        if (approver == null)
            return NotFound("Aprovador não encontrado.");

        // Verificar se o registro de horas pendentes existe no banco de dados e se ainda não foi aprovado
        var pendingHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == approvalDto.PendingHourId && !ph.IsApproved);
        if (pendingHour == null)
            return NotFound("Hora pendente não encontrada ou já aprovada.");

        // Verificar se o colaborador é um Approver ou Admin
        if (approver.CollaboratorRole != Role.Approver && approver.CollaboratorRole != Role.Admin)
            return Forbid("Você não tem permissão para aprovar horas.");

        // Definir o status de aprovação com base no campo IsApproved do DTO
        pendingHour.IsApproved = approvalDto.IsApproved;

        // Salvar as alterações no banco de dados
        _context.SaveChanges();

        // Remover a propriedade ApprovedHours do DTO de leitura
        var readPendingHourDto = _mapper.Map<ReadPendingHourDto>(pendingHour);
        return Ok(readPendingHourDto);
    }

    [HttpGet]
    public IActionResult GetAllApprovedHours()
    {
        var approvedHours = _context.PendingHours.Where(ph => ph.IsApproved).ToList();
        var readApprovedHourDtos = _mapper.Map<List<ReadPendingHourDto>>(approvedHours);
        return Ok(readApprovedHourDtos);
    }

    [HttpGet("{id}")]
    public IActionResult GetApprovedHour(int id)
    {
        var approvedHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == id && ph.IsApproved);
        if (approvedHour == null)
            return NotFound();

        var readApprovedHourDto = _mapper.Map<ReadPendingHourDto>(approvedHour);
        return Ok(readApprovedHourDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateApprovedHour(int id, [FromBody] UpdatePendingHourDto approvedHourDto)
    {
        var existingApprovedHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == id && ph.IsApproved);
        if (existingApprovedHour == null)
            return NotFound();

        _mapper.Map(approvedHourDto, existingApprovedHour);
        _context.SaveChanges();

        var readApprovedHourDto = _mapper.Map<ReadPendingHourDto>(existingApprovedHour);
        return Ok(readApprovedHourDto);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteApprovedHour(int id)
    {
        var approvedHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == id && ph.IsApproved);
        if (approvedHour == null)
            return NotFound("Hora aprovada não encontrada.");

        _context.PendingHours.Remove(approvedHour);
        _context.SaveChanges();

        return NoContent();
    }
}

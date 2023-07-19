using AutoMapper;
using DijitaruVatigoGengar.Data;
using DijitaruVatigoGengar.Data.Dtos;
using DijitaruVatigoGengar.Enums;
using DijitaruVatigoGengar.Models;
using Microsoft.AspNetCore.Mvc;

namespace DijitaruVatigoGengar.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PendingHoursController : ControllerBase
{
    private readonly DijitaruVatigoGengarContext _context;
    private readonly IMapper _mapper;

    public PendingHoursController(DijitaruVatigoGengarContext dbContext, IMapper mapper)
    {
        _context = dbContext;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AddPendingHour([FromBody] CreatePendingHourDto pendingHourDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var pendingHour = _mapper.Map<PendingHour>(pendingHourDto);
        _context.PendingHours.Add(pendingHour);
        _context.SaveChanges();

        var readPendingHourDto = _mapper.Map<ReadPendingHourDto>(pendingHour);
        return CreatedAtAction(nameof(GetPendingHour), new { id = pendingHour.Id }, readPendingHourDto);
    }

    [HttpDelete("{id}")]
    public IActionResult DeletePendingHour(int id)
    {
        var pendingHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == id);
        if (pendingHour == null)
            return NotFound("Hora pendente não encontrada.");

        _context.PendingHours.Remove(pendingHour);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(500, "Ocorreu um erro ao excluir a hora pendente.");
        }

        return NoContent();
    }

    [HttpGet]
    public IActionResult GetAllPendingHours()
    {
        var pendingHours = _context.PendingHours.ToList();
        var readPendingHourDtos = _mapper.Map<List<ReadPendingHourDto>>(pendingHours);
        return Ok(readPendingHourDtos);
    }

    [HttpGet("{id}")]
    public IActionResult GetPendingHour(int id)
    {
        var pendingHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == id);
        if (pendingHour == null)
            return NotFound("Hora pendente não encontrada.");

        var readPendingHourDto = _mapper.Map<ReadPendingHourDto>(pendingHour);
        return Ok(readPendingHourDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdatePendingHour(int id, [FromBody] UpdatePendingHourDto pendingHourDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingPendingHour = _context.PendingHours.FirstOrDefault(ph => ph.Id == id);
        if (existingPendingHour == null)
            return NotFound("Hora pendente não encontrada.");

        _mapper.Map(pendingHourDto, existingPendingHour);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(500, "Ocorreu um erro ao atualizar a hora pendente.");
        }

        var readPendingHourDto = _mapper.Map<ReadPendingHourDto>(existingPendingHour);
        return Ok(readPendingHourDto);
    }

    [HttpGet("Project/{projectId}/Collaborator/{collaboratorId}")]
    public IActionResult GetPendingHoursByProjectAndCollaborator(int projectId, int collaboratorId)
    {
        var pendingHours = _context.PendingHours
            .Where(ph => ph.ProjectId == projectId && ph.CollaboratorId == collaboratorId)
            .ToList();

        var readPendingHourDtos = _mapper.Map<List<ReadPendingHourDto>>(pendingHours);
        return Ok(readPendingHourDtos);
    }



}



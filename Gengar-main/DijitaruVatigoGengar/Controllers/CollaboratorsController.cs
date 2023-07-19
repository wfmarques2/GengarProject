using AutoMapper;
using DijitaruVatigoGengar.Data;
using DijitaruVatigoGengar.Data.Dtos;
using DijitaruVatigoGengar.Models;
using Microsoft.AspNetCore.Mvc;

namespace DijitaruVatigoGengar.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CollaboratorsController : ControllerBase
{
    private readonly DijitaruVatigoGengarContext _context;
    private readonly IMapper _mapper;

    public CollaboratorsController(DijitaruVatigoGengarContext dbContext, IMapper mapper)
    {
        _context = dbContext;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AddCollaborator([FromBody] CreateCollaboratorDto collaboratorDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var collaborator = _mapper.Map<Collaborator>(collaboratorDto);
        _context.Collaborators.Add(collaborator);
        _context.SaveChanges();

        var readCollaboratorDto = _mapper.Map<ReadCollaboratorDto>(collaborator);
        return CreatedAtAction(nameof(GetCollaborator), new { id = collaborator.Id }, readCollaboratorDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCollaborator(int id, [FromBody] UpdateCollaboratorDto collaboratorDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingCollaborator = _context.Collaborators.FirstOrDefault(c => c.Id == id);
        if (existingCollaborator == null)
            return NotFound("Colaborador não encontrado.");

        _mapper.Map(collaboratorDto, existingCollaborator);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(500, "Ocorreu um erro ao atualizar o colaborador.");
        }

        var readCollaboratorDto = _mapper.Map<ReadCollaboratorDto>(existingCollaborator);
        return Ok(readCollaboratorDto);
    }

    [HttpGet]
    public IActionResult GetAllCollaborators()
    {
        var collaborators = _context.Collaborators.ToList();
        var readCollaboratorDtos = _mapper.Map<List<ReadCollaboratorDto>>(collaborators);
        return Ok(readCollaboratorDtos);
    }

    [HttpGet("{id}")]
    public IActionResult GetCollaborator(int id)
    {
        var collaborator = _context.Collaborators.FirstOrDefault(c => c.Id == id);
        if (collaborator == null)
            return NotFound("Colaborador não encontrado.");

        var readCollaboratorDto = _mapper.Map<ReadCollaboratorDto>(collaborator);
        return Ok(readCollaboratorDto);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCollaborator(int id)
    {
        var collaborator = _context.Collaborators.FirstOrDefault(c => c.Id == id);
        if (collaborator == null)
            return NotFound("Colaborador não encontrado.");

        _context.Collaborators.Remove(collaborator);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(500, "Ocorreu um erro ao excluir o colaborador.");
        }

        return NoContent();
    }

}

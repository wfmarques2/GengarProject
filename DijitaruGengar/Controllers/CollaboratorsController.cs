using AutoMapper;
using DijitaruVatigoGengar.Data;
using DijitaruVatigoGengar.Data.Dtos;
using DijitaruVatigoGengar.Enums;
using DijitaruVatigoGengar.Models;
using Microsoft.AspNetCore.Mvc;

namespace DijitaruVatigoGengar.Controllers;

[ApiController]
[Route("Colaborações")]
public class CollaboratorsController : ControllerBase
{
    private DijitaruVatigoGengarContext _context;
    private IMapper _mapper;

    public CollaboratorsController(DijitaruVatigoGengarContext dbContext, IMapper mapper)
    {
        _context = dbContext;
        _mapper = mapper;
    }
    [HttpPost]
    public IActionResult AddCollaborator([FromBody] CreateCollaboratorDto collaboratorDto)
    {
        // Verificar se o gênero é válido (masculino ou feminino)
        if (!Enum.TryParse<Gender>(collaboratorDto.GenderText, true, out var gender))
        {
            return BadRequest("Gênero inválido. Use masculino ou feminino.");
        }

        // Verificar se o papel do colaborador é válido (normal, admin ou approver)
        if (!Enum.TryParse<Role>(collaboratorDto.RoleText, true, out var role))
        {
            return BadRequest("Papel inválido. Use normal, admin ou approver.");
        }

        // Verificar se a modalidade do contrato é válida (intern, clt ou pj)
        if (!Enum.TryParse<ContractModality>(collaboratorDto.ModalityText, true, out var modality))
        {
            return BadRequest("Modalidade inválida. Use intern, clt ou pj.");
        }

        var collaborator = _mapper.Map<Collaborator>(collaboratorDto);

        // Definir as strings de enum padronizadas nos campos correspondentes
        collaborator.GenderText = collaboratorDto.GenderText;
        collaborator.RoleText = collaboratorDto.RoleText;
        collaborator.ModalityText = collaboratorDto.ModalityText;

        _context.Collaborators.Add(collaborator);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetCollaborator), new { id = collaborator.Id }, collaborator);
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

    [HttpPut("{id}")]
    public IActionResult UpdateCollaborator(int id, [FromBody] UpdateCollaboratorDto collaboratorDto)
    {
        var existingCollaborator = _context.Collaborators.FirstOrDefault(c => c.Id == id);

        if (existingCollaborator == null)
        {
            // Criar um novo colaborador, já que o id não foi encontrado
            var newCollaborator = _mapper.Map<Collaborator>(collaboratorDto);

            // Verificar se o gênero é válido (masculino ou feminino)
            if (!Enum.TryParse<Gender>(newCollaborator.GenderText, true, out var gender))
            {
                return BadRequest("Gênero inválido. Use masculino ou feminino.");
            }

            // Verificar se o papel do colaborador é válido (normal, admin ou approver)
            if (!Enum.TryParse<Role>(newCollaborator.RoleText, true, out var role))
            {
                return BadRequest("Papel inválido. Use normal, admin ou approver.");
            }

            // Verificar se a modalidade do contrato é válida (intern, clt ou pj)
            if (!Enum.TryParse<ContractModality>(newCollaborator.ModalityText, true, out var modality))
            {
                return BadRequest("Modalidade inválida. Use intern, clt ou pj.");
            }

            _context.Collaborators.Add(newCollaborator);
            _context.SaveChanges();

            var readCollaboratorDto = _mapper.Map<ReadCollaboratorDto>(newCollaborator);
            return CreatedAtAction(nameof(GetCollaborator), new { id = newCollaborator.Id }, readCollaboratorDto);
        }
        else
        {
            // Verificar se o gênero é válido (masculino ou feminino)
            if (!Enum.TryParse<Gender>(collaboratorDto.GenderText, true, out var gender))
            {
                return BadRequest("Gênero inválido. Use masculino ou feminino.");
            }

            // Verificar se o papel do colaborador é válido (normal, admin ou approver)
            if (!Enum.TryParse<Role>(collaboratorDto.RoleText, true, out var role))
            {
                return BadRequest("Papel inválido. Use normal, admin ou approver.");
            }

            // Verificar se a modalidade do contrato é válida (intern, clt ou pj)
            if (!Enum.TryParse<ContractModality>(collaboratorDto.ModalityText, true, out var modality))
            {
                return BadRequest("Modalidade inválida. Use intern, clt ou pj.");
            }

            _mapper.Map(collaboratorDto, existingCollaborator);
            _context.SaveChanges();

            var readCollaboratorDto = _mapper.Map<ReadCollaboratorDto>(existingCollaborator);
            return Ok(readCollaboratorDto);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCollaborator(int id)
    {
        var collaborator = _context.Collaborators.FirstOrDefault(c => c.Id == id);
        if (collaborator == null)
            return NotFound("Colaborador não encontrado.");

        _context.Collaborators.Remove(collaborator);
        _context.SaveChanges();

        return NoContent();
    }
}

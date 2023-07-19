using AutoMapper;
using DijitaruVatigoGengar.Data;
using DijitaruVatigoGengar.Data.Dtos;
using DijitaruVatigoGengar.Models;
using Microsoft.AspNetCore.Mvc;

namespace DijitaruVatigoGengar.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly DijitaruVatigoGengarContext _context;
    private readonly IMapper _mapper;

    public ProjectsController(DijitaruVatigoGengarContext dbContext, IMapper mapper)
    {
        _context = dbContext;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AddProject([FromBody] CreateProjectDto projectDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var project = _mapper.Map<Project>(projectDto);
        _context.Projects.Add(project);
        _context.SaveChanges();

        var readProjectDto = _mapper.Map<ReadProjectDto>(project);
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, readProjectDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProject(int id, [FromBody] UpdateProjectDto projectDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingProject = _context.Projects.FirstOrDefault(p => p.Id == id);
        if (existingProject == null)
            return NotFound("Projeto não encontrado.");

        _mapper.Map(projectDto, existingProject);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(500, "Ocorreu um erro ao atualizar o projeto.");
        }

        var readProjectDto = _mapper.Map<ReadProjectDto>(existingProject);
        return Ok(readProjectDto);
    }

    [HttpGet]
    public IActionResult GetAllProjects()
    {
        var projects = _context.Projects.ToList();
        var readProjectDtos = _mapper.Map<List<ReadProjectDto>>(projects);
        return Ok(readProjectDtos);
    }

    [HttpGet("{id}")]
    public IActionResult GetProject(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == id);
        if (project == null)
            return NotFound("Projeto não encontrado.");

        var readProjectDto = _mapper.Map<ReadProjectDto>(project);
        return Ok(readProjectDto);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProject(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == id);
        if (project == null)
            return NotFound("Projeto não encontrado.");

        _context.Projects.Remove(project);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(500, "Ocorreu um erro ao excluir o projeto.");
        }

        return NoContent();
    }
}

using AutoMapper;
using DijitaruVatigoGengar.Data;
using DijitaruVatigoGengar.Data.Dtos;
using DijitaruVatigoGengar.Models;
using Microsoft.AspNetCore.Mvc;

namespace DijitaruVatigoGengar.Controllers;

[ApiController]
[Route("Projetos")]
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
        var project = _mapper.Map<Project>(projectDto);
        _context.Projects.Add(project);
        _context.SaveChanges();

        var readProjectDto = _mapper.Map<ReadProjectDto>(project);
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, readProjectDto);
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
            return NotFound("Projeto não encontrado");

        var readProjectDto = _mapper.Map<ReadProjectDto>(project);
        return Ok(readProjectDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProject(int id, [FromBody] UpdateProjectDto projectDto)
    {
        var existingProject = _context.Projects.FirstOrDefault(p => p.Id == id);
        if (existingProject == null)
            return NotFound("Projeto não encontrado");

        _mapper.Map(projectDto, existingProject);
        _context.SaveChanges();

        var readProjectDto = _mapper.Map<ReadProjectDto>(existingProject);
        return Ok(readProjectDto);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProject(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == id);
        if (project == null)
            return NotFound("Projeto não encontrado");

        _context.Projects.Remove(project);
        _context.SaveChanges();

        return NoContent();
    }
}
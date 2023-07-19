using DijitaruVatigoGengar.Enums;
using DijitaruVatigoGengar.Models;

namespace DijitaruVatigoGengar.Data.Dtos;

public class UpdateProjectDto
{
    public string Name { get; set; }
    public double Budget { get; set; }
    public ProjectType Type { get; set; }

}

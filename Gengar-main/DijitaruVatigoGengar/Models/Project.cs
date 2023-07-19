using DijitaruVatigoGengar.Enums;

namespace DijitaruVatigoGengar.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Budget { get; set; }
    public ProjectType Type { get; set; }

    public IList<PendingHour> PendingHours { get; set; }
    public IList<ProjectCollaborator> ProjectCollaborators { get; set; }

    public Project()
    {
        PendingHours = new List<PendingHour>();
        ProjectCollaborators = new List<ProjectCollaborator>();
    }
}
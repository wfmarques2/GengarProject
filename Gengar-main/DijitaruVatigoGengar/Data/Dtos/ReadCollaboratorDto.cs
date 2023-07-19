using DijitaruVatigoGengar.Enums;
using DijitaruVatigoGengar.Extensions;
using DijitaruVatigoGengar.Models;

namespace DijitaruVatigoGengar.Data.Dtos;

public class ReadCollaboratorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public char Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public string ModalityText { get; private set; }
    public ContractModality Modality
    {
        get { return ModalityText.ModalityToValue(); }
        set { ModalityText = value.ModalityToString(); }
    }

    public string RoleText { get; private set; }
    public Role CollaboratorRole
    {
        get { return RoleText.RoleToValue(); }
        set { RoleText = value.RoleToString(); }
    }

    public IList<PendingHour> PendingHours { get; set; }
    public IList<ProjectCollaborator> ProjectCollaborators { get; set; }
}

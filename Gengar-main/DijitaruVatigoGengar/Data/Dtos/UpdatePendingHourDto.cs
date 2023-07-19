using DijitaruVatigoGengar.Models;

namespace DijitaruVatigoGengar.Data.Dtos;

public class UpdatePendingHourDto
{
    public int CollaboratorId { get; set; }
    public int ProjectId { get; set; }
    public int HourAmount { get; set; }

}

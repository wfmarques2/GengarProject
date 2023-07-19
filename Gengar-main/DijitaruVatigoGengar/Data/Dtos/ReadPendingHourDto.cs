using DijitaruVatigoGengar.Models;

namespace DijitaruVatigoGengar.Data.Dtos;

public class ReadPendingHourDto
{
    public int Id { get; set; }
    public int CollaboratorId { get; set; }
    public int ProjectId { get; set; }
    public int HourAmount { get; set; }
    public bool IsApproved { get; set; }

   
}

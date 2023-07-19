namespace DijitaruVatigoGengar.Data.Dtos;

public class CreateApprovedHourDto
{
    public int ApproverId { get; set; }
    public int PendingHourId { get; set; }
    public bool IsApproved { get; set; }
}

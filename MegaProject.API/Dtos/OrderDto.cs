namespace MegaProject.Dtos;

public class OrderDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string WorkStatus { get; set; }

    public int BidId { get; set; }
}
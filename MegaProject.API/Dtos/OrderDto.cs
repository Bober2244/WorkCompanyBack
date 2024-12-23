namespace MegaProject.Dtos;

public class OrderDto
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string WorkStatus { get; set; }

    public int BidId { get; set; }
}
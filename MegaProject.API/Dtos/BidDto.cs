namespace MegaProject.Dtos;

public class BidDto
{
    public DateOnly DateOfRequest { get; set; } 
    public int ConstructionPeriod { get; set; } 

    public int CustomerId { get; set; } 
}
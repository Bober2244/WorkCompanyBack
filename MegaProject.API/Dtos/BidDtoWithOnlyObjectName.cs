using MegaProject.Domain.Models;

namespace MegaProject.Dtos;

public class BidDtoWithOnlyObjectName
{
    public DateOnly DateOfRequest { get; set; } 
    public int ConstructionPeriod { get; set; } 

    public int CustomerId { get; set; } 
    public Customer Customer { get; set; } 
    public ICollection<Order> Orders { get; set; } 
    public string ObjectName { get; set; }
}
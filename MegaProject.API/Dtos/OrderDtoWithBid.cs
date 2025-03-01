using MegaProject.Domain.Models;

namespace MegaProject.Dtos;

public class OrderDtoWithBid
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; } 
    public DateOnly EndDate { get; set; } 
    public string WorkStatus { get; set; } 

    public int BidId { get; set; }
    public BidDtoWithOnlyObjectName Bid { get; set; }
    public ICollection<MaterialOrder> MaterialOrders { get; set; } 

    public ICollection<BrigadeOrder> BrigadeOrders { get; set; } 
}
namespace MegaProject.Domain.Models;

//Заказ
public class Order : DatabaseEntity.DatabaseEntity
{
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; } 
    public string WorkStatus { get; set; } 

    public int BidId { get; set; }
    public Bid Bid { get; set; }
    public ICollection<MaterialOrder> MaterialOrders { get; set; } 
    public ICollection<BrigadeOrder> BrigadeOrders { get; set; } 
}
using System.Text.Json.Serialization;

namespace MegaProject.Domain.Models;

//Заказ
public class Order : DatabaseEntity.DatabaseEntity
{
    public DateOnly StartDate { get; set; } 
    public DateOnly EndDate { get; set; } 
    public string WorkStatus { get; set; } 

    [JsonIgnore]
    public int BidId { get; set; }
    [JsonIgnore]
    public Bid Bid { get; set; }
    [JsonIgnore]
    public ICollection<MaterialOrder> MaterialOrders { get; set; } 

    public ICollection<BrigadeOrder> BrigadeOrders { get; set; } 
}
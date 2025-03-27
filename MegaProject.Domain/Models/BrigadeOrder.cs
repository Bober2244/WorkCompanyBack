using System.Text.Json.Serialization;

namespace MegaProject.Domain.Models;

//Бригады в заказе
public class BrigadeOrder : DatabaseEntity.DatabaseEntity
{
    [JsonIgnore]
    public int OrderId { get; set; } 
    [JsonIgnore]
    public Order Order { get; set; } 
    
    public int BrigadeId { get; set; } 
    public Brigade Brigade { get; set; } 
    public string WorkStatus { get; set; }
    public bool IsBrigadeResponded { get; set; }
}
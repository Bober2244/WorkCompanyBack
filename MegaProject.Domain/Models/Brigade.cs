using System.Text.Json.Serialization;

namespace MegaProject.Domain.Models;

//бригада
public class Brigade : DatabaseEntity.DatabaseEntity
{
    public string Name { get; set; } 
    public int WorkerCount { get; set; } 

    public ICollection<Worker> Workers { get; set; } 
    [JsonIgnore]
    public ICollection<BrigadeOrder> BrigadeOrders { get; set; }
}
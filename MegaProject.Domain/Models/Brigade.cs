namespace MegaProject.Domain.Models;

public class Brigade : DatabaseEntity.DatabaseEntity
{
    public string Name { get; set; } 
    public int WorkerCount { get; set; } 

    public ICollection<Worker> Workers { get; set; } 
    public ICollection<BrigadeOrder> BrigadeOrders { get; set; }
}
namespace MegaProject.Domain.Models;

//Заявка
public class Bid : DatabaseEntity.DatabaseEntity
{
    public DateOnly DateOfRequest { get; set; } 
    public int ConstructionPeriod { get; set; } 

    public int CustomerId { get; set; } 
    public Customer Customer { get; set; } 
    public ICollection<Order> Orders { get; set; } 
}
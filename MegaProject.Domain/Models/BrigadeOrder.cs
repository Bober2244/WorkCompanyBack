namespace MegaProject.Domain.Models;

//Бригады в заказе
public class BrigadeOrder : DatabaseEntity.DatabaseEntity
{
    public int OrderId { get; set; } 
    public Order Order { get; set; } 
    
    public int BrigadeId { get; set; } 
    public Brigade Brigade { get; set; } 
}
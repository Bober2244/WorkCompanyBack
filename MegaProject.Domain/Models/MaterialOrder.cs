namespace MegaProject.Domain.Models;

//Материал в заказе
public class MaterialOrder : DatabaseEntity.DatabaseEntity
{
    public int Quantity { get; set; } 

    public int OrderId { get; set; } 
    public Order Order { get; set; } 
    
    public int MaterialId { get; set; } 
    public Material Material { get; set; } 
}
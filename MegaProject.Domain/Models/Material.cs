namespace MegaProject.Domain.Models;

//Материал
public class Material : DatabaseEntity.DatabaseEntity
{
    public string Name { get; set; }
    public int Quantity { get; set; } 
    public string MeasurementUnit { get; set; } 

    public ICollection<MaterialOrder> MaterialOrders { get; set; } 
    public ICollection<Purchase> Purchases { get; set; }
}
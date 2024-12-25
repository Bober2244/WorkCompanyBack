using System.Text.Json.Serialization;

namespace MegaProject.Domain.Models;

//Материал
public class Material : DatabaseEntity.DatabaseEntity
{
    public string Name { get; set; }
    public int Quantity { get; set; } 
    public string MeasurementUnit { get; set; } 

    [JsonIgnore]
    public ICollection<MaterialOrder> MaterialOrders { get; set; } 
    [JsonIgnore]
    public ICollection<Purchase> Purchases { get; set; }
}
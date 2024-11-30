namespace MegaProject.Domain.Models;

//Закупка
public class Purchase : DatabaseEntity.DatabaseEntity
{
    public DateTime DateOfPurchase { get; set; } 
    public DateTime DeliveryDate { get; set; } 
    public int PurchaseQuantity { get; set; } 

    public int MaterialId { get; set; }
    public Material Material { get; set; } 
}
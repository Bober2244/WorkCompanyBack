namespace MegaProject.Domain.Models;

//Заказчик
public class Customer : DatabaseEntity.DatabaseEntity
{
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; } 
    public string PhoneNumber { get; set; } 
    public string Email { get; set; } 

    public ICollection<Bid> Bids { get; set; } 
}
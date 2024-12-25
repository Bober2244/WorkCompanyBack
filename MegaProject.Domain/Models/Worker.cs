using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Domain.Models;

//Рабочий
public class Worker : DatabaseEntity.DatabaseEntity
{

    public string Position { get; set; } 
    public string FullName { get; set; } 
    public string PhoneNumber { get; set; } 
    public string Email { get; set; } 

    [JsonIgnore]
    public int? BrigadeId { get; set; } 
    [JsonIgnore]
    public Brigade? Brigade { get; set; } 
}
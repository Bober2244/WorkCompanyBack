namespace MegaProject.Dtos;

public class BrigadeOrderDto
{
    public int OrderId { get; set; } 
    
    public int BrigadeId { get; set; } 
    public string WorkStatus { get; set; }
    public bool IsBrigadeResponded { get; set; }
}
namespace MegaProject.Domain.Models.DatabaseEntity;

public abstract class DatabaseEntity : IdObject<int>
{
    public int Id { get; set; }
}
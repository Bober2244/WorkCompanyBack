namespace MegaProject.Domain.Models.DatabaseEntity;

public interface IdObject<T>
{
    public T Id { get; set; }
}
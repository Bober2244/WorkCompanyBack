namespace MegaProject.Services.Interfaces;

public interface IBaseService<T>
{
    Task<IEnumerable<T>> Get();
    Task<T> GetById(int id);
    Task<bool> Delete(int id);
    Task<T> Create(T entity);
    Task<bool> Update(T entity);
}
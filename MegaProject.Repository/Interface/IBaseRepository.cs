namespace MegaProject.Repository.Interface;

public interface IBaseRepository<T>
{
    Task<T> Create(T entity);
    Task<T> GetById(int id);
    Task<List<T>> Get();
    Task<bool> Delete(int id);
    Task<bool> Update(T entity);
}
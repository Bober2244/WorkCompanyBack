using MegaProject.Domain.Models;

namespace MegaProject.Repository.Interface;

public interface IBrigadeOrdersRepository : IBaseRepository<BrigadeOrder>
{
    public Task<IEnumerable<BrigadeOrder>> GetByOrderId(int id);
}
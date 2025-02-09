using MegaProject.Domain.Models;

namespace MegaProject.Repository.Interface;

public interface IBidsRepository : IBaseRepository<Bid>
{
    Task<List<Bid>> GetBidsById(int id);
}
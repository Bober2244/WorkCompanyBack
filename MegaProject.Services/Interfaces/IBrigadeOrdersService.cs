using MegaProject.Domain.Models;

namespace MegaProject.Services.Interfaces;

public interface IBrigadeOrdersService : IBaseService<BrigadeOrder>
{
    public Task<IEnumerable<BrigadeOrder>> GetByOrderId(int id);
}
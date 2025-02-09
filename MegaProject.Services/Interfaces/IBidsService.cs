using MegaProject.Domain.Models;

namespace MegaProject.Services.Interfaces;

public interface IBidsService : IBaseService<Bid>
{
    Task<List<Bid>> GetBidsById(int id);
}
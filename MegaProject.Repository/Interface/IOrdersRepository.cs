using MegaProject.Domain.Models;

namespace MegaProject.Repository.Interface;

public interface IOrdersRepository : IBaseRepository<Order>
{
    Task ApplyForOrderAsync(int orderId, int brigadeId);
}
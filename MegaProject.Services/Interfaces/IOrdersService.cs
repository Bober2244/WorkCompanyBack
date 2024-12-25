using MegaProject.Domain.Models;

namespace MegaProject.Services.Interfaces;

public interface IOrdersService : IBaseService<Order>
{
    Task ApplyForOrderAsync(int orderId, int brigadeId);
}
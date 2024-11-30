using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using MegaProject.Services.Interfaces;

namespace MegaProject.Services.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _ordersRepository;
    public OrdersService(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }
    public async Task<IEnumerable<Order>> Get()
    {
        return await _ordersRepository.Get();
    }

    public async Task<Order> GetById(int id)
    {
        return await _ordersRepository.GetById(id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _ordersRepository.Delete(id);
    }

    public async Task<Order> Create(Order entity)
    {
        return await _ordersRepository.Create(entity);
    }

    public async Task<bool> Update(Order entity)
    {
        return await _ordersRepository.Update(entity);
    }
}
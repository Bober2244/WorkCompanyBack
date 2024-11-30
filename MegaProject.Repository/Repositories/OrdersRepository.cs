using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Repository.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly AppDbContext _context;

    public OrdersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order> Create(Order entity)
    {
        await _context.Orders.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Order> GetById(int id)
    {
        return await _context.Orders.AsNoTracking().Include(w => w.Bid).Include(w => w.MaterialOrders).Include(w => w.BrigadeOrders).FirstOrDefaultAsync(w => w.Id == id);

    }

    public async Task<List<Order>> Get()
    {
        return await _context.Orders.AsNoTracking().
            Include(w => w.Bid).Include(w => w.MaterialOrders).Include(w => w.BrigadeOrders).ToListAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return false;
        }

        await _context.Orders.Where(w => w.Id == id).Include(e => e.MaterialOrders).Include(w => w.BrigadeOrders).ExecuteDeleteAsync();
        return true;
    }

    public async Task<bool> Update(Order entity)
    {
        var result = await _context.Orders
            .Where(w => w.Id == entity.Id)
            .Include(w => w.MaterialOrders)
            .Include(w => w.BrigadeOrders)
            .Include(w => w.Bid)
            .ExecuteUpdateAsync(w => w
                .SetProperty(w => w.StartDate, entity.StartDate)
                .SetProperty(w => w.EndDate, entity.EndDate)
                .SetProperty(w => w.WorkStatus, entity.WorkStatus)
                .SetProperty(w => w.MaterialOrders, entity.MaterialOrders)
                .SetProperty(w => w.BrigadeOrders, entity.BrigadeOrders)
                .SetProperty(w => w.BidId, entity.BidId));

        return result > 0;
    }
}
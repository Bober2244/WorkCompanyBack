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
    
    public async Task ApplyForOrderAsync(int orderId, int brigadeId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            throw new Exception("Заказ не найден");

        var brigade = await _context.Brigades.FindAsync(brigadeId);
        if (brigade == null)
            throw new Exception("Бригада не найдена");

        var existingRelation = await _context.BrigadeOrders
            .AnyAsync(bo => bo.OrderId == orderId && bo.BrigadeId == brigadeId);
        if (existingRelation)
            throw new Exception("Бригада уже откликнулась на этот заказ");

        var brigadeOrder = new BrigadeOrder
        {
            OrderId = orderId,
            BrigadeId = brigadeId
        };
        _context.BrigadeOrders.Add(brigadeOrder);

        order.WorkStatus = "Откликнулся";
        _context.Orders.Update(order);

        await _context.SaveChangesAsync();
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
            Include(w => w.Bid).Include(w => w.MaterialOrders).Include(w => w.BrigadeOrders).ThenInclude(bo => bo.Brigade).ToListAsync();
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
    try
    {
        var order = await _context.Orders
            .Include(o => o.MaterialOrders)
            .Include(o => o.BrigadeOrders)
            .FirstOrDefaultAsync(o => o.Id == entity.Id);

        if (order == null)
        {
            // Order not found
            return false;
        }

        // Update simple properties
        order.StartDate = entity.StartDate;
        order.EndDate = entity.EndDate;
        order.WorkStatus = entity.WorkStatus;
        order.BidId = entity.BidId;

        // Save changes to the database
        await _context.SaveChangesAsync();
        return true;
    }
    catch (Exception ex)
    {
        // Log the error
        Console.Error.WriteLine($"Error updating order: {ex.Message}");
        return false;
    }
}



}
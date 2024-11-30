using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Repository.Repositories;

public class BrigadeOrdersRepository : IBrigadeOrdersRepository
{
    private readonly AppDbContext _context;

    public BrigadeOrdersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BrigadeOrder> Create(BrigadeOrder entity)
    {
        await _context.BrigadeOrders.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<BrigadeOrder> GetById(int id)
    {
        return await _context.BrigadeOrders
            .AsNoTracking()
            .Include(bo => bo.Order)
            .Include(bo => bo.Brigade)
            .FirstOrDefaultAsync(bo => bo.Id == id);
    }

    public async Task<List<BrigadeOrder>> Get()
    {
        return await _context.BrigadeOrders
            .AsNoTracking()
            .Include(bo => bo.Order)
            .Include(bo => bo.Brigade)
            .ToListAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var brigadeOrder = await _context.BrigadeOrders.FindAsync(id);
        if (brigadeOrder == null)
        {
            return false;
        }

        _context.BrigadeOrders.Remove(brigadeOrder);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(BrigadeOrder entity)
    {
        var result = await _context.BrigadeOrders
            .Where(bo => bo.Id == entity.Id)
            .ExecuteUpdateAsync(bo => bo
                .SetProperty(bo => bo.OrderId, entity.OrderId)
                .SetProperty(bo => bo.BrigadeId, entity.BrigadeId));

        return result > 0;
    }
}

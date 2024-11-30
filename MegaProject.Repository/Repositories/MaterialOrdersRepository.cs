using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Repository.Repositories;

public class MaterialOrdersRepository : IMaterialOrdersRepository
{
    private readonly AppDbContext _context;

    public MaterialOrdersRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<MaterialOrder> Create(MaterialOrder entity)
    {
        await _context.MaterialOrders.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<MaterialOrder> GetById(int id)
    {
        return await _context.MaterialOrders.AsNoTracking().Include(w => w.Order).Include(w => w.Material).FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<List<MaterialOrder>> Get()
    {
        return await _context.MaterialOrders.AsNoTracking().
            Include(w => w.Order).Include(w => w.Material).ToListAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var materialOrder = await _context.MaterialOrders.FindAsync(id);
        if (materialOrder == null)
        {
            return false;
        }

        await _context.MaterialOrders.Where(w => w.Id == id).Include(e => e.Order).ExecuteDeleteAsync();
        return true;
    }

    public async Task<bool> Update(MaterialOrder entity)
    {
        var result = await _context.MaterialOrders
            .Where(w => w.Id == entity.Id)
            .Include(w => w.Order)
            .Include(w => w.Material)
            .ExecuteUpdateAsync(w => w
                .SetProperty(w => w.Quantity, entity.Quantity)
                .SetProperty(w => w.OrderId, entity.OrderId)
                .SetProperty(w => w.MaterialId, entity.MaterialId));

        return result > 0;
    }
}
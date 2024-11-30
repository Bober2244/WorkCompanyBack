using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Repository.Repositories;

public class MaterialsRepository : IMaterialsRepository
{
    private readonly AppDbContext _context;

    public MaterialsRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Material> Create(Material entity)
    {
        await _context.Materials.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Material> GetById(int id)
    {
        return await _context.Materials.AsNoTracking().Include(w => w.MaterialOrders).Include(w => w.Purchases).FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<List<Material>> Get()
    {
        return await _context.Materials.AsNoTracking().
            Include(w => w.MaterialOrders).Include(w => w.Purchases).ToListAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var material = await _context.Materials.FindAsync(id);
        if (material == null)
        {
            return false;
        }

        await _context.Materials.Where(w => w.Id == id).Include(e => e.MaterialOrders).Include(w => w.Purchases).ExecuteDeleteAsync();
        return true;
    }

    public async Task<bool> Update(Material entity)
    {
        var result = await _context.Materials
            .Where(w => w.Id == entity.Id)
            .Include(w => w.MaterialOrders)
            .Include(w => w.Purchases)
            .ExecuteUpdateAsync(w => w
                .SetProperty(w => w.Name, entity.Name)
                .SetProperty(w => w.Quantity, entity.Quantity)
                .SetProperty(w => w.MeasurementUnit, entity.MeasurementUnit)
                .SetProperty(w => w.MaterialOrders, entity.MaterialOrders)
                .SetProperty(w => w.Purchases, entity.Purchases));

        return result > 0;
    }
}
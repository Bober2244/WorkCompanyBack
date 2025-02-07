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
        await _context.Materials.AddAsync(new Material
        {
            Id = _context.Materials.Max(x => x.Id) + 1,
            Name = entity.Name,
            Quantity = entity.Quantity,
            MeasurementUnit = entity.MeasurementUnit,
        });
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Material> GetById(int id)
    {
        return await _context.Materials.AsNoTracking().Include(w => w.MaterialOrders).Include(w => w.Purchases)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<List<Material>> Get()
    {
        return await _context.Materials.AsNoTracking().Include(w => w.MaterialOrders).Include(w => w.Purchases)
            .ToListAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var material = await _context.Materials.FindAsync(id);
        if (material == null)
        {
            return false;
        }

        await _context.Materials.Where(w => w.Id == id).Include(e => e.MaterialOrders).Include(w => w.Purchases)
            .ExecuteDeleteAsync();
        return true;
    }

    public async Task<bool> Update(Material entity)
    {
        try
        {
            var material = await _context.Materials
                .Include(o => o.MaterialOrders)
                .Include(o => o.Purchases)
                .FirstOrDefaultAsync(o => o.Id == entity.Id);

            if (material == null)
            {
                // Order not found
                return false;
            }

            material.Name = entity.Name;
            material.Quantity = entity.Quantity;
            material.MeasurementUnit = entity.MeasurementUnit;

            // Save changes to the database
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Log the error
            Console.Error.WriteLine($"Error updating material: {ex.Message}");
            return false;
        }
    }
}
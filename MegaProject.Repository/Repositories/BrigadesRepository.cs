using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Repository.Repositories;

public class BrigadesRepository : IBrigadesRepository
{
    private readonly AppDbContext _context;

    public BrigadesRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Brigade> Create(Brigade entity)
    {
        await _context.Brigades.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Brigade> GetById(int id)
    {
        return await _context.Brigades.AsNoTracking().Include(w => w.Workers).Include(w => w.BrigadeOrders).FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<List<Brigade>> Get()
    {
        return await _context.Brigades.AsNoTracking().
            Include(w => w.Workers).Include(w => w.BrigadeOrders).ToListAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var brigade = await _context.Brigades.FindAsync(id);
        if (brigade == null)
        {
            return false;
        }

        await _context.Brigades.Where(w => w.Id == id).Include(e => e.BrigadeOrders).ExecuteDeleteAsync();
        return true;
    }

    public async Task<bool> Update(Brigade entity)
    {
        var result = await _context.Brigades
            .Where(w => w.Id == entity.Id)
            .Include(w => w.BrigadeOrders)
            .Include(w => w.Workers)
            .ExecuteUpdateAsync(w => w
                .SetProperty(w => w.Name, entity.Name)
                .SetProperty(w => w.WorkerCount, entity.WorkerCount)
                .SetProperty(w => w.Workers, entity.Workers)
                .SetProperty(w => w.BrigadeOrders, entity.BrigadeOrders));

        return result > 0;
    }
}
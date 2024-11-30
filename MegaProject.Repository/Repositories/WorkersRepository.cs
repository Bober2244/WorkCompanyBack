using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Repository.Repositories;

public class WorkersRepository : IWorkersRepository
{
    private readonly AppDbContext _context;

    public WorkersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Worker> Create(Worker entity)
    {
        await _context.Workers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Worker> GetById(int id)
    {
        return await _context.Workers.AsNoTracking().Include(w => w.Brigade).FirstOrDefaultAsync(w => w.Id == id);

    }

    public async Task<List<Worker>> Get()
    {
        return await _context.Workers.AsNoTracking().
            Include(w => w.Brigade).ToListAsync();    }

    public async Task<bool> Delete(int id)
    {
        var worker = await _context.Workers.FindAsync(id);
        if (worker == null)
        {
            return false;
        }

        await _context.Workers.Where(w => w.Id == id).Include(e => e.Brigade).ExecuteDeleteAsync();
        return true;
    }

    public async Task<bool> Update(Worker entity)
    {
        var result = await _context.Workers
            .Where(w => w.Id == entity.Id)
            .Include(w => w.Brigade)
            .ExecuteUpdateAsync(w => w
                .SetProperty(w => w.Position, entity.Position)
                .SetProperty(w => w.FullName, entity.FullName)
                .SetProperty(w => w.PhoneNumber, entity.PhoneNumber)
                .SetProperty(w => w.Email, entity.Email)
                .SetProperty(w => w.BrigadeId, entity.BrigadeId));

        return result > 0;
    }

}
using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Repository.Repositories;

public class BidsRepository : IBidsRepository
{
    private readonly AppDbContext _context;

    public BidsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Bid> Create(Bid entity)
    {
        var newEntity = new Bid
        {
            Id = _context.Bids.Max(x => x.Id) + 1,
            DateOfRequest = entity.DateOfRequest,
            ConstructionPeriod = entity.ConstructionPeriod,
            CustomerId = entity.CustomerId,
            ObjectName = entity.ObjectName
        };
        await _context.Bids.AddAsync(newEntity);
        await _context.SaveChangesAsync();
        return newEntity;
    }

    public async Task<Bid> GetById(int id)
    {
        return await _context.Bids
            .AsNoTracking()
            .Include(bo => bo.Customer)
            .Include(bo => bo.Orders)
            .FirstOrDefaultAsync(bo => bo.Id == id);
    }
    
    public async Task<List<Bid>> GetBidsById(int id)
    {
        return await _context.Bids
            .AsNoTracking()
            .Include(bo => bo.Customer)
            .Include(bo => bo.Orders)
            .ThenInclude(bo => bo.BrigadeOrders)
            .ThenInclude(bo => bo.Brigade)
            .Where(bo => bo.CustomerId == id)
            .ToListAsync();
    }

    public async Task<List<Bid>> Get()
    {
        return await _context.Bids
            .AsNoTracking()
            .Include(bo => bo.Customer)
            .Include(bo => bo.Orders)
            .ThenInclude(bo => bo.BrigadeOrders)
            .ThenInclude(bo => bo.Brigade)
            .ToListAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var bid = await _context.Bids.FindAsync(id);
        if (bid == null)
        {
            return false;
        }

        _context.Bids.Remove(bid);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(Bid entity)
    {
        var result = await _context.Bids
            .Where(bo => bo.Id == entity.Id)
            .ExecuteUpdateAsync(bo => bo
                .SetProperty(bo => bo.DateOfRequest, entity.DateOfRequest)
                .SetProperty(bo => bo.ConstructionPeriod, entity.ConstructionPeriod)
                .SetProperty(bo => bo.CustomerId, entity.CustomerId));

        return result > 0;
    }
}
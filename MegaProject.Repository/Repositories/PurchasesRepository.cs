using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Repository.Repositories;

public class PurchasesRepository : IPurchasesRepository
{
    private readonly AppDbContext _context;

    public PurchasesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Purchase> Create(Purchase entity)
    {
        await _context.Purchases.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Purchase> GetById(int id)
    {
        return await _context.Purchases.AsNoTracking().Include(w => w.Material).FirstOrDefaultAsync(w => w.Id == id);

    }

    public async Task<List<Purchase>> Get()
    {
        return await _context.Purchases.AsNoTracking().
            Include(w => w.Material).ToListAsync();    
    }

    public async Task<bool> Delete(int id)
    {
        var purchase = await _context.Purchases.FindAsync(id);
        if (purchase == null)
        {
            return false;
        }

        await _context.Purchases.Where(w => w.Id == id).Include(e => e.Material).ExecuteDeleteAsync();
        return true;
    }

    public async Task<bool> Update(Purchase entity)
    {
        var result = await _context.Purchases
            .Where(w => w.Id == entity.Id)
            .Include(w => w.Material)
            .ExecuteUpdateAsync(w => w
                .SetProperty(w => w.DateOfPurchase, entity.DateOfPurchase)
                .SetProperty(w => w.PurchaseQuantity, entity.PurchaseQuantity)
                .SetProperty(w => w.MaterialId, entity.MaterialId));

        return result > 0;
    }
}
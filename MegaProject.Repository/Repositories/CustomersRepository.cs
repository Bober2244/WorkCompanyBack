using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Repository.Repositories;

public class CustomersRepository : ICustomersRepository
{
    private readonly AppDbContext _context;

    public CustomersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> Create(Customer entity)
    {
        await _context.Customers.AddAsync(new Customer
            {
                Id = _context.Users.Max(x => x.Id),
                FullName = entity.FullName,
                DateOfBirth = DateTime.SpecifyKind(entity.DateOfBirth, DateTimeKind.Utc),
                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email,
            }
            );
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Customer> GetById(int id)
    {
        return await _context.Customers.AsNoTracking().Include(w => w.Bids).FirstOrDefaultAsync(w => w.Id == id);

    }

    public async Task<List<Customer>> Get()
    {
        return await _context.Customers.AsNoTracking().
            Include(w => w.Bids).ToListAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return false;
        }

        await _context.Customers.Where(w => w.Id == id).Include(e => e.Bids).ExecuteDeleteAsync();
        return true;
    }

    public async Task<bool> Update(Customer entity)
    {
        var result = await _context.Customers
            .Where(w => w.Id == entity.Id)
            .Include(w => w.Bids)
            .ExecuteUpdateAsync(w => w
                .SetProperty(w => w.FullName, entity.FullName)
                .SetProperty(w => w.DateOfBirth, entity.DateOfBirth)
                .SetProperty(w => w.PhoneNumber, entity.PhoneNumber)
                .SetProperty(w => w.Email, entity.Email)
                .SetProperty(w => w.Bids, entity.Bids));

        return result > 0;
    }
}
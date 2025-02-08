using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Repository.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _context;

    public UsersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> Create(User entity)
    {
        await _context.Users.AddAsync(new User
        {
            Id = _context.Users.Max(x => x.Id) + 1,
            UserName = entity.UserName,
            Email = entity.Email,
            Password = entity.Password,
            Role = entity.Role,
            
        });
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<User> GetById(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> Get()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Delete(int id)
    {
        await _context.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync();
        return true;
    }

    public async Task<bool> Update(User entity)
    {
        await _context.Users
            .Where(u => u.Id == entity.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.UserName, entity.UserName)
                .SetProperty(u => u.Email, entity.Email)
                .SetProperty(u => u.Password, entity.Password)
                .SetProperty(u => u.Role, entity.Role));
        return true;
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
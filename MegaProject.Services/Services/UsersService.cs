using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using MegaProject.Services.Interfaces;

namespace MegaProject.Services.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    
    public async Task<IEnumerable<User>> Get()
    {
        var users = await _usersRepository.Get();
        return users;
    }

    public async Task<User> GetById(int id)
    {
        return await _usersRepository.GetById(id);
    }
    
    public async Task<User> Create(User entity)
    {
        return await _usersRepository.Create(entity);
    }

    public async Task<bool> Delete(int id)
    {
        return await _usersRepository.Delete(id);
    }
    public async Task<bool> Update(User entity)
    {
        return await _usersRepository.Update(entity);
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _usersRepository.GetByEmail(email);
    }
}
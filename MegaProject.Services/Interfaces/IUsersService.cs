using MegaProject.Domain.Models;

namespace MegaProject.Services.Interfaces;

public interface IUsersService : IBaseService<User>
{
    Task<User> GetByEmail(string email);
}
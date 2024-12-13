using MegaProject.Domain.Models;

namespace MegaProject.Repository.Interface;

public interface IUsersRepository : IBaseRepository<User>
{
    Task<User> GetByEmail(string email);
}
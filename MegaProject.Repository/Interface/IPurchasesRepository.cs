using MegaProject.Domain.Models;

namespace MegaProject.Repository.Interface;

public interface IPurchasesRepository : IBaseRepository<Purchase>
{
    public Task<byte[]> GetSmeta();
}
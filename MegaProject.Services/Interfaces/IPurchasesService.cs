using MegaProject.Domain.Models;

namespace MegaProject.Services.Interfaces;

public interface IPurchasesService : IBaseService<Purchase>
{
    public Task<byte[]> GetSmeta();
}
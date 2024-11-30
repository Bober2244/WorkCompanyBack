using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using MegaProject.Services.Interfaces;

namespace MegaProject.Services.Services;

public class PurchasesService : IPurchasesService
{
    private readonly IPurchasesRepository _purchasesRepository;

    public PurchasesService(IPurchasesRepository purchasesRepository)
    {
        _purchasesRepository = purchasesRepository;
    }

    public async Task<IEnumerable<Purchase>> Get()
    {
        return await _purchasesRepository.Get();
    }

    public async Task<Purchase> GetById(int id)
    {
        return await _purchasesRepository.GetById(id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _purchasesRepository.Delete(id);
    }

    public async Task<Purchase> Create(Purchase entity)
    {
        return await _purchasesRepository.Create(entity);
    }

    public async Task<bool> Update(Purchase entity)
    {
        return await _purchasesRepository.Update(entity);
    }
}
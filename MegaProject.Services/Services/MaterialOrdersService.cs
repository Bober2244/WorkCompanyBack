using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using MegaProject.Services.Interfaces;

namespace MegaProject.Services.Services;

public class MaterialOrdersService : IMaterialOrdersService
{
    private readonly IMaterialOrdersRepository _materialOrdersRepository;

    public MaterialOrdersService(IMaterialOrdersRepository materialOrdersRepository)
    {
        _materialOrdersRepository = materialOrdersRepository;
    }

    public async Task<IEnumerable<MaterialOrder>> Get()
    {
        return await _materialOrdersRepository.Get();
    }

    public async Task<MaterialOrder> GetById(int id)
    {
        return await _materialOrdersRepository.GetById(id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _materialOrdersRepository.Delete(id);
    }

    public async Task<MaterialOrder> Create(MaterialOrder entity)
    {
        return await _materialOrdersRepository.Create(entity);
    }

    public async Task<bool> Update(MaterialOrder entity)
    {
        return await _materialOrdersRepository.Update(entity);
    }
}
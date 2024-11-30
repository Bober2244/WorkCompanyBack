using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using MegaProject.Services.Interfaces;

namespace MegaProject.Services.Services;

public class MaterialsService : IMaterialsService
{
    private readonly IMaterialsRepository _materialsRepository;

    public MaterialsService(IMaterialsRepository materialsRepository)
    {
        _materialsRepository = materialsRepository;
    }

    public async Task<IEnumerable<Material>> Get()
    {
        return await _materialsRepository.Get();
    }

    public async Task<Material> GetById(int id)
    {
        return await _materialsRepository.GetById(id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _materialsRepository.Delete(id);
    }

    public async Task<Material> Create(Material entity)
    {
        return await _materialsRepository.Create(entity);
    }

    public async Task<bool> Update(Material entity)
    {
        return await _materialsRepository.Update(entity);
    }
}
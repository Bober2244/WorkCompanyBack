using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using MegaProject.Services.Interfaces;

namespace MegaProject.Services.Services;

public class BrigadesService : IBrigadesService
{
    private readonly IBrigadesRepository _brigadesRepository;

    public BrigadesService(IBrigadesRepository brigadesRepository)
    {
        _brigadesRepository = brigadesRepository;
    }

    public async Task<IEnumerable<Brigade>> Get()
    {
        return await _brigadesRepository.Get();
    }

    public async Task<Brigade> GetById(int id)
    {
        return await _brigadesRepository.GetById(id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _brigadesRepository.Delete(id);
    }

    public async Task<Brigade> Create(Brigade entity)
    {
        return await _brigadesRepository.Create(entity);
    }

    public async Task<bool> Update(Brigade entity)
    {
        return await _brigadesRepository.Update(entity);
    }
}
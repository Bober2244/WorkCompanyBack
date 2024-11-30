using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using MegaProject.Services.Interfaces;

namespace MegaProject.Services.Services;

public class BrigadeOrdersService : IBrigadeOrdersService
{
    private readonly IBrigadeOrdersRepository _repository;

    public BrigadeOrdersService(IBrigadeOrdersRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BrigadeOrder>> Get()
    {
        return await _repository.Get();
    }

    public async Task<BrigadeOrder> GetById(int id)
    {
        return await _repository.GetById(id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _repository.Delete(id);
    }

    public async Task<BrigadeOrder> Create(BrigadeOrder entity)
    {
        return await _repository.Create(entity);
    }

    public async Task<bool> Update(BrigadeOrder entity)
    {
        return await _repository.Update(entity);
    }
}
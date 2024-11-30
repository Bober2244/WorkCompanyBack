using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using MegaProject.Services.Interfaces;

namespace MegaProject.Services.Services;

public class WorkersService : IWorkersService
{
    private readonly IWorkersRepository _workersRepository;

    public WorkersService(IWorkersRepository workersRepository)
    {
        _workersRepository = workersRepository;
    }

    public async Task<IEnumerable<Worker>> Get()
    {
        return await _workersRepository.Get();
    }

    public async Task<Worker> GetById(int id)
    {
        return await _workersRepository.GetById(id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _workersRepository.Delete(id);
    }

    public async Task<Worker> Create(Worker entity)
    {
        return await _workersRepository.Create(entity);
    }

    public async Task<bool> Update(Worker entity)
    {
        return await _workersRepository.Update(entity);
    }
}
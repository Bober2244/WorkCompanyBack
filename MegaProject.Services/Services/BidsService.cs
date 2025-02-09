using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using MegaProject.Services.Interfaces;

namespace MegaProject.Services.Services;

public class BidsService : IBidsService
{
    private readonly IBidsRepository _bidsRepository;

    public BidsService(IBidsRepository bidsRepository)
    {
        _bidsRepository = bidsRepository;
    }

    public async Task<IEnumerable<Bid>> Get()
    {
        return await _bidsRepository.Get();
    }

    public async Task<List<Bid>> GetBidsById(int id)
    {
        return await _bidsRepository.GetBidsById(id);
    }

    public async Task<Bid> GetById(int id)
    {
        return await _bidsRepository.GetById(id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _bidsRepository.Delete(id);
    }

    public async Task<Bid> Create(Bid entity)
    {
        return await _bidsRepository.Create(entity);
    }

    public async Task<bool> Update(Bid entity)
    {
        return await _bidsRepository.Update(entity);
    }
}
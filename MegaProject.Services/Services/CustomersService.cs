using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using MegaProject.Services.Interfaces;

namespace MegaProject.Services.Services;

public class CustomersService : ICustomersService
{
    private readonly ICustomersRepository _customersRepository;

    public CustomersService(ICustomersRepository customersRepository)
    {
        _customersRepository = customersRepository;
    }
    public async Task<IEnumerable<Customer>> Get()
    {
        return await _customersRepository.Get();
    }

    public async Task<Customer> GetById(int id)
    {
        return await _customersRepository.GetById(id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _customersRepository.Delete(id);
    }

    public async Task<Customer> Create(Customer entity)
    {
        return await _customersRepository.Create(entity);
    }

    public async Task<bool> Update(Customer entity)
    {
        return await _customersRepository.Update(entity);
    }
}
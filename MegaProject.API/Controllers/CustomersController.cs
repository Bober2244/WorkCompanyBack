using Microsoft.AspNetCore.Mvc;
using MegaProject.Domain.Models;
using MegaProject.Dtos;
using MegaProject.Services;
using MegaProject.Services.Interfaces;

namespace MegaProject.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomersService _customersService;

    public CustomersController(ICustomersService customersService)
    {
        _customersService = customersService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _customersService.Get();
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _customersService.GetById(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
    {
        var customer = MapToEntity(customerDto);
        var createdCustomer = await _customersService.Create(customer);
        return Created($"/customers/{createdCustomer.Id}", MapToDto(createdCustomer));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
    {
        var existingCustomer = await _customersService.GetById(id);
        if (existingCustomer == null) return NotFound();

        var updatedCustomer = MapToEntity(customerDto);
        updatedCustomer.Id = id;
        await _customersService.Update(updatedCustomer);
        return Ok(MapToDto(updatedCustomer));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var isDeleted = await _customersService.Delete(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }

    private CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email
        };
    }

    private Customer MapToEntity(CustomerDto customerDto)
    {
        return new Customer
        {
            FullName = customerDto.FullName,
            DateOfBirth = customerDto.DateOfBirth,
            PhoneNumber = customerDto.PhoneNumber,
            Email = customerDto.Email
        };
    }
}

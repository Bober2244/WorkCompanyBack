using MegaProject.Domain.Models;
using MegaProject.Dtos;
using MegaProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MegaProject.Controllers;

[ApiController]
[Route("[controller]")]
public class BrigadeOrdersController : ControllerBase
{
    private readonly IBrigadeOrdersService _brigadeOrdersService;

    public BrigadeOrdersController(IBrigadeOrdersService brigadeOrdersService)
    {
        _brigadeOrdersService = brigadeOrdersService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrigadeOrders()
    {
        var brigadeOrders = await _brigadeOrdersService.Get();
        return Ok(brigadeOrders);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBrigadeOrderById(int id)
    {
        var brigadeOrder = await _brigadeOrdersService.GetById(id);
        if (brigadeOrder == null) return NotFound();
        return Ok(brigadeOrder);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrigadeOrder([FromBody] BrigadeOrderDto brigadeOrderDto)
    {
        var brigadeOrder = MapToEntity(brigadeOrderDto);
        var createdbrigadeOrder = await _brigadeOrdersService.Create(brigadeOrder);
        return Created($"/workers/{createdbrigadeOrder.Id}", MapToDto(createdbrigadeOrder));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateBrigadeOrder(int id, [FromBody] BrigadeOrderDto brigadeOrderDto)
    {
        var existingBrigadeOrder = await _brigadeOrdersService.GetById(id);
        if (existingBrigadeOrder == null) return NotFound();
        var updatedBrigadeOrder = MapToEntity(brigadeOrderDto);
        updatedBrigadeOrder.Id = id;
        await _brigadeOrdersService.Update(updatedBrigadeOrder);
        return Ok(brigadeOrderDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBrigadeOrder(int id)
    {
        var isDeleted = await _brigadeOrdersService.Delete(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }

    private BrigadeOrderDto MapToDto(BrigadeOrder brigadeOrder)
    {
        return new BrigadeOrderDto
        {
            OrderId = brigadeOrder.OrderId,
            BrigadeId = brigadeOrder.BrigadeId
        };
    }

    private BrigadeOrder MapToEntity(BrigadeOrderDto brigadeOrderDto)
    {
        return new BrigadeOrder
        {
            OrderId = brigadeOrderDto.OrderId,
            BrigadeId = brigadeOrderDto.BrigadeId
        };
    }
}
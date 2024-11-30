using Microsoft.AspNetCore.Mvc;
using MegaProject.Domain.Models;
using MegaProject.Dtos;
using MegaProject.Services;
using MegaProject.Services.Interfaces;

namespace MegaProject.Controllers;

[ApiController]
[Route("[controller]")]
public class MaterialOrdersController : ControllerBase
{
    private readonly IMaterialOrdersService _materialOrdersService;

    public MaterialOrdersController(IMaterialOrdersService materialOrdersService)
    {
        _materialOrdersService = materialOrdersService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMaterialOrders()
    {
        var materialOrders = await _materialOrdersService.Get();
        return Ok(materialOrders);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMaterialOrderById(int id)
    {
        var materialOrder = await _materialOrdersService.GetById(id);
        if (materialOrder == null) return NotFound();
        return Ok(materialOrder);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMaterialOrder([FromBody] MaterialOrderDto materialOrderDto)
    {
        var materialOrder = MapToEntity(materialOrderDto);
        var createdMaterialOrder = await _materialOrdersService.Create(materialOrder);
        return Created($"/material-orders/{createdMaterialOrder.Id}", MapToDto(createdMaterialOrder));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateMaterialOrder(int id, [FromBody] MaterialOrderDto materialOrderDto)
    {
        var existingMaterialOrder = await _materialOrdersService.GetById(id);
        if (existingMaterialOrder == null) return NotFound();

        var updatedMaterialOrder = MapToEntity(materialOrderDto);
        updatedMaterialOrder.Id = id;
        await _materialOrdersService.Update(updatedMaterialOrder);
        return Ok(MapToDto(updatedMaterialOrder));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMaterialOrder(int id)
    {
        var isDeleted = await _materialOrdersService.Delete(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }

    private MaterialOrderDto MapToDto(MaterialOrder materialOrder)
    {
        return new MaterialOrderDto
        {
            Quantity = materialOrder.Quantity,
            OrderId = materialOrder.OrderId,
            MaterialId = materialOrder.MaterialId
        };
    }

    private MaterialOrder MapToEntity(MaterialOrderDto materialOrderDto)
    {
        return new MaterialOrder
        {
            Quantity = materialOrderDto.Quantity,
            OrderId = materialOrderDto.OrderId,
            MaterialId = materialOrderDto.MaterialId
        };
    }
}

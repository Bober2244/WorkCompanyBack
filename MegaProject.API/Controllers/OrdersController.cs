using Microsoft.AspNetCore.Mvc;
using MegaProject.Domain.Models;
using MegaProject.Dtos;
using MegaProject.Services;
using MegaProject.Services.Interfaces;

namespace MegaProject.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService _ordersService;

    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _ordersService.Get();
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _ordersService.GetById(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
    {
        var order = MapToEntity(orderDto);
        var createdOrder = await _ordersService.Create(order);
        return Created($"/orders/{createdOrder.Id}", MapToDto(createdOrder));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
    {
        var existingOrder = await _ordersService.GetById(id);
        if (existingOrder == null) return NotFound();

        var updatedOrder = MapToEntity(orderDto);
        updatedOrder.Id = id;
        await _ordersService.Update(updatedOrder);
        return Ok(MapToDto(updatedOrder));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var isDeleted = await _ordersService.Delete(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }

    private OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            StartDate = order.StartDate,
            EndDate = order.EndDate,
            WorkStatus = order.WorkStatus,
            BidId = order.BidId
        };
    }

    private Order MapToEntity(OrderDto orderDto)
    {
        return new Order
        {
            StartDate = orderDto.StartDate,
            EndDate = orderDto.EndDate,
            WorkStatus = orderDto.WorkStatus,
            BidId = orderDto.BidId
        };
    }
}

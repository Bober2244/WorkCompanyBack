using Microsoft.AspNetCore.Mvc;
using MegaProject.Domain.Models;
using MegaProject.Dtos;
using MegaProject.Repository;
using MegaProject.Services;
using MegaProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MegaProject.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService _ordersService;
    private readonly AppDbContext _context;

    public OrdersController(IOrdersService ordersService, AppDbContext context)
    {
        _ordersService = ordersService;
        _context = context;
    }
    
    [HttpPatch("{orderId:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
        {
            return NotFound($"Заказ с ID {orderId} не найден.");
        }

        order.WorkStatus = newStatus;

        await _context.SaveChangesAsync();

        return Ok(new { Message = "Статус заказа успешно обновлен.", OrderId = orderId, NewStatus = newStatus });
    }


    [HttpPost("{orderId:int}/apply")]
    public async Task<IActionResult> ApplyForOrder(int orderId, [FromBody] int brigadeId)
    {
        try
        {
            await _ordersService.ApplyForOrderAsync(orderId, brigadeId);
            return Ok(new { Message = "Бригада успешно откликнулась на заказ." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Ошибка при отклике на заказ.", Details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _ordersService.Get();
    
        var orderDtos = orders.Select(order => new OrderDtoWithBid
        {
            Id = order.Id,
            StartDate = order.StartDate,
            EndDate = order.EndDate,
            WorkStatus = order.WorkStatus,
            BidId = order.BidId,

            Bid = order.Bid == null ? null : new BidDtoWithOnlyObjectName
            {
                DateOfRequest = order.Bid.DateOfRequest,
                ConstructionPeriod = order.Bid.ConstructionPeriod,
                CustomerId = order.Bid.CustomerId,
                Customer = order.Bid.Customer,
                Orders = order.Bid.Orders,
                ObjectName = order.Bid.ObjectName
            },

            MaterialOrders = order.MaterialOrders,
            BrigadeOrders = order.BrigadeOrders
        }).ToList();

        return Ok(orderDtos);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _ordersService.GetById(id);
        if (order == null)
            return NotFound();

        var orderDto = new OrderDtoWithBid
        {
            Id = order.Id,
            StartDate = order.StartDate,
            EndDate = order.EndDate,
            WorkStatus = order.WorkStatus,
            BidId = order.BidId,

            Bid = new BidDtoWithOnlyObjectName
            {
                ObjectName = order.Bid?.ObjectName
            },

            BrigadeOrders = order.BrigadeOrders
        };

        return Ok(orderDto);
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
            BidId = orderDto.BidId,
        };
    }
    
    [HttpGet("{orderId}/materials")]
    public async Task<IActionResult> GetMaterialsForOrder(int orderId)
    {
        // Находим заказ с указанным ID, включая привязанные материалы
        var order = await _context.Orders
            .Include(o => o.MaterialOrders)
            .ThenInclude(mo => mo.Material)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            return NotFound($"Заказ с ID {orderId} не найден.");
        }

        // Формируем список материалов для ответа
        var materials = order.MaterialOrders.Select(mo => new
        {
            MaterialId = mo.MaterialId,
            Name = mo.Material.Name,
            Quantity = mo.Quantity,
            MeasurementUnit = mo.Material.MeasurementUnit
        });

        return Ok(materials);
    }


    [HttpPost("{orderId}/materials")]
    public async Task<IActionResult> AttachMaterialToOrder(int orderId, [FromBody] AttachMaterialDto dto)
    {
        var order = await _context.Orders
            .Include(o => o.MaterialOrders) // Загружаем связанные записи
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            return NotFound($"Заказ с ID {orderId} не найден.");
        }

        // Проверяем существование материала
        var material = await _context.Materials.FindAsync(dto.MaterialId);
        if (material == null)
        {
            return NotFound($"Материал с ID {dto.MaterialId} не найден.");
        }

        // Проверяем достаточное количество материала
        if (dto.Quantity > material.Quantity)
        {
            return BadRequest($"Недостаточно материала. Доступно: {material.Quantity} {material.MeasurementUnit}.");
        }

        // Привязываем материал к заказу
        var existingMaterialOrder = order.MaterialOrders
            .FirstOrDefault(mo => mo.MaterialId == dto.MaterialId);

        if (existingMaterialOrder != null)
        {
            // Если материал уже привязан, обновляем количество
            existingMaterialOrder.Quantity += dto.Quantity;
        }
        else
        {
            // Создаем новую запись
            var materialOrder = new MaterialOrder
            {
                OrderId = orderId,
                MaterialId = dto.MaterialId,
                Quantity = dto.Quantity
            };
            order.MaterialOrders.Add(materialOrder);
        }

        // Обновляем количество материала на складе
        material.Quantity -= dto.Quantity;

        // Сохраняем изменения
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Материал успешно привязан к заказу.",
            MaterialId = dto.MaterialId,
            OrderId = orderId,
            RemainingMaterialQuantity = material.Quantity
        });
    }

    public class AttachMaterialDto
    {
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
    }
}

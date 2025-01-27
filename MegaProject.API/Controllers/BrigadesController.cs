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
public class BrigadesController : ControllerBase
{
    private readonly IBrigadesService _brigadesService;
    private readonly AppDbContext _context;

    public BrigadesController(IBrigadesService brigadesService, AppDbContext context)
    {
        _brigadesService = brigadesService;
        _context = context;
    }
    
    
    
    [HttpGet("user-brigade/{userId:int}")]
    public async Task<IActionResult> GetBrigadeByUserId(int userId)
    {
        // Проверяем, существует ли пользователь с указанным userId
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return NotFound("Пользователь с указанным ID не найден.");
        }

        // Получаем бригаду, связанную с этим userId, включая связанные данные
        var brigade = await _context.Brigades
            .Include(b => b.Workers)          // Загрузить связанных работников
            .Include(b => b.BrigadeOrders)   // Загрузить заказы, связанные с бригадой
            .FirstOrDefaultAsync(b => b.UserId == userId);

        if (brigade == null)
        {
            return NotFound("Бригада для указанного пользователя не найдена.");
        }

        return Ok(brigade); // Возвращаем сущность полностью
    }

    [HttpGet]
    public async Task<IActionResult> GetBrigades()
    {
        var brigades = await _brigadesService.Get();
        return Ok(brigades);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBrigadeById(int id)
    {
        var brigade = await _brigadesService.GetById(id);
        if (brigade == null) return NotFound();
        return Ok(brigade);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrigade([FromBody] BrigadeDto brigadeDto)
    {
        var brigade = MapToEntity(brigadeDto);
        var createdBrigade = await _brigadesService.Create(brigade);
        return Created($"/brigades/{createdBrigade.Id}", MapToDto(createdBrigade));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateBrigade(int id, [FromBody] BrigadeDto brigadeDto)
    {
        var existingBrigade = await _brigadesService.GetById(id);
        if (existingBrigade == null) return NotFound();

        var updatedBrigade = MapToEntity(brigadeDto);
        updatedBrigade.Id = id;
        await _brigadesService.Update(updatedBrigade);
        return Ok(MapToDto(updatedBrigade));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBrigade(int id)
    {
        var isDeleted = await _brigadesService.Delete(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }

    private BrigadeDto MapToDto(Brigade brigade)
    {
        return new BrigadeDto
        {
            Name = brigade.Name,
            WorkerCount = brigade.WorkerCount,
            UserId = brigade.UserId
        };
    }

    private Brigade MapToEntity(BrigadeDto brigadeDto)
    {
        return new Brigade
        {
            Name = brigadeDto.Name,
            WorkerCount = brigadeDto.WorkerCount,
            UserId = brigadeDto.UserId
            
        };
    }
}
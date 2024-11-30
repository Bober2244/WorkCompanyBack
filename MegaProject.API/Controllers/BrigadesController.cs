using Microsoft.AspNetCore.Mvc;
using MegaProject.Domain.Models;
using MegaProject.Dtos;
using MegaProject.Services;
using MegaProject.Services.Interfaces;

namespace MegaProject.Controllers;

[ApiController]
[Route("[controller]")]
public class BrigadesController : ControllerBase
{
    private readonly IBrigadesService _brigadesService;

    public BrigadesController(IBrigadesService brigadesService)
    {
        _brigadesService = brigadesService;
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
            WorkerCount = brigade.WorkerCount
        };
    }

    private Brigade MapToEntity(BrigadeDto brigadeDto)
    {
        return new Brigade
        {
            Name = brigadeDto.Name,
            WorkerCount = brigadeDto.WorkerCount
        };
    }
}
using MegaProject.Domain.Models;
using MegaProject.Services.Interfaces;
using MegaProject.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MegaProject.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkersController : ControllerBase
{
    private readonly IWorkersService _workersService;

    public WorkersController(IWorkersService workersService)
    {
        _workersService = workersService;
    }

    [HttpGet]
    public async Task<IActionResult> GetWorkers()
    {
        var workers = await _workersService.Get();
        return Ok(workers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWorkerById(int id)
    {
        var worker = await _workersService.GetById(id);
        if (worker == null) return NotFound();
        return Ok(worker);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorker([FromBody] WorkerDto workerDto)
    {
        var worker = MapToEntity(workerDto);
        var createdWorker = await _workersService.Create(worker);
        return Created($"/workers/{createdWorker.Id}", MapToDto(createdWorker));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateWorker(int id, [FromBody] WorkerDto workerDto)
    {
        var existingWorker = await _workersService.GetById(id);
        if (existingWorker == null) return NotFound();
        var updatedWorker = MapToEntity(workerDto);
        updatedWorker.Id = id;
        await _workersService.Update(updatedWorker);
        return Ok(workerDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWorker(int id)
    {
        var isDeleted = await _workersService.Delete(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }

    private WorkerDto MapToDto(Worker worker)
    {
        return new WorkerDto
        {
            Position = worker.Position,
            FullName = worker.FullName,
            PhoneNumber = worker.PhoneNumber,
            Email = worker.Email,
            BrigadeId = worker.BrigadeId
        };
    }

    private Worker MapToEntity(WorkerDto workerDto)
    {
        return new Worker
        {
            Position = workerDto.Position,
            FullName = workerDto.FullName,
            PhoneNumber = workerDto.PhoneNumber,
            Email = workerDto.Email,
            BrigadeId = workerDto.BrigadeId
        };
    }
}

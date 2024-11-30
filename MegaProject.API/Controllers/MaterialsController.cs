using MegaProject.Domain.Models;
using MegaProject.Dtos;
using MegaProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MegaProject.Controllers;

[ApiController]
[Route("[controller]")]
public class MaterialsController : ControllerBase
{
     private readonly IMaterialsService _materialsService;

    public MaterialsController(IMaterialsService materialsService)
    {
        _materialsService = materialsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMaterials()
    {
        var materials = await _materialsService.Get();
        return Ok(materials);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMaterialById(int id)
    {
        var material = await _materialsService.GetById(id);
        if (material == null) return NotFound();
        return Ok(material);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMaterial([FromBody] MaterialDto materialDto)
    {
        var material = MapToEntity(materialDto);
        var createdMaterial = await _materialsService.Create(material);
        return Created($"/materials/{createdMaterial.Id}", MapToDto(createdMaterial));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateMaterial(int id, [FromBody] MaterialDto materialDto)
    {
        var existingMaterial = await _materialsService.GetById(id);
        if (existingMaterial == null) return NotFound();

        var updatedMaterial = MapToEntity(materialDto);
        updatedMaterial.Id = id;
        await _materialsService.Update(updatedMaterial);
        return Ok(MapToDto(updatedMaterial));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        var isDeleted = await _materialsService.Delete(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }

    private MaterialDto MapToDto(Material material)
    {
        return new MaterialDto
        {
            Name = material.Name,
            Quantity = material.Quantity,
            MeasurementUnit = material.MeasurementUnit
        };
    }

    private Material MapToEntity(MaterialDto materialDto)
    {
        return new Material
        {
            Name = materialDto.Name,
            Quantity = materialDto.Quantity,
            MeasurementUnit = materialDto.MeasurementUnit
        };
    }
    
}
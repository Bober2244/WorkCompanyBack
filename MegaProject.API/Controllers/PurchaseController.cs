using MegaProject.Domain.Models;
using MegaProject.Services.Interfaces;
using MegaProject.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MegaProject.Controllers;

[ApiController]
[Route("[controller]")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchasesService _purchasesService;

    public PurchaseController(IPurchasesService purchasesService)
    {
        _purchasesService = purchasesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPurchases()
    {
        var purchases = await _purchasesService.Get();
        return Ok(purchases);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPurchaseById(int id)
    {
        var purchase = await _purchasesService.GetById(id);
        if (purchase == null) return NotFound();
        return Ok(purchase);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePurchase([FromBody] PurchaseDto purchaseDto)
    {
        var purchase = MapToEntity(purchaseDto);
        var createdPurchase = await _purchasesService.Create(purchase);
        return Created($"/workers/{createdPurchase.Id}", MapToDto(createdPurchase));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdatePurchase(int id, [FromBody] PurchaseDto purchaseDto)
    {
        var existingPurchase = await _purchasesService.GetById(id);
        if (existingPurchase == null) return NotFound();
        var updatedPurchase = MapToEntity(purchaseDto);
        updatedPurchase.Id = id;
        await _purchasesService.Update(updatedPurchase);
        return Ok(purchaseDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePurchase(int id)
    {
        var isDeleted = await _purchasesService.Delete(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }

    private PurchaseDto MapToDto(Purchase purchase)
    {
        return new PurchaseDto
        {
            DateOfPurchase = purchase.DateOfPurchase,
            PurchaseQuantity = purchase.PurchaseQuantity,
            MaterialId = purchase.MaterialId
        };
    }

    private Purchase MapToEntity(PurchaseDto purchaseDto)
    {
        return new Purchase
        {
            DateOfPurchase = purchaseDto.DateOfPurchase,
            PurchaseQuantity = purchaseDto.PurchaseQuantity,
            MaterialId = purchaseDto.MaterialId
        };
    }
}
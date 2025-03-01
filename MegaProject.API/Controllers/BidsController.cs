using Microsoft.AspNetCore.Mvc;
using MegaProject.Domain.Models;
using MegaProject.Dtos;
using MegaProject.Repository.Repositories;
using MegaProject.Services.Interfaces;

namespace MegaProject.Controllers;

[ApiController]
[Route("[controller]")]
public class BidsController : ControllerBase
{
    private readonly IBidsService _bidsService;

    public BidsController(IBidsService bidsService)
    {
        _bidsService = bidsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBids()
    {
        var bids = await _bidsService.Get();
        return Ok(bids);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBidsById(int id)
    {
        var bids = await _bidsService.GetBidsById(id);
        if (bids == null) return NotFound();
        return Ok(bids);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBid([FromBody] BidDto bidDto)
    {
        var bid = MapToEntity(bidDto);
        var createdBid = await _bidsService.Create(bid);
        return Created($"/bids/{createdBid.Id}", MapToDto(createdBid));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateBid(int id, [FromBody] BidDto bidDto)
    {
        var existingBid = await _bidsService.GetById(id);
        if (existingBid == null) return NotFound();

        var updatedBid = MapToEntity(bidDto);
        updatedBid.Id = id;
        await _bidsService.Update(updatedBid);
        return Ok(MapToDto(updatedBid));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBid(int id)
    {
        var isDeleted = await _bidsService.Delete(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }

    private BidDto MapToDto(Bid bid)
    {
        return new BidDto
        {
            DateOfRequest = bid.DateOfRequest,
            ConstructionPeriod = bid.ConstructionPeriod,
            CustomerId = bid.CustomerId,
            ObjectName = bid.ObjectName,
        };
    }

    private Bid MapToEntity(BidDto bidDto)
    {
        return new Bid
        {
            DateOfRequest = bidDto.DateOfRequest,
            ConstructionPeriod = bidDto.ConstructionPeriod,
            CustomerId = bidDto.CustomerId,
            ObjectName = bidDto.ObjectName
        };
    }
}

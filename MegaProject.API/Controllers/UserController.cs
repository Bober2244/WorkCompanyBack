using MegaProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MegaProject.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UserController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _usersService.GetById(id);
        if (user == null) return NotFound();
        return Ok(user);
    }
}
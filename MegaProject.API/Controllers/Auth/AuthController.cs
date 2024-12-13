using System.IdentityModel.Tokens.Jwt;
using MegaProject.Domain.Enums;
using MegaProject.Domain.Models;
using MegaProject.Dtos.Auth;
using MegaProject.Services.Helpers;
using MegaProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MegaProject.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly JwtService _jwtService;

    public AuthController(IUsersService usersService, JwtService jwtService)
    {
        _usersService = usersService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto regDto)
    {
        // Валидация данных модели
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }
        
        var newUser = new User
        {
            UserName = regDto.UserName,
            Email = regDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(regDto.Password),
            Role = (TypeRole)regDto.Role 
        };
    
        var createdUser = await _usersService.Create(newUser);

        return Ok(new
        {
            message = "User registered successfully",
            createdUser
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto logDto)
    {
        var user = await _usersService.GetByEmail(logDto.Email);
        if (user == null) return BadRequest(new { message = "Invalid credentials" });

        if (!BCrypt.Net.BCrypt.Verify(logDto.Password, user.Password))
        {
            return BadRequest(new { message = "Invalid credentials" });
        }

        var jwt = _jwtService.Generate(user.Id, user.Role);
        Response.Cookies.Append("jwt", jwt, new CookieOptions
        {
            HttpOnly = true
        });

        return Ok(new
        {
            message = "success",
            id = user.Id,
            role = (int)user.Role,
            jwt
        });
    }

    [HttpGet("user")]
    [Authorize]
    public async Task<IActionResult> User()
    {
        var token = Request.Cookies["jwt"];
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
        }

        JwtSecurityToken jwtToken;
        try
        {
            jwtToken = _jwtService.Verify(token);
        }
        catch
        {
            return Unauthorized();
        }

        if (jwtToken == null)
        {
            return Unauthorized();
        }

        int userId = int.Parse(jwtToken.Subject);
        var user = await _usersService.GetById(userId);
        var jwt = jwtToken.RawData;
        return Ok(new
        {
            user,
            jwt
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return Ok(new
        {
            message = "success"
        });
    }

    [HttpGet("admin")]
    [Authorize(Policy = "AdminPolicy")]
    public IActionResult AdminEndpoint()
    {
        try
        {
            return Ok("Вы админ");
        }
        catch (Exception e)
        {
            return Unauthorized();
        }
        
    }
}
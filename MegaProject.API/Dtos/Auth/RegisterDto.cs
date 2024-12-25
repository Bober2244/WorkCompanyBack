using System.ComponentModel.DataAnnotations;
using MegaProject.Domain.Enums;

namespace MegaProject.Dtos.Auth;

public class RegisterDto
{
    [Required] 
    public string UserName { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required, Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = String.Empty;
    
    [Required] 
    public int Role { get; set; }
    
    
    
}
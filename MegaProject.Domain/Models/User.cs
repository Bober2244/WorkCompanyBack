using MegaProject.Domain.Enums;

namespace MegaProject.Domain.Models;

// Пользователь
public class User : DatabaseEntity.DatabaseEntity
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public TypeRole Role { get; set; } = TypeRole.BrigadeController;
}
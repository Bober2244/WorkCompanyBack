using System.ComponentModel.DataAnnotations;

namespace MegaProject;

public class RoleValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is int intValue)
        {
            return intValue == 0 || intValue == 1;  // Только 0 или 1
        }
        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"Поле {name} должно быть равно 0 (Бригадир) или 1 (Заказчик)";
    }
}
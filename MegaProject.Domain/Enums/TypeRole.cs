using System.ComponentModel.DataAnnotations;

namespace MegaProject.Domain.Enums;

public enum TypeRole
{
    [Display(Name = "Бригадир")]
    BrigadeController = 0,
    [Display(Name = "Заказчик")]
    Customer = 1,
    [Display(Name = "Админ")]
    Admin = 2
}
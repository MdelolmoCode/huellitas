using System.ComponentModel.DataAnnotations;

namespace Huellitas.Models;

public enum AnimalStatus
{
    [Display(Name = "Disponible")]
    Available,

    [Display(Name = "Adoptado")]
    Adopted
}

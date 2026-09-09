using System.ComponentModel.DataAnnotations;

namespace Huellitas.Models;

public enum AnimalSex
{
    [Display(Name = "Desconocido")]
    Unknown,

    [Display(Name = "Macho")]
    Male,

    [Display(Name = "Hembra")]
    Female
}

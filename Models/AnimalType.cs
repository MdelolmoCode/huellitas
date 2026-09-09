using System.ComponentModel.DataAnnotations;

namespace Huellitas.Models;

public enum AnimalType
{
    [Display(Name = "Perro")]
    Dog,

    [Display(Name = "Gato")]
    Cat,

    [Display(Name = "Conejo")]
    Rabbit,

    [Display(Name = "Ave")]
    Bird,

    [Display(Name = "Otro")]
    Other
}

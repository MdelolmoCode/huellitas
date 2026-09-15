using System.ComponentModel.DataAnnotations;

namespace Huellitas.ViewModels.AdoptionRequests;

public sealed class AdoptionRequestFormViewModel
{
    public int AnimalId { get; set; }

    public string AnimalName { get; set; } = string.Empty;

    [Display(Name = "Motivo")]
    [Required(ErrorMessage = "El motivo es obligatorio.")]
    [StringLength(1000, MinimumLength = 30,
        ErrorMessage = "El motivo debe tener entre 30 y 1000 caracteres.")]
    public string Reason { get; set; } = string.Empty;

    [Display(Name = "Ya convivo con otros animales")]
    public bool HasOtherAnimals { get; set; }
}

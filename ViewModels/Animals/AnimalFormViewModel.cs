using System.ComponentModel.DataAnnotations;
using Huellitas.Models;

namespace Huellitas.ViewModels.Animals;

public sealed class AnimalFormViewModel : IValidatableObject
{
    public int Id { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(80, ErrorMessage = "El nombre no puede superar los 80 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Tipo")]
    [Required(ErrorMessage = "El tipo es obligatorio.")]
    [EnumDataType(typeof(AnimalType), ErrorMessage = "Elige un tipo válido.")]
    public AnimalType? AnimalType { get; set; }

    [Display(Name = "Raza")]
    [StringLength(100, ErrorMessage = "La raza no puede superar los 100 caracteres.")]
    public string? Breed { get; set; }

    [Display(Name = "Edad aproximada")]
    [Required(ErrorMessage = "La edad aproximada es obligatoria.")]
    [Range(0, 40, ErrorMessage = "La edad aproximada debe estar entre 0 y 40.")]
    public int? ApproximateAge { get; set; }

    [Display(Name = "Sexo")]
    [Required(ErrorMessage = "El sexo es obligatorio.")]
    [EnumDataType(typeof(AnimalSex), ErrorMessage = "Elige un sexo válido.")]
    public AnimalSex? Sex { get; set; }

    [Display(Name = "Descripción")]
    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(2000, MinimumLength = 20,
        ErrorMessage = "La descripción debe tener entre 20 y 2000 caracteres.")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Fecha de entrada")]
    [Required(ErrorMessage = "La fecha de entrada es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime? EntryDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EntryDate is not null && EntryDate.Value.Date > DateTime.Today)
        {
            yield return new ValidationResult(
                "La fecha de entrada no puede ser futura.",
                new[] { nameof(EntryDate) });
        }
    }

    public static AnimalFormViewModel FromAnimal(Animal animal)
    {
        return new AnimalFormViewModel
        {
            Id = animal.Id,
            Name = animal.Name,
            AnimalType = animal.AnimalType,
            Breed = animal.Breed,
            ApproximateAge = animal.ApproximateAge,
            Sex = animal.Sex,
            Description = animal.Description,
            EntryDate = animal.EntryDate
        };
    }

    public Animal ToAnimal()
    {
        return new Animal
        {
            Id = Id,
            Name = Name,
            AnimalType = AnimalType!.Value,
            Breed = Breed,
            ApproximateAge = ApproximateAge!.Value,
            Sex = Sex!.Value,
            Description = Description,
            EntryDate = EntryDate!.Value
        };
    }
}

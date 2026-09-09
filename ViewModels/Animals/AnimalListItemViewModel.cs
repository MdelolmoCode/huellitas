using Huellitas.Models;

namespace Huellitas.ViewModels.Animals;

public sealed class AnimalListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Breed { get; set; }
    public int ApproximateAge { get; set; }
    public AnimalType AnimalType { get; set; }
    public AnimalSex AnimalSex { get; set; }
    public AnimalStatus AnimalStatus { get; set; }
}

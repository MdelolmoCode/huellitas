using Huellitas.Models;

namespace Huellitas.ViewModels.Animals;

public sealed class AnimalDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Breed { get; set; }
    public int ApproximateAge { get; set; }
    public AnimalType AnimalType { get; set; }
    public AnimalSex AnimalSex { get; set; }
    public AnimalStatus AnimalStatus { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
}

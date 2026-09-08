namespace Huellitas.Models;

public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public AnimalType AnimalType { get; set; }
    public string? Breed { get; set; }
    public int ApproximateAge { get; set; } // en años
    public AnimalSex Sex { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
    public AnimalStatus Status { get; set; }
}
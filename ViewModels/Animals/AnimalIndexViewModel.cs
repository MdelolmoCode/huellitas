using Huellitas.Models;

namespace Huellitas.ViewModels.Animals;

public sealed class AnimalIndexViewModel
{
    public List<AnimalListItemViewModel> Items { get; set; }
        = new List<AnimalListItemViewModel>();

    public string? Search { get; set; }

    public AnimalType? AnimalType { get; set; }

    public AnimalSex? Sex { get; set; }

    public AnimalStatus? Status { get; set; }

    public string? Sort { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }
}

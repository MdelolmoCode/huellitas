using Huellitas.Models;

namespace Huellitas.ViewModels.AdoptionRequests;

public sealed class AdoptionRequestListItemViewModel
{
    public int Id { get; set; }

    public int AnimalId { get; set; }

    public string AnimalName { get; set; } = string.Empty;

    public string UserDisplayName { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; }

    public AdoptionRequestStatus Status { get; set; }
}

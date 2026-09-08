namespace Huellitas.Models;

public class AdoptionRequest
{
    public int Id { get; set; }

    public int AnimalId { get; set; } // clave foránea
    public Animal Animal { get; set; } = null!; // propiedad de navegación

    public string UserId { get; set; } = string.Empty; // clave foránea
    public ApplicationUser User { get; set; } = null!; // propiedad de navegación

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public string Reason { get; set; } = string.Empty;
    public bool HasOtherAnimals { get; set; }
    public AdoptionRequestStatus Status { get; set; } = AdoptionRequestStatus.Pending;
}

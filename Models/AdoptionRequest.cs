namespace Huellitas.Models;

public class AdoptionRequest
{
    public int Id { get; set; }
    public DateTime SubmittedAt { get; set; }
    public string Reason { get; set; } = string.Empty;
    public bool HasOtherAnimals { get; set; }
    public AdoptionRequestStatus Status { get; set; }
}
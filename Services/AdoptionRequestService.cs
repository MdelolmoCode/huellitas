using Huellitas.Data;
using Huellitas.Models;
using Huellitas.ViewModels.AdoptionRequests;
using Microsoft.EntityFrameworkCore;

namespace Huellitas.Services;

public sealed class AdoptionRequestService
{
    private readonly ApplicationDbContext context;

    public AdoptionRequestService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<OperationResult> SubmitAsync(
        int animalId,
        string userId,
        AdoptionRequestFormViewModel model)
    {
        var animal = await context.Animals
            .SingleOrDefaultAsync(animal => animal.Id == animalId);

        if (animal is null)
        {
            return OperationResult.NotFound();
        }

        if (animal.Status != AnimalStatus.Available)
        {
            return OperationResult.Conflict(
                "Este animal ya no está disponible para adopción.");
        }

        var alreadyRequested = await context.AdoptionRequests
            .AnyAsync(request =>
                request.AnimalId == animalId &&
                request.UserId == userId &&
                request.Status == AdoptionRequestStatus.Pending);

        if (alreadyRequested)
        {
            return OperationResult.Conflict(
                "Ya tienes una solicitud pendiente para este animal.");
        }

        var adoptionRequest = new AdoptionRequest
        {
            AnimalId = animalId,
            UserId = userId,
            Reason = model.Reason,
            HasOtherAnimals = model.HasOtherAnimals,
            SubmittedAt = DateTime.UtcNow,
            Status = AdoptionRequestStatus.Pending
        };

        context.AdoptionRequests.Add(adoptionRequest);
        await context.SaveChangesAsync();

        return OperationResult.Success();
    }
}

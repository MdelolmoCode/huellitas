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

    public async Task<MyAdoptionRequestsViewModel> GetForUserAsync(string userId)
    {
        var items = await context.AdoptionRequests
            .AsNoTracking()
            .Where(request => request.UserId == userId)
            .OrderByDescending(request => request.SubmittedAt)
            .ThenBy(request => request.Id)
            .Select(request => new AdoptionRequestListItemViewModel
            {
                Id = request.Id,
                AnimalId = request.AnimalId,
                AnimalName = request.Animal.Name,
                SubmittedAt = request.SubmittedAt,
                Status = request.Status
            })
            .ToListAsync();

        return new MyAdoptionRequestsViewModel { Items = items };
    }

    public async Task<AdoptionRequestDetailsViewModel?> GetDetailsForUserAsync(
        int id,
        string userId)
    {
        return await context.AdoptionRequests
            .AsNoTracking()
            .Where(request => request.Id == id && request.UserId == userId)
            .Select(request => new AdoptionRequestDetailsViewModel
            {
                Id = request.Id,
                AnimalId = request.AnimalId,
                AnimalName = request.Animal.Name,
                UserDisplayName = request.User.DisplayName,
                SubmittedAt = request.SubmittedAt,
                Reason = request.Reason,
                HasOtherAnimals = request.HasOtherAnimals,
                Status = request.Status,
                CanCancel = request.Status == AdoptionRequestStatus.Pending
            })
            .SingleOrDefaultAsync();
    }

    public async Task<OperationResult> CancelAsync(int id, string userId)
    {
        var request = await context.AdoptionRequests
            .SingleOrDefaultAsync(request =>
                request.Id == id && request.UserId == userId);

        if (request is null)
        {
            return OperationResult.NotFound();
        }

        if (request.Status != AdoptionRequestStatus.Pending)
        {
            return OperationResult.Conflict(
                "Solo se pueden cancelar las solicitudes pendientes.");
        }

        request.Status = AdoptionRequestStatus.Cancelled;

        await context.SaveChangesAsync();

        return OperationResult.Success();
    }
}

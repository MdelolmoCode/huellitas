using Huellitas.Services;
using Huellitas.Utilities;
using Huellitas.ViewModels.AdoptionRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Huellitas.Controllers;

[Authorize]
public class AdoptionRequestsController : Controller
{
    private readonly AdoptionRequestService adoptionRequests;
    private readonly AnimalService animals;

    public AdoptionRequestsController(
        AdoptionRequestService adoptionRequests,
        AnimalService animals)
    {
        this.adoptionRequests = adoptionRequests;
        this.animals = animals;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int animalId)
    {
        var animal = await animals.GetDetailsAsync(animalId);

        if (animal is null)
        {
            return NotFound();
        }

        var model = new AdoptionRequestFormViewModel
        {
            AnimalId = animal.Id,
            AnimalName = animal.Name
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromQuery] int animalId,
        AdoptionRequestFormViewModel model)
    {
        if (animalId != model.AnimalId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            var animal = await animals.GetDetailsAsync(animalId);
            model.AnimalName = animal?.Name ?? string.Empty;

            return View(model);
        }

        var result = await adoptionRequests.SubmitAsync(
            animalId, User.GetRequiredUserId(), model);

        if (result.Status == OperationStatus.NotFound)
        {
            return NotFound();
        }

        if (result.Status == OperationStatus.Conflict)
        {
            TempData["Error"] = result.Error;

            return RedirectToAction("Details", "Animals", new { id = animalId });
        }

        TempData["Success"] = "Tu solicitud de adopción se ha enviado.";

        return RedirectToAction("Details", "Animals", new { id = animalId });
    }
}

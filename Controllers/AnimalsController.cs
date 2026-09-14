using Huellitas.Models;
using Microsoft.AspNetCore.Authorization;
using Huellitas.Services;
using Huellitas.ViewModels.Animals;
using Microsoft.AspNetCore.Mvc;

namespace Huellitas.Controllers;

public class AnimalsController : Controller
{
    private readonly AnimalService animals;

    public AnimalsController(AnimalService animals)
    {
        this.animals = animals;
    }

    public async Task<IActionResult> Index(
        string? search,
        AnimalType? animalType,
        AnimalSex? sex,
        AnimalStatus? status,
        string? sort,
        int page = 1)
    {
        var model = await animals.GetIndexAsync(
            search, animalType, sex, status, sort, page);

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await animals.GetDetailsAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet]
    public IActionResult Create()
    {
        return View(new AnimalFormViewModel());
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(AnimalFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var created = await animals.CreateAsync(model.ToAnimal());

        return RedirectToAction(nameof(Details), new { id = created.Id });
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var animal = await animals.GetForEditAsync(id);

        if (animal is null)
        {
            return NotFound();
        }

        return View(AnimalFormViewModel.FromAnimal(animal));
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost]
    public async Task<IActionResult> Edit(
        [FromRoute] int id,
        AnimalFormViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var updated = await animals.UpdateAsync(id, model.ToAnimal());

        if (!updated)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await animals.GetDetailsAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await animals.DeleteAsync(id);

        if (result.Status == OperationStatus.NotFound)
        {
            return NotFound();
        }

        if (result.Status == OperationStatus.Conflict)
        {
            TempData["Error"] = result.Error;

            return RedirectToAction(nameof(Details), new { id });
        }

        TempData["Success"] = "El animal se ha borrado.";

        return RedirectToAction(nameof(Index));
    }
}

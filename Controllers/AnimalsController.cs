using Huellitas.Models;
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
        AnimalStatus? status
    )
    {
        var entities = await animals.SearchAsync(search, animalType, sex, status);

        var model = new AnimalIndexViewModel
        {
            Items = entities.Select(ToListItem).ToList(),
            Search = search,
            AnimalType = animalType,
            Sex = sex,
            Status = status
        };

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var animal = await animals.GetByIdAsync(id);

        if (animal is null)
        {
            return NotFound();
        }

        return View(ToDetails(animal));
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AnimalFormViewModel());
    }

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

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var animal = await animals.GetByIdAsync(id);

        if (animal is null)
        {
            return NotFound();
        }

        return View(AnimalFormViewModel.FromAnimal(animal));
    }

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

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var animal = await animals.GetByIdAsync(id);

        if (animal is null)
        {
            return NotFound();
        }

        return View(ToDetails(animal));
    }

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

    private static AnimalListItemViewModel ToListItem(Animal animal)
    {
        return new AnimalListItemViewModel
        {
            Id = animal.Id,
            Name = animal.Name,
            Breed = animal.Breed,
            ApproximateAge = animal.ApproximateAge,
            AnimalType = animal.AnimalType,
            AnimalSex = animal.Sex,
            AnimalStatus = animal.Status
        };
    }

    private static AnimalDetailsViewModel ToDetails(Animal animal)
    {
        return new AnimalDetailsViewModel
        {
            Id = animal.Id,
            Name = animal.Name,
            Breed = animal.Breed,
            ApproximateAge = animal.ApproximateAge,
            AnimalType = animal.AnimalType,
            AnimalSex = animal.Sex,
            AnimalStatus = animal.Status,
            Description = animal.Description,
            EntryDate = animal.EntryDate
        };
    }
}

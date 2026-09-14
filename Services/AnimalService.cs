using Huellitas.Data;
using Huellitas.Models;
using Huellitas.ViewModels.Animals;
using Microsoft.EntityFrameworkCore;

namespace Huellitas.Services;

public sealed class AnimalService
{
    private const int PageSize = 6;

    private readonly ApplicationDbContext context;

    public AnimalService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<AnimalIndexViewModel> GetIndexAsync(
        string? search,
        AnimalType? animalType,
        AnimalSex? sex,
        AnimalStatus? status,
        string? sort,
        int page)
    {
        var query = context.Animals.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var text = search.Trim();

            query = query.Where(animal =>
                EF.Functions.Like(animal.Name, $"%{text}%") ||
                (animal.Breed != null && EF.Functions.Like(animal.Breed, $"%{text}%")));
        }

        if (animalType is not null)
        {
            query = query.Where(animal => animal.AnimalType == animalType);
        }

        if (sex is not null)
        {
            query = query.Where(animal => animal.Sex == sex);
        }

        if (status is AnimalStatus statusValue)
        {
            query = query.Where(animal => animal.Status == statusValue);
        }

        var totalCount = await query.CountAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

        var currentPage = totalPages == 0
            ? 1
            : Math.Clamp(page, 1, totalPages);

        var ordered = sort switch
        {
            "age" => query.OrderBy(animal => animal.ApproximateAge),
            "entry" => query.OrderByDescending(animal => animal.EntryDate),
            _ => query.OrderBy(animal => animal.Name)
        };

        var items = await ordered
            .ThenBy(animal => animal.Id)
            .Skip((currentPage - 1) * PageSize)
            .Take(PageSize)
            .Select(animal => new AnimalListItemViewModel
            {
                Id = animal.Id,
                Name = animal.Name,
                Breed = animal.Breed,
                ApproximateAge = animal.ApproximateAge,
                AnimalType = animal.AnimalType,
                AnimalSex = animal.Sex,
                AnimalStatus = animal.Status
            })
            .ToListAsync();

        return new AnimalIndexViewModel
        {
            Items = items,
            Search = search,
            AnimalType = animalType,
            Sex = sex,
            Status = status,
            Sort = sort,
            CurrentPage = currentPage,
            TotalPages = totalPages,
            TotalCount = totalCount
        };
    }

    public async Task<Animal> CreateAsync(Animal animal)
    {
        animal.Status = AnimalStatus.Available;

        context.Animals.Add(animal);
        await context.SaveChangesAsync();

        return animal;
    }

    public async Task<bool> UpdateAsync(int id, Animal source)
    {
        var animal = await context.Animals
            .SingleOrDefaultAsync(animal => animal.Id == id);

        if (animal is null)
        {
            return false;
        }

        animal.Name = source.Name;
        animal.AnimalType = source.AnimalType;
        animal.Breed = source.Breed;
        animal.ApproximateAge = source.ApproximateAge;
        animal.Sex = source.Sex;
        animal.Description = source.Description;
        animal.EntryDate = source.EntryDate;

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<OperationResult> DeleteAsync(int id)
    {
        var animal = await context.Animals
            .SingleOrDefaultAsync(animal => animal.Id == id);

        if (animal is null)
        {
            return OperationResult.NotFound();
        }

        var hasRequests = await context.AdoptionRequests
            .AnyAsync(request => request.AnimalId == id);

        if (hasRequests)
        {
            return OperationResult.Conflict(
                "Este animal no se puede borrar porque tiene solicitudes de adopción."
                );
        }

        context.Animals.Remove(animal);
        await context.SaveChangesAsync();

        return OperationResult.Success();
    }

    public async Task<AnimalDetailsViewModel?> GetDetailsAsync(int id)
    {
        return await context.Animals
            .AsNoTracking()
            .Where(animal => animal.Id == id)
            .Select(animal => new AnimalDetailsViewModel
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
            })
            .SingleOrDefaultAsync();
    }

    public async Task<Animal?> GetForEditAsync(int id)
    {
        return await context.Animals
            .AsNoTracking()
            .SingleOrDefaultAsync(animal => animal.Id == id);
    }
}

using Huellitas.Data;
using Huellitas.Models;
using Microsoft.EntityFrameworkCore;

namespace Huellitas.Services;

public sealed class AnimalService
{
    private readonly ApplicationDbContext context;

    public AnimalService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Animal>> GetAllAsync()
    {
        return await context.Animals
            .AsNoTracking()
            .OrderBy(animal => animal.Name)
            .ThenBy(animal => animal.Id)
            .ToListAsync();
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

    public async Task<Animal?> GetByIdAsync(int id)
    {
        return await context.Animals
            .AsNoTracking()
            .Where(animal => animal.Id == id)
            .SingleOrDefaultAsync();
    }
}

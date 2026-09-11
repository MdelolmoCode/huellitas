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

    public async Task<Animal?> GetByIdAsync(int id)
    {
        return await context.Animals
            .AsNoTracking()
            .Where(animal => animal.Id == id)
            .SingleOrDefaultAsync();
    }
}

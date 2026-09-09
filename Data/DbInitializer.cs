using Huellitas.Models;
using Microsoft.EntityFrameworkCore;

namespace Huellitas.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var configuration = services.GetRequiredService<IConfiguration>();

        await context.Database.MigrateAsync();

        if (!configuration.GetValue("SeedData:Enabled", false))
        {
            return;
        }

        if (!await context.Animals.AnyAsync())
        {
            context.Animals.AddRange(BuildDemoAnimals());
            await context.SaveChangesAsync();
        }
    }

    private static List<Animal> BuildDemoAnimals()
    {
        return new List<Animal>
        {
            new Animal { Name = "Buddy", AnimalType = AnimalType.Dog, Sex = AnimalSex.Male, Breed = "Labrador Retriever", ApproximateAge = 3, EntryDate = DateTime.Today.AddDays(-40), Description = "Sociable y con mucha energía. Camina bien con correa y se lleva bien con los niños." },
            new Animal { Name = "Milo", AnimalType = AnimalType.Cat, Sex = AnimalSex.Male, Breed = "Europeo de pelo corto", ApproximateAge = 2, EntryDate = DateTime.Today.AddDays(-35), Description = "Tranquilo e independiente. Prefiere una casa sin otros gatos." },
            new Animal { Name = "Kiwi", AnimalType = AnimalType.Bird, Sex = AnimalSex.Unknown, Breed = null, ApproximateAge = 1, EntryDate = DateTime.Today.AddDays(-30), Description = "Muy activo y curioso. Necesita una jaula amplia y compañía a diario." },
            new Animal { Name = "Coco", AnimalType = AnimalType.Rabbit, Sex = AnimalSex.Female, Breed = "Enano", ApproximateAge = 1, EntryDate = DateTime.Today.AddDays(-28), Description = "Dócil y fácil de manejar. Ya está acostumbrado a vivir dentro de casa." },
            new Animal { Name = "Luna", AnimalType = AnimalType.Dog, Sex = AnimalSex.Female, Breed = "Border Collie", ApproximateAge = 5, EntryDate = DateTime.Today.AddDays(-25), Description = "Muy inteligente y necesita ejercicio a diario. Conoce las órdenes básicas." },
            new Animal { Name = "Nala", AnimalType = AnimalType.Cat, Sex = AnimalSex.Female, Breed = "Siamés", ApproximateAge = 4, EntryDate = DateTime.Today.AddDays(-21), Description = "Cariñosa y habladora. Busca compañía casi todo el día." },
            new Animal { Name = "Rocky", AnimalType = AnimalType.Dog, Sex = AnimalSex.Male, Breed = "Pastor alemán", ApproximateAge = 7, EntryDate = DateTime.Today.AddDays(-18), Description = "Leal y protector. Necesita a alguien con experiencia y espacio." },
            new Animal { Name = "Daisy", AnimalType = AnimalType.Other, Sex = AnimalSex.Female, Breed = null, ApproximateAge = 2, EntryDate = DateTime.Today.AddDays(-14), Description = "Cobaya tranquila y sociable. Mejor adoptarla acompañada." },
            new Animal { Name = "Simba", AnimalType = AnimalType.Cat, Sex = AnimalSex.Male, Breed = "Maine Coon", ApproximateAge = 6, EntryDate = DateTime.Today.AddDays(-10), Description = "Grande y tranquilo. Se lleva bien con perros y con niños mayores." },
            new Animal { Name = "Toby", AnimalType = AnimalType.Dog, Sex = AnimalSex.Female, Breed = "Jack Russell", ApproximateAge = 4, EntryDate = DateTime.Today.AddDays(-5), Description = "Juguetona y despierta. Vale para un piso con paseos regulares." }
        };
    }
}

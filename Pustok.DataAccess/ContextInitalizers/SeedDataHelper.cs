using Microsoft.EntityFrameworkCore;
using Pustok.Core.Entites;
using Pustok.DataAccess.Helpers;

namespace Pustok.DataAccess.ContextInitalizers;

internal static class SeedDataHelper
{

    public static void AddSeedData(this ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);

        var category = new Category()
        {   
            Id = Guid.Parse("51e03370-dd1b-4b8d-aa74-fab5d463cf3d"),
            Name = "Default Category"
        };

        modelBuilder.Entity<Category>().HasData(category);


        modelBuilder.Entity<Gender>().HasData(StaticData.MaleGender, StaticData.FemaleGender, StaticData.MechanicalGender);

    }
}

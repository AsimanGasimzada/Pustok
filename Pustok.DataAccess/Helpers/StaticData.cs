using Pustok.Core.Entites;

namespace Pustok.DataAccess.Helpers;

public static class StaticData
{

    public static Gender MaleGender = new()
    {
        Id = Guid.Parse("491ce33b-1749-4964-b3e7-28ca163e4a6f"),
        Name = "Male"
    };

    public static Gender FemaleGender = new()
    {
        Id = Guid.Parse("5c08bb40-5462-4625-b9e0-47a9d4544990"),
        Name = "Female"
    };



    public static Gender MechanicalGender = new()
    {
        Id = Guid.Parse("b92e8dda-8c53-4b56-810a-fb5b1061e18f"),
        Name = "Mechanical"
    };





}

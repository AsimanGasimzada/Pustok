using Microsoft.AspNetCore.Identity;

namespace Pustok.Core.Entites;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; } = null!;
}
using Microsoft.AspNetCore.Identity;

namespace Pustok.Core.Entites;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpiredDate { get; set; }
}
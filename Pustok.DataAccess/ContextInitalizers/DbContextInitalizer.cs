using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pustok.Core.Entites;
using Pustok.Core.Enums;
using Pustok.DataAccess.Abstractions;
using Pustok.DataAccess.Contexts;

namespace Pustok.DataAccess.ContextInitalizers;

internal class DbContextInitalizer : IContextInitalizer
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly string _adminPassword;
    private readonly string _adminEmail;
    private readonly string _adminFullname;
    private readonly string _adminUsername;
    public DbContextInitalizer(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;


        var section = _configuration.GetSection("AdminSettings");

        _adminPassword = section.GetValue<string>("Password") ?? "";
        _adminEmail = section.GetValue<string>("Email") ?? "";
        _adminFullname = section.GetValue<string>("Fullname") ?? "";
        _adminUsername = section.GetValue<string>("UserName") ?? "";
    }


    public async Task InitDatabaseAsync()
    {


        await _context.Database.MigrateAsync();
        await CreateRolesAsync();
        await CreateAdminAsync();
    }

    private async Task CreateAdminAsync()
    {
        AppUser adminUser = new()
        {
            Fullname = _adminFullname,
            Email = _adminEmail,
            UserName = _adminUsername
            //GenderId=StaticData.MaleGender.Id
        };


        var result = await _userManager.CreateAsync(adminUser, _adminPassword);


        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(adminUser, IdentityRoles.Admin.ToString());
        }
    }

    private async Task CreateRolesAsync()
    {
        foreach (string role in Enum.GetNames(typeof(IdentityRoles)))
        {
            await _roleManager.CreateAsync(new()
            {
                Name = role
            });
        }
    }
}

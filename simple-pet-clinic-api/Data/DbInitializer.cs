using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using simple_pet_clinic_api.Models.Entities;
using simple_pet_clinic_api.Models.Enums;

namespace simple_pet_clinic_api.Data;

public class DbInitializer : IDbInitializer
{
    private readonly AppDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    
    public DbInitializer(AppDbContext context, UserManager<UserEntity> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task InitializeAsync()
    {
        
        foreach (string roleName in Enum.GetNames(typeof(UserRole))) 
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }

        string adminEmail = "admin@petclinic.com";
        if (await _userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new UserEntity()
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, "Admin_123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Operator");
            }
        }
    }
    
}
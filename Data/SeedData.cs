using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;

namespace StudentApp.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider sp, IConfiguration config)
    {
        var db = sp.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();

        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { "Admin", "Student" })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Admin деректері кодта емес, user-secrets / ортада сақталады
        var email = config["Admin:Email"];
        var password = config["Admin:Password"];
        if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
        {
            var admin = await userManager.FindByEmailAsync(email);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };
                var created = await userManager.CreateAsync(admin, password);
                if (created.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

        if (!await db.Students.AnyAsync())
        {
            db.Students.AddRange(
                new Student { FullName = "Айдос Нұрлан", Group = "ИС-21" },
                new Student { FullName = "Дана Сейітқызы", Group = "ИС-21" },
                new Student { FullName = "Ерлан Бекболат", Group = "ИС-22" });
            await db.SaveChangesAsync();
        }
    }
}

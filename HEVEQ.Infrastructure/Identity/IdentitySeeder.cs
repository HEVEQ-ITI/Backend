using HEVEQ.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HEVEQ.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        string[] roles =
        [
            "Admin",
            "Customer",
            "Provider",
            "Employee"
        ];

        foreach (var role in roles)
        {
            var exists = await roleManager.RoleExistsAsync(role);

            if (!exists)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = role,
                    NormalizedName = role.ToUpper()
                });
            }
        }
    }
}
using Microsoft.AspNetCore.Identity;
namespace Pedidos360.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            //aca se crean los roels
            string[] roles = { "Admin", "Ventas", "Operaciones" };
            foreach (var role in roles)
            {

                if (!await roleManager.RoleExistsAsync(role))  //si el rol no existre
                {
                    await roleManager.CreateAsync(new IdentityRole(role)); //crea uno nuevo
                }
            }

            string emailAdmin = "admin@pedidos360.com";
            string passwordAdmin = "Admin123*";

            var admin = await userManager.FindByEmailAsync(emailAdmin);
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = emailAdmin,
                    Email = emailAdmin,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, passwordAdmin);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            //usuarioventas
            string emailVentas = "ventas@pedidos360.com";
            string passwordVentas = "Ventas123*";

            var ventas = await userManager.FindByEmailAsync(emailVentas);
            if (ventas == null)
            {
                ventas = new IdentityUser
                {
                    UserName = emailVentas,
                    Email = emailVentas,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(ventas, passwordVentas);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(ventas, "Ventas");
                }
            }

            //usuario operaciones
            string emailOperaciones = "operaciones@pedidos360.com";
            string passwordOperaciones = "Opera123*";

            var operaciones = await userManager.FindByEmailAsync(emailOperaciones);
            if (operaciones == null)
            {
                operaciones = new IdentityUser
                {
                    UserName = emailOperaciones,
                    Email = emailOperaciones,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(operaciones, passwordOperaciones);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operaciones, "Operaciones");
                }
            }
        }

    }
}
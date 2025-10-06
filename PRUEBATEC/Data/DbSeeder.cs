using Microsoft.AspNetCore.Identity;
using PRUEBATEC.Models;

namespace PRUEBATEC.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Usuarios.Any())
            {
                var passwordHasher = new PasswordHasher<UsuarioModel>();

                var admin = new UsuarioModel
                {
                    Usuario = "admin",
                    Nombre = "Administrador"
                };

                // Genera hash de la contraseña
                admin.Password = passwordHasher.HashPassword(admin, "1234");

                context.Usuarios.Add(admin);
            }
                        
            context.SaveChanges();
        }
    }
}

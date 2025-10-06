using Microsoft.EntityFrameworkCore;
using PRUEBATEC.Models;

namespace PRUEBATEC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProductoModel> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductoModel>().ToTable("Productos");
        }
        public DbSet<UsuarioModel> Usuarios { get; set; }

    }
}

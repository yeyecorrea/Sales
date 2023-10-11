using Microsoft.EntityFrameworkCore;
using Sales.Shared.Entities;

namespace Sales.API.Data
{
    public class DataContext : DbContext
    {
        //Constructor del DataContext
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }

        // Enlazamos la clase de modelo con el DbSet
        public DbSet<Country> Countries { get; set; }

        // Como no existen paises con el mismo nombre creamos un indixe unico sobreescribiendo el metodo OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Llamamos al modelo y al metodo IsUniqueI() para defifnir el campo Nombre en unico
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
        }

    }
}

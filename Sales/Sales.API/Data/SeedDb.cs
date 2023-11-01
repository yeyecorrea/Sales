namespace Sales.API.Data
{
    /// <summary>
    /// se crea el seeder 
    /// </summary>
    public class SeedDb
    {
        // Inyectamos el data context
        private readonly DataContext _context;
        public SeedDb(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// metodo que crea la base de datos si no existe y aplica las migraciones si hay es como un update database
        /// </summary>
        /// <returns></returns>
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
        }

        /// <summary>
        /// Metodo que valida si existe paises si no los crea
        /// </summary>
        /// <returns></returns>
        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Shared.Entities.Country { Name = "Colombia" });
                _context.Countries.Add(new Shared.Entities.Country { Name = "Argentina" });
                _context.Countries.Add(new Shared.Entities.Country { Name = "Brazil" });
                _context.Countries.Add(new Shared.Entities.Country { Name = "Paraguay" });

                // guardamos los datos en la base de datos
                await _context.SaveChangesAsync();
            }
        }
    }
}

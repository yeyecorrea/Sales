using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    // El Rote por concepcion o buena practica se utiliza siempre el nombre api/y el nombre del controlador
    [ApiController]
    [Route("/api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            // Enlazamos el contexto de la base de datos al constructor por inyeccion de dependencias
            _context = context;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // Obtenemos las lista de de los countries, con el _contex y Countries que es el data set
            return Ok(await _context.Countries.ToListAsync());
        }

        [HttpGet("id:int")]
        public async Task<IActionResult> GetAsync(int id) 
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Country country)
        {
            try
            {
                // Guardamos el objecto del "Modelo Country" por medio de enlace de datos
                _context.Add(country);
                // Guardamos en la base de datos
                await _context.SaveChangesAsync();
                return Ok(country);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un pais con el mismo nombre");
                }
                throw;
            }
            catch (Exception execption) 
            {
                return BadRequest(execption.Message);
            }
            
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Country country)
        {
            try
            {
                // Actualizamos el objecto del "Modelo Country" por medio de enlace de datos
                _context.Update(country);
                // Guardamos en la base de datos
                await _context.SaveChangesAsync();
                return Ok(country);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un pais con el mismo nombre");
                }
                throw;
            }
            catch (Exception execption)
            {
                return BadRequest(execption.Message);
            }
            
        }

        [HttpDelete("id:int")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
            {
                return NotFound();
            }
            
            _context.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}

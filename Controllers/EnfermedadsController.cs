using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Epidemiologia.Data;
using Epidemiologia.Dominio.DTO;

namespace Epidemiologia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnfermedadsController : ControllerBase
    {
        private readonly EpidemiologiaContext _context;

        public EnfermedadsController(EpidemiologiaContext context)
        {
            _context = context;
        }

        // GET: api/Enfermedads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnfermedadDTO>>> GetEnfermedad()
        {
            var enfermedades = await _context.Enfermedad
                .Select(e => new EnfermedadDTO
                {
                    Id_enfermedad = e.Id_enfermedad,
                    Cod_enferemedad = e.Cod_enferemedad,
                    Nombre = e.Nombre,
                    Estado = e.Estado
                })
                .ToListAsync();

            return enfermedades;
        }

        // GET: api/Enfermedads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnfermedadDTO>> GetEnfermedad(int id)
        {
            // Usamos .Select para mapear solo lo que el DTO necesita
            var enfermedadDTO = await _context.Enfermedad
                .Select(e => new EnfermedadDTO
                {
                    Id_enfermedad = e.Id_enfermedad,
                    Cod_enferemedad = e.Cod_enferemedad,
                    Nombre = e.Nombre,
                    Estado = e.Estado
                })
                .FirstOrDefaultAsync(e => e.Id_enfermedad == id);

            if (enfermedadDTO == null)
            {
                return NotFound();
            }

            return enfermedadDTO;
        }

        // PUT: api/Enfermedads/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnfermedad(int id, EnfermedadDTO enfermedadDTO)
        {
            if (id != enfermedadDTO.Id_enfermedad)
            {
                return BadRequest("El ID no coincide");
            }

            // Buscamos el registro real en la base de datos
            var enfermedad = await _context.Enfermedad.FindAsync(id);

            if (enfermedad == null)
            {
                return NotFound();
            }

            // Actualizamos solo los campos necesarios con los datos que vienen del DTO
            enfermedad.Cod_enferemedad = enfermedadDTO.Cod_enferemedad;
            enfermedad.Nombre = enfermedadDTO.Nombre;
            enfermedad.Estado = enfermedadDTO.Estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnfermedadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Indica que la actualización fue exitosa pero no devuelve contenido
        }

        // POST: api/Enfermedads
        [HttpPost]
        public async Task<ActionResult<EnfermedadDTO>> PostEnfermedad(EnfermedadDTO enfermedadDTO)
        {
            // Convertimos el DTO a la Entidad de base de datos
            var enfermedad = new Enfermedad
            {
                Cod_enferemedad = enfermedadDTO.Cod_enferemedad,
                Nombre = enfermedadDTO.Nombre,
                Estado = enfermedadDTO.Estado
            };

            _context.Enfermedad.Add(enfermedad);
            await _context.SaveChangesAsync();

            // Actualizamos el ID del DTO con el que generó la base de datos
            enfermedadDTO.Id_enfermedad = enfermedad.Id_enfermedad;

            return CreatedAtAction("GetEnfermedad", new { id = enfermedad.Id_enfermedad }, enfermedadDTO);
        }

        // DELETE: api/Enfermedads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnfermedad(int id)
        {
            var enfermedad = await _context.Enfermedad.FindAsync(id);
            if (enfermedad == null)
            {
                return NotFound();
            }

            _context.Enfermedad.Remove(enfermedad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnfermedadExists(int id)
        {
            return _context.Enfermedad.Any(e => e.Id_enfermedad == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Epidemiologia.Data;
using Epidemiologia.Dominio;
using Epidemiologia.Dominio.DTO; // Asegúrate que este namespace sea correcto

namespace Epidemiologia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroCasoesController : ControllerBase
    {
        private readonly EpidemiologiaContext _context;

        public RegistroCasoesController(EpidemiologiaContext context)
        {
            _context = context;
        }

        // 1. GET: api/RegistroCasoes (LISTADO GENERAL)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroCasoDTO>>> GetRegistroCaso()
        {
            return await _context.RegistroCaso
                .Include(r => r.Paciente)
                .Include(r => r.Enfermedad)
                .Select(r => new RegistroCasoDTO
                {
                    Id_RegistroCaso = r.Id_RegistroCaso,
                    Cod_Caso = r.Cod_Caso,
                    fecha = r.fecha,
                    Estado = r.Estado,
                    Id_paciente = r.Id_paciente,
                    NombrePaciente = r.Paciente.Nombre,
                    Id_enfermedad = r.Id_enfermedad,
                    NombreEnfermedad = r.Enfermedad.Nombre
                })
                .ToListAsync();
        }

        // 2. GET: api/RegistroCasoes/5 (BUSCAR UNO SOLO)
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroCasoDTO>> GetRegistroCaso(int id)
        {
            var registro = await _context.RegistroCaso
                .Include(r => r.Paciente)
                .Include(r => r.Enfermedad)
                .Where(r => r.Id_RegistroCaso == id)
                .Select(r => new RegistroCasoDTO
                {
                    Id_RegistroCaso = r.Id_RegistroCaso,
                    Cod_Caso = r.Cod_Caso,
                    fecha = r.fecha,
                    Estado = r.Estado,
                    Id_paciente = r.Id_paciente,
                    NombrePaciente = r.Paciente.Nombre,
                    Id_enfermedad = r.Id_enfermedad,
                    NombreEnfermedad = r.Enfermedad.Nombre
                })
                .FirstOrDefaultAsync();

            if (registro == null)
            {
                return NotFound("No se encontró el registro solicitado.");
            }

            return registro;
        }

        // 3. PUT: api/RegistroCasoes/5 (ACTUALIZAR)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroCaso(int id, RegistroCasoDTO dto)
        {
            if (id != dto.Id_RegistroCaso)
            {
                return BadRequest("El ID no coincide con el registro.");
            }

            var registro = await _context.RegistroCaso.FindAsync(id);
            if (registro == null)
            {
                return NotFound();
            }

            // Actualizamos los campos
            registro.Cod_Caso = dto.Cod_Caso;
            registro.fecha = dto.fecha;
            registro.Estado = dto.Estado;
            registro.Id_paciente = dto.Id_paciente;
            registro.Id_enfermedad = dto.Id_enfermedad;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroCasoExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // 4. POST: api/RegistroCasoes (CREAR)
        [HttpPost]
        public async Task<ActionResult<RegistroCasoDTO>> PostRegistroCaso(RegistroCasoDTO dto)
        {
            var registroCaso = new RegistroCaso
            {
                Cod_Caso = dto.Cod_Caso,
                fecha = dto.fecha,
                Estado = dto.Estado,
                Id_paciente = dto.Id_paciente,
                Id_enfermedad = dto.Id_enfermedad
            };

            _context.RegistroCaso.Add(registroCaso);
            await _context.SaveChangesAsync();

            dto.Id_RegistroCaso = registroCaso.Id_RegistroCaso;

            return CreatedAtAction("GetRegistroCaso", new { id = registroCaso.Id_RegistroCaso }, dto);
        }

        // 5. DELETE: api/RegistroCasoes/5 (BORRAR)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistroCaso(int id)
        {
            var registroCaso = await _context.RegistroCaso.FindAsync(id);
            if (registroCaso == null)
            {
                return NotFound("El registro de caso no existe.");
            }

            _context.RegistroCaso.Remove(registroCaso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroCasoExists(int id)
        {
            return _context.RegistroCaso.Any(e => e.Id_RegistroCaso == id);
        }
    }
}
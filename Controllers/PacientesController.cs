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
    public class PacientesController : ControllerBase
    {
        private readonly EpidemiologiaContext _context;

        public PacientesController(EpidemiologiaContext context)
        {
            _context = context;
        }

        // GET: api/Pacientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PacienteDTO>>> GetPaciente()
        {
            var pacientes = await _context.Paciente
                .Select(p => new PacienteDTO
                {
                    Id_paciente = p.Id_paciente,
                    Cod_paciente = p.Cod_paciente,
                    Nombre = p.Nombre,
                    Estado = p.Estado
                })
                .ToListAsync();

            return pacientes;
        }
        // GET: api/Pacientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteDTO>> GetPaciente(int id)
        {
            // Usamos .Select para que solo traiga los datos que definimos en el DTO
            var pacienteDTO = await _context.Paciente
                .Select(p => new PacienteDTO
                {
                    Id_paciente = p.Id_paciente,
                    Cod_paciente = p.Cod_paciente,
                    Nombre = p.Nombre,
                    Estado = p.Estado
                })
                .FirstOrDefaultAsync(p => p.Id_paciente == id);

            if (pacienteDTO == null)
            {
                return NotFound();
            }

            return pacienteDTO;
        }

        // PUT: api/Pacientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaciente(int id, PacienteDTO pacienteDTO)
        {
            if (id != pacienteDTO.Id_paciente)
            {
                return BadRequest("El ID del paciente no coincide.");
            }

            // Buscamos el paciente en la base de datos usando el ID
            var paciente = await _context.Paciente.FindAsync(id);

            if (paciente == null)
            {
                return NotFound("Paciente no encontrado.");
            }

            // Actualizamos los datos del objeto original con los del DTO
            paciente.Cod_paciente = pacienteDTO.Cod_paciente;
            paciente.Nombre = pacienteDTO.Nombre;
            paciente.Estado = pacienteDTO.Estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PacienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // 204 No Content: Éxito sin devolver datos
        }

        // POST: api/Pacientes
        [HttpPost]
        public async Task<ActionResult<PacienteDTO>> PostPaciente(PacienteDTO pacienteDTO)
        {
            var paciente = new Paciente
            {
                Cod_paciente = pacienteDTO.Cod_paciente,
                Nombre = pacienteDTO.Nombre,
                Estado = pacienteDTO.Estado
            };

            _context.Paciente.Add(paciente);
            await _context.SaveChangesAsync();

            pacienteDTO.Id_paciente = paciente.Id_paciente;

            return CreatedAtAction("GetPaciente", new { id = paciente.Id_paciente }, pacienteDTO);
        }

        // DELETE: api/Pacientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Paciente.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }

            _context.Paciente.Remove(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PacienteExists(int id)
        {
            return _context.Paciente.Any(e => e.Id_paciente == id);
        }
    }
}

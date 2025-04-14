using Microsoft.AspNetCore.Mvc;
using Microservicio_ConsultasMedicas.Data;
using Microservicio_ConsultasMedicas.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservicio_ConsultasMedicas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly DataContext _context;

        public PacienteController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
        {
            return await _context.Paciente.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Paciente>> GetPaciente(int id)
        {
            var paciente = await _context.Paciente.FindAsync(id);

            if (paciente == null)
                return NotFound();

            return paciente;
        }

        [HttpPost]
        public async Task<ActionResult<Paciente>> PostPaciente(Paciente paciente)
        {
            _context.Paciente.Add(paciente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaciente), new { id = paciente.id_paciente }, paciente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaciente(int id, Paciente paciente)
        {
            if (id != paciente.id_paciente)
                return BadRequest();

            _context.Entry(paciente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Paciente.FindAsync(id);
            if (paciente == null)
                return NotFound();

            _context.Paciente.Remove(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

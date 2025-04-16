using Microservicio_ConsultasMedicas.Data;
using Microservicio_ConsultasMedicas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microservicio_ConsultasMedicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasMedicasController : ControllerBase
    {
        private readonly DataContext _context;

        public ConsultasMedicasController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ConsultasMedicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultasMedicasEntity>>> GetConsultasMedicas()
        {
            return await _context.ConsultasMedicas.Include(c => c.paciente).ToListAsync();
        }

        // GET: api/ConsultasMedicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConsultasMedicasEntity>> GetConsultaMedica(int id)
        {
            var consulta = await _context.ConsultasMedicas.Include(c => c.paciente)
                                                           .FirstOrDefaultAsync(c => c.id_consulta_medica == id);

            if (consulta == null)
            {
                return NotFound();
            }

            return consulta;
        }

        // POST: api/ConsultasMedicas
        [HttpPost]
        public async Task<ActionResult<ConsultasMedicasEntity>> PostConsultaMedica(ConsultasMedicasEntity consulta)
        {
            _context.ConsultasMedicas.Add(consulta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsultaMedica), new { id = consulta.id_consulta_medica }, consulta);
        }

        // PUT: api/ConsultasMedicas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsultaMedica(int id, ConsultasMedicasEntity consulta)
        {
            if (id != consulta.id_consulta_medica)
            {
                return BadRequest();
            }

            _context.Entry(consulta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultaMedicaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ConsultasMedicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsultaMedica(int id)
        {
            var consulta = await _context.ConsultasMedicas.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            _context.ConsultasMedicas.Remove(consulta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConsultaMedicaExists(int id)
        {
            return _context.ConsultasMedicas.Any(e => e.id_consulta_medica == id);
        }
    }
}

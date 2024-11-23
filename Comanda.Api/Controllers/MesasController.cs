using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeComandas.BancoDeDados;
using SistemaDeComandas.Modelos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Comanda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesasController : ControllerBase
    {
        private readonly ComandaContexto _context;

        public MesasController(ComandaContexto context)
        {
            _context = context;
        }

        // GET: api/Mesas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mesa>>> GetMesas()
        {
            return await _context.Mesas.ToListAsync();
        }
        // GET: api/Usuarios/5
        [HttpGet("{numeroMesa}")]
        public async Task<ActionResult<Mesa>> GetMesa(int numeroMesa)
        {
            var mesa = _context.Mesas.FirstOrDefault(m => m.NumeroMesa == numeroMesa);

            if (mesa == null)
            {
                return BadRequest("Mesa não encontrada");

            }

            //if (mesa == null)
            //{
            //    return NotFound();
            //}

            return mesa;
        }
        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mesa>> PostMesa(Mesa mesa)
        {
            //verificar se uma mesa com esse numeroMesa ja existe
            var mesaNoBanco = _context.Mesas.FirstOrDefault(m => m.NumeroMesa == mesa.NumeroMesa);
            if(mesaNoBanco == null)
            {
                _context.Mesas.Add(mesa);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMesa", new { id = mesa.IdMesa }, mesa);
            }
            return BadRequest("Uma mesa com esse numero ja existe xdd.");
            
        }
        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMesa(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }

            _context.Mesas.Remove(mesa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMesa(int id, Mesa mesa)
        {
            if (id != mesa.IdMesa)
            {
                return BadRequest();
            }

            _context.Entry(mesa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MesaExists(id))
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
        private bool MesaExists(int id)
        {
            return _context.Mesas.Any(e => e.IdMesa == id);
        }
    }
}

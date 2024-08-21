using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comanda.Api.Dtos_data_transfer_object_;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeComandas.BancoDeDados;

namespace Comanda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //ComandasController é a Rota(ali encima)
    public class ComandasController : ControllerBase
    {
        private readonly ComandaContexto _context;

        public ComandasController(ComandaContexto context)
        {
            _context = context;
        }

        // GET: api/Comandas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SistemaDeComandas.Modelos.Comanda>>> GetComandas()
        {
            return await _context.Comandas.ToListAsync();
        }

        // GET: api/Comandas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SistemaDeComandas.Modelos.Comanda>> GetComanda(int id)
        {
            var comanda = await _context.Comandas.FindAsync(id);

            if (comanda == null)
            {
                return NotFound();
            }

            return comanda;
        }

        // PUT: api/Comandas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComanda(int id, SistemaDeComandas.Modelos.Comanda comanda)
        {
            if (id != comanda.Id)
            {
                return BadRequest();
            }

            _context.Entry(comanda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComandaExists(id))
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

        // POST: api/Comandas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //Dto significado Data Tranfer Object
        public async Task<ActionResult<SistemaDeComandas.Modelos.Comanda>> PostComanda(ComandaDto comanda)
            //comanda aqui eh o json
        {
            // criando uma nova comanda

            var novaComanda = new SistemaDeComandas.Modelos.Comanda()
            {
                NomeCliente = comanda.NomeCliente,
                NumeroMesa = comanda.NumeroMesa
            };

            //adicionanado a comanda no banco de maneira asincrona
            await _context.Comandas.AddAsync(novaComanda);

            //salvando a comanda de maneira asincrona
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComanda", new { id = novaComanda.Id }, comanda);
        }

        // DELETE: api/Comandas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComanda(int id)
        {
            var comanda = await _context.Comandas.FindAsync(id);
            if (comanda == null)
            {
                return NotFound();
            }

            _context.Comandas.Remove(comanda);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComandaExists(int id)
        {
            return _context.Comandas.Any(e => e.Id == id);
        }
    }
}

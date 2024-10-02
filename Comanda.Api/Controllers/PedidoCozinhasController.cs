using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comanda.Api.Dtos_data_transfer_object_;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeComandas.BancoDeDados;
using SistemaDeComandas.Modelos;

namespace Comanda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoCozinhasController : ControllerBase
    {
        private readonly ComandaContexto _context;

        public PedidoCozinhasController(ComandaContexto context)
        {
            _context = context;
        }

        // GET: api/PedidoCozinhas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoCozinhaGetDto>>> GetPedidoCozinhas([FromQuery] int? situacaoId)
        {
            var query = _context.PedidoCozinhas
                .Include(p=>p.Comanda)
                .Include(p => p.PedidoCozinhaItems)
                    .ThenInclude(p => p.ComandaItem)
                        .ThenInclude(p => p.CardapioItem)
                .AsQueryable();

            if(situacaoId > 0)
            {
                query = query.Where(w => w.SituacaoId == situacaoId);
            }

            return await query.Select(s => new PedidoCozinhaGetDto()
            {
                Id = s.Id,
                NumeroMesa = s.Comanda.NumeroMesa,
                NomeCliente = s.Comanda.NomeCliente,
                Titulo = s.PedidoCozinhaItems.First().ComandaItem.CardapioItem.Titulo,
            }).ToListAsync();
        }

        // GET: api/PedidoCozinhas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoCozinha>> GetPedidoCozinha(int id)
        {
            var pedidoCozinha = await _context.PedidoCozinhas.FindAsync(id);

            if (pedidoCozinha == null)
            {
                return NotFound();
            }

            return pedidoCozinha;
        }

        // PUT: api/PedidoCozinhas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        //atualizar um pedido da cozinha para um novo status
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedidoCozinha(int id, PedidoCozinhaUpdateDto pedido)
        {
            // Consulta o pedido pelo Id informado
            // SELECT * FROM PedidoCozinha WHERE id = {id}
            var pedidoCozinha = await _context.
                                        PedidoCozinhas.
                                        FirstOrDefaultAsync(p => p.Id == id);
            //Alteração do Status
            pedidoCozinha.SituacaoId = pedido.NovoStatusId;
            //Gravação no Banco de Dados
            //UPDATE PedidoCozinha SET SituaçãoId = 3 WHERE Id = {id}
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/PedidoCozinhas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PedidoCozinha>> PostPedidoCozinha(PedidoCozinha pedidoCozinha)
        {
            _context.PedidoCozinhas.Add(pedidoCozinha);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedidoCozinha", new { id = pedidoCozinha.Id }, pedidoCozinha);
        }

        // DELETE: api/PedidoCozinhas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedidoCozinha(int id)
        {
            var pedidoCozinha = await _context.PedidoCozinhas.FindAsync(id);
            if (pedidoCozinha == null)
            {
                return NotFound();
            }

            _context.PedidoCozinhas.Remove(pedidoCozinha);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PedidoCozinhaExists(int id)
        {
            return _context.PedidoCozinhas.Any(e => e.Id == id);
        }
    }
}

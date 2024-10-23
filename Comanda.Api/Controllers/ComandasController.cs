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
        public async Task<ActionResult<IEnumerable<ComandaGetDto>>> GetComandas()
        {
            //SELECT c.NumeroMesa, c.NomeCliente FROM Comandas WHERE SituacaoComanda = 1
            var resultComandas = await _context.Comandas
                .Where(c => c.SituacaoComanda == 1)
                .Select(c => new ComandaGetDto
                {
                    Id = c.Id,
                    NumeroMesa = c.NumeroMesa,
                    NomeCliente = c.NomeCliente,
                    ComandaItens = c.ComandaItems.Select(ci => new ComandaItensGetDto
                                    {
                                        Id = ci.Id,
                                        Titulo = ci.CardapioItem.Titulo,
                                    }
                                    ).ToList()
                }) 
                .ToListAsync();
            //retorna o conteudo com a lista de comandas
            return Ok(resultComandas);
        }

        // GET: api/Comandas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComandaGetDto>> GetComanda(int id)
        {
            // SELECT * FROM Comandas WHERE Id = {id que eu passar la}
            // busca a comanda por id
            var comanda = await _context.Comandas
                                        .FirstOrDefaultAsync(c => c.Id == id);

            if (comanda == null)
            {
                return NotFound();
            }

            var comandaDto = new ComandaGetDto
            {
                NumeroMesa = comanda.NumeroMesa,
                NomeCliente = comanda.NomeCliente
            };
            // SELECT Id, Titulo FROM ComandaItem ci WHERE ci.ComandaId = 1
            // INNER JOIN CardapioItem cii on cii.Id = ci.CardapioItemId
            // busca os itens da comanda
            var comandaItensDto = await _context.ComandaItems
                                .Include(ci => ci.CardapioItem)
                                 .Where(ci => ci.ComandaId == id)
                                    .Select(cii => new ComandaItensGetDto
                                    {
                                        Id = cii.Id,
                                        Titulo = cii.CardapioItem.Titulo
                                    })
                                           .ToListAsync();
            comandaDto.ComandaItens = comandaItensDto;

            return comandaDto;
        }

        // PUT: api/Comandas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComanda(int id, ComandaUpdateDto comanda)
        {
            if (id != comanda.Id)
            {
                return BadRequest();
            }
            // SELECT * FROM Comandas WHERE id = **idInformado**
            var comandaUpdate = await _context.Comandas
                .FirstAsync(c => c.Id == id);

            if (comanda.NumeroMesa > 0)
            {
                comandaUpdate.NumeroMesa = comanda.NumeroMesa;
            }
            if (!string.IsNullOrEmpty(comanda.NomeCliente))
            {
                comandaUpdate.NomeCliente = comanda.NomeCliente;
            }

            foreach (var item in comanda.CardapioItems)
            {
                var novoComandaItem = new ComandaItem()
                {
                    Comanda = comandaUpdate,
                    CardapioItemId = item
                };

                await _context.ComandaItems.AddAsync(novoComandaItem);

                var CardapioItem = await _context.CardapioItems.FindAsync(item);
                var PossuiPreparo = CardapioItem.PossuiPreparo;
                if (PossuiPreparo)
                {
                    var novoPedidoCozinha = new PedidoCozinha()
                    {
                        Comanda = comandaUpdate,
                        SituacaoId = 1
                    };
                    await _context.PedidoCozinhas.AddAsync(novoPedidoCozinha);

                    var novoPedidoCozinhaItem = new PedidoCozinhaItem()
                    {
                        PedidoCozinha = novoPedidoCozinha,
                        ComandaItem = novoComandaItem,

                    };
                    await _context.PedidoCozinhaItems.AddAsync(novoPedidoCozinhaItem);
                }
            }

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

            foreach (var item in comanda.CardapioItems)
            {
                //var cardapioItem = await _context.CardapioItems.FindAsync(comanda.CardapioItems[0]);
                var novoItemComanda = new SistemaDeComandas.Modelos.ComandaItem()
                {
                    Comanda = novaComanda,
                    CardapioItemId = item
                };

                // adicionando o novo item na comanda
                await _context.ComandaItems.AddAsync(novoItemComanda);

                // verificar se o cardapio possui preparo
                // SELECT PossuiPreparo From Cardapioitem Where id = <item>
                // Find pode retornar nulo
                // First nao retorna nulo, pega sempre o primeiro
                var Cardapioitem = await _context.CardapioItems.FindAsync(item);
                var PossuiPreparo = Cardapioitem.PossuiPreparo;

                if (PossuiPreparo)
                {
                    var novoPedidoCozinha = new PedidoCozinha()
                    {
                        Comanda = novaComanda,
                        SituacaoId = 1 //1 === Pendente
                    };

                    // INSERT INTO PedidoCozinha (Id, ComandaId, Situacaoid) VALUES (1,6,1)
                    await _context.PedidoCozinhas.AddAsync(novoPedidoCozinha);
                    var novoPedidoCozinhaIten = new PedidoCozinhaItem()
                    {
                        PedidoCozinha = novoPedidoCozinha,
                        ComandaItem = novoItemComanda
                    };
                    await _context.PedidoCozinhaItems.AddAsync(novoPedidoCozinhaIten);
                }
            }







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
        //api/comanda/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchComanda(int id)
        {
            //consulta a comanda SELECT * from Comandas WHERE id = {id}
            var comanda = await _context.Comandas.FindAsync(id);
            if (comanda == null)
                //retorna um 404
                return NotFound();
            //alteracao comanda

            comanda.SituacaoComanda = 2;
            //UPDATE Comandas SET SituacaoComanda = 2 WHERE id = {id}
            await _context.SaveChangesAsync();
            //retorna um 204
            return NoContent();
        }

        private bool ComandaExists(int id)
        {
            return _context.Comandas.Any(e => e.Id == id);
        }
    }
}

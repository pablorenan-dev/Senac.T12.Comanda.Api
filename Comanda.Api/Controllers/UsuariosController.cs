using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Comanda.Api.Dtos_data_transfer_object_;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaDeComandas.BancoDeDados;
using SistemaDeComandas.Modelos;

namespace Comanda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ComandaContexto _context;

        public UsuariosController(ComandaContexto context)
        {
            _context = context;
        }
        //CRIAR Um ENDPOINT DE LOGIN
        //fROM BODY EH PQ OS DADOS VAO VIR DO CORPO, E NAO PASSAR OS DADOS PELO URL
        //especificar caminho do post, para nao ter nenhum igual e nao dar erro
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioResponse>> Login([FromBody] UsuarioRequest usuarioRequest)
        {
            // Crie um token JWT
            //Cria uma fabrica de token
            var tokenHandler = new JwtSecurityTokenHandler();
            //chave secreta
            var key = Encoding.UTF8.GetBytes("3e8acfc238f45a314fd4b2bde272678ad30bd1774743a11dbc5c53ac71ca494b");

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.emailUsuario.Equals(usuarioRequest.emailUsuario));

            if(usuario == null)
            {
                return NotFound("Usuario Invalido.");
            }

            if(!usuario.senhaUsuario.Equals(usuarioRequest.senhaUsuario))
            {
                return BadRequest("Usuário/Senha invalido.");
            }

            //descreve as informacoes que o token possuira
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, usuario.nomeUsuario),
                new Claim(ClaimTypes.NameIdentifier, usuario.idUsuario.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Tempo de expiração do token

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new UsuarioResponse() { idUsuario = 1, emailUsuario = "teste@teste.com", token = tokenString });
        }
        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.idUsuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.idUsuario }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.idUsuario == id);
        }
    }
}

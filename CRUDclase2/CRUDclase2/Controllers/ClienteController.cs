using CRUDclase2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDclase2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly BitwisePrototipoContext _context;

        public ClienteController(BitwisePrototipoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
        {
            try
            {
                var lista = await _context.Clientes.ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("{id}", Name = "GetCliente")]
        public async Task<ActionResult<Cliente>> GetById(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

                if (cliente == null)
                    return NotFound("No se ha encontrado el cliente solicitado");

                return Ok(cliente);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("filtro")]
        public async Task<ActionResult<IEnumerable<Cliente>>> Filtro(string nombre = null)
        {
            var lista = new List<Cliente>();
            lista = await _context.Clientes.ToListAsync();
            if (string.IsNullOrEmpty(nombre))
            {
                // Si no se proporciona un nombre, devuelve la lista completa de clientes
                return Ok(lista);
            }
            else
            {
                // Filtra los clientes cuyo nombre coincida (ignorando mayúsculas/minúsculas)
                return Ok(lista.Where(c => c.Nombre.ToLower().Contains(nombre.ToLower())));
            }
        }



        [HttpPost]
        public async Task<ActionResult> Post(Cliente cliente)
        {
            try
            {
                if (cliente.MontoDeuda == null)
                    cliente.MontoDeuda = 0;
                _context.Clientes.Add(cliente);

                await _context.SaveChangesAsync();
                
                return new CreatedAtRouteResult("GetCliente", cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Cliente cliente)
        {
            var clienteExistente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
            if (clienteExistente == null)
                return NotFound("No existe el cliente que se quiere actualizar");

            try
            {
                clienteExistente.Nombre = cliente.Nombre is null ? clienteExistente.Nombre : cliente.Nombre;
                clienteExistente.Direccion = cliente.Direccion is null ? clienteExistente.Direccion : cliente.Direccion;
                clienteExistente.Ciudad = cliente.Ciudad is null ? clienteExistente.Ciudad : cliente.Ciudad;
                clienteExistente.Telefono = cliente.Telefono is null ? clienteExistente.Telefono : cliente.Telefono;
                clienteExistente.Duedor = cliente.Duedor is null ? clienteExistente.Duedor : cliente.Duedor;
                clienteExistente.MontoDeuda = cliente.MontoDeuda is null ? clienteExistente.MontoDeuda : cliente.MontoDeuda;

                _context.Clientes.Update(clienteExistente);
                await _context.SaveChangesAsync();
                return Ok(clienteExistente);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
            if (cliente == null)
            {
                return NotFound("No se ha encontrado el cliente solicitado");
            }

            try
            {
                _context.Clientes.Remove(cliente);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

    }
}

using AppLogin.Models;
using AppLogin.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLogin.Areas.Colaborador.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteApiController : Controller
    {
        private IClienteRepository _clienteRepository;
        public ClienteApiController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        public IActionResult ListarClientes()
        {
            var clientes = _clienteRepository.ObterTodosClientes();
            return Ok(clientes);
        }

        [HttpPost]
        public IActionResult Post(Cliente cliente) //cadastrar
        {
            if (cliente is null)
            {
                return BadRequest(new
                {
                    mensagem = "Os dados do cliente são obrigatórios."
                });
            }
            _clienteRepository.Cadastrar(cliente);

            return Ok(new
            {
                mensagem = "Cadastro realizado com sucesso.",
                cliente = cliente
            });
        }

        [HttpGet("{id}")]
        public IActionResult BuscaPorId(int id)
        {
            var cliente = _clienteRepository.ObterCliente(id);

            if (cliente == null || cliente.Id == 0)
            {
                return NotFound(new
                {
                    mensagem = "Cliente não encontrado."
                });
            }

            return Ok(cliente);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clienteExistente = _clienteRepository.ObterCliente(id);

            if (clienteExistente == null || clienteExistente.Id == 0)
            {
                return NotFound(new
                {
                    mensagem = "Cliente não encontrado."
                });
            }

            cliente.Id = id;

            _clienteRepository.Atualizar(cliente);

            return Ok(new
            {
                mensagem = "Cliente atualizado com sucesso."
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var cliente = _clienteRepository.ObterCliente(id);

            if (cliente == null || cliente.Id == 0)
            {
                return NotFound(new
                {
                    mensagem = "Cliente não encontrado."
                });
            }

            _clienteRepository.Excluir(id);

            return Ok(new
            {
                mensagem = "Cliente excluído com sucesso."
            });
        }
    }
}

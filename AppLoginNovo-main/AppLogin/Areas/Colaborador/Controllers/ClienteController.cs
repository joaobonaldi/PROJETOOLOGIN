using AppLogin.Libraries.Filtro;
using AppLogin.Models;
using AppLogin.Models.Constant;
using AppLogin.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLogin.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public IActionResult Index()
        {
            return View(_clienteRepository.ObterTodosClientes());
        }

        public IActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar([FromForm] Cliente cliente)
        {
            cliente.Situacao = SituacaoConstant.Ativo;

            _clienteRepository.Cadastrar(cliente);
            return RedirectToAction(nameof(Cadastrar));
        }

        [ValidateHttpReferer]
        public IActionResult Ativar(int id)
        {
            _clienteRepository.Ativar(id);
            return RedirectToAction(nameof(Index));
        }
        [ValidateHttpReferer]
        public IActionResult Desativar(int id)
        {
            _clienteRepository.Desativar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

using System.Diagnostics;
using AppLogin.Libraries.Filtro;
using AppLogin.Libraries.Login;
using AppLogin.Models;
using AppLogin.Models.Constant;
using AppLogin.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLogin.Controllers
{
    public class HomeController : Controller
    {
        // injeção de dependencia
        private IClienteRepository _clienteRepository;
        private LoginCliente _loginCliente;

        public HomeController(IClienteRepository clienteRepository, LoginCliente loginCliente)
        {
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromForm] Cliente cliente)
        {
            Cliente clienteDB = _clienteRepository.Login(cliente.Email, cliente.Senha);

            if (clienteDB.Email != null && clienteDB.Senha != null)
            {
                _loginCliente.Login(clienteDB);
                return new RedirectResult(Url.Action(nameof(PainelCliente)));
            }
            else
            {
                //erro na sessão
                ViewData["MSG_E"] = "Usuário não localizado, por favor verifique e-mail e senha digitado";
                return View();
            }
        }

        [ClienteAutorizacao]
        public IActionResult PainelCliente()
        {
            ViewBag.Nome = _loginCliente.GetCliente().Nome;
            ViewBag.CPF = _loginCliente.GetCliente().CPF;
            ViewBag.Email = _loginCliente.GetCliente().Email;
            //return new ContentResult() { Content = "Este é o Painel do Cliente!" };
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ClienteAutorizacao]
        public IActionResult LogoutCliente()
        {
            _loginCliente.Logout();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Cliente cliente)
        {
            var CPFExist = _clienteRepository.BuscaCpfCliente(cliente.CPF).CPF;
            var EmailExist = _clienteRepository.BuscaEmailCliente(cliente.Email).Email;

            if (!string.IsNullOrWhiteSpace(CPFExist))
            {
                //CPF cadastrado
                ViewData["MSG_CPF"] = "CPF já cadastrado, por favor verifique os dados digitados";
                return View();
            }
            else if (!string.IsNullOrWhiteSpace(EmailExist))
            {
                //Email cadastrado
                ViewData["MSG_Email"] = "E-mail já cadastrado, por favor verifique os dados digitados";
                return View();
            }
            else if (ModelState.IsValid)
            {
                _clienteRepository.Cadastrar(cliente);
                return RedirectToAction(nameof(Login));
            }
            return View();
        }
    }
}

using AppLogin.Models;
using AppLogin.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLogin.Areas.Colaborador.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColaboradorApiController : Controller
    {
        private IColaboradorRepository _colaboradorRepository;
        public ColaboradorApiController(IColaboradorRepository colaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
        }

        // GET: api/ColaboradorApi
        [HttpGet]
        public IActionResult ListarColaboradores()
        {
            var colaboradores = _colaboradorRepository.ObterTodosColaboradores();
            return Ok(colaboradores);
        }

        // GET: api/ColaboradorApi/5
        [HttpGet("{id}")]
        public IActionResult BuscaPorId(int id)
        {
            var colaborador = _colaboradorRepository.ObterColaborador(id);

            // o repositório devolve um objeto vazio (Id = 0) quando não encontra
            if (colaborador == null || colaborador.Id == 0)
            {
                return NotFound(new
                {
                    mensagem = "Colaborador não encontrado."
                });
            }

            return Ok(colaborador);
        }

        // POST: api/ColaboradorApi
        [HttpPost]
        public IActionResult Post(Colaborador colaborador) //cadastrar
        {
            if (colaborador is null)
            {
                return BadRequest(new
                {
                    mensagem = "Os dados do colaborador são obrigatórios."
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _colaboradorRepository.Cadastrar(colaborador);

            return Ok(new
            {
                mensagem = "Cadastro realizado com sucesso.",
                colaborador = colaborador
            });
        }

        // PUT: api/ColaboradorApi/5
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Colaborador colaborador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var colaboradorExistente = _colaboradorRepository.ObterColaborador(id);

            if (colaboradorExistente == null || colaboradorExistente.Id == 0)
            {
                return NotFound(new
                {
                    mensagem = "Colaborador não encontrado."
                });
            }

            colaborador.Id = id;

            _colaboradorRepository.Atualizar(colaborador);

            return Ok(new
            {
                mensagem = "Colaborador atualizado com sucesso."
            });
        }

        // DELETE: api/ColaboradorApi/5
        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var colaborador = _colaboradorRepository.ObterColaborador(id);

            if (colaborador == null || colaborador.Id == 0)
            {
                return NotFound(new
                {
                    mensagem = "Colaborador não encontrado."
                });
            }

            _colaboradorRepository.Excluir(id);

            return Ok(new
            {
                mensagem = "Colaborador excluído com sucesso."
            });
        }
    }
}

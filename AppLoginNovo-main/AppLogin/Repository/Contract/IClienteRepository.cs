using AppLogin.Models;
using X.PagedList;

namespace AppLogin.Repository.Contract
{
    public interface IClienteRepository
    {
        // Login Cliente
        Cliente Login(string Email, string Senha);

        //CRUD
        void Cadastrar(Cliente cliente);
        void Atualizar(Cliente cliente);
        void Excluir(int Id);
        void Ativar(int Id);
        void Desativar(int Id);
        Cliente ObterCliente(int Id);
        Cliente BuscaCpfCliente(string CPF);
        Cliente BuscaEmailCliente(string email);
        IEnumerable<Cliente> ObterTodosClientes();

        IPagedList<Cliente> ObterTodosClientes(int? pagina, string pesquisa);
    }
}
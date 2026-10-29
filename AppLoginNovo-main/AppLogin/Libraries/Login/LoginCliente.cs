using AppLogin.Models;
using Newtonsoft.Json;

namespace AppLogin.Libraries.Login
{
    public class LoginCliente
    {
        private string Key = "Login.Cliente";
        private Sessao.Sessao _sessao;
        public LoginCliente(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }

        public void Login(Cliente cliente)
        {
            //serializar
            string clienteJsonString = JsonConvert.SerializeObject(cliente);
            _sessao.Cadastrar(Key, clienteJsonString);
        }
        public Cliente GetCliente()
        {
            //deserializar
            if (_sessao.Existe(Key))
            {
                string clienteJsonString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<Cliente>(clienteJsonString);
            }
            else
            {
                return null;
            }
        }
        //Remove a sessão e desloga o cliente
        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}

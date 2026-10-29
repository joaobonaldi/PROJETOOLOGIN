using AppLogin.Models;
using AppLogin.Models.Constant;
using AppLogin.Repository.Contract;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data;
using X.PagedList;
using X.PagedList.Extensions;

namespace AppLogin.Repository
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private readonly string _conexaoMySQL;
        private IConfiguration _conf;

        public ColaboradorRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _conf = conf;
        }

        public void Atualizar(Colaborador colaborador)
        {
            string Tipo = ColaboradorTipoConstant.Comum;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Colaborador set Nome=@Nome, " +
                    " Email=@Email, Senha=@Senha, Telefone=@Telefone, CPF=@CPF, Tipo=@Tipo Where Id=@Id ", conexao);

                cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = colaborador.Id;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = colaborador.Nome;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = colaborador.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = colaborador.Senha;
                cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = colaborador.Telefone;
                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = colaborador.CPF;
                cmd.Parameters.Add("@Tipo", MySqlDbType.VarChar).Value = Tipo;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void AtualizarSenha(Colaborador colaborador)
        {
            throw new NotImplementedException();
        }

        public void Cadastrar(Colaborador colaborador)
        {
            string comum = ColaboradorTipoConstant.Comum;

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("insert into Colaborador(Nome, CPF, Telefone, Email, Senha, Tipo)" + 
                                                    " values (@Nome, @CPF, @Telefone, @Email, @Senha, @Tipo)", conexao);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = colaborador.Nome;
                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = colaborador.CPF;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = colaborador.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = colaborador.Senha;
                cmd.Parameters.Add("@Tipo", MySqlDbType.VarChar).Value = colaborador.Tipo;
                cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = colaborador.Telefone;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from Colaborador WHERE Id=@Id", conexao);
                cmd.Parameters.AddWithValue("@Id", Id);
                int i = cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public Colaborador Login(string Email, string Senha)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from Colaborador where Email = @Email and Senha = @Senha", conexao);

                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = Senha;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Colaborador colaborador = new Colaborador();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    colaborador.Id = (Int32)(dr["Id"]);
                    colaborador.Nome = (string)(dr["Nome"]);
                    colaborador.Tipo = (string)(dr["Tipo"]);
                    colaborador.Email = (string)(dr["Email"]);
                    colaborador.Senha = (string)(dr["Senha"]);
                }
                return colaborador;
            }
        }

        public Colaborador ObterColaborador(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Colaborador WHERE Id=@Id ", conexao);
                cmd.Parameters.AddWithValue("@Id", Id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Colaborador colaborador = new Colaborador();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    colaborador.Id = (Int32)(dr["Id"]);
                    colaborador.Nome = (string)(dr["Nome"]);
                    colaborador.Tipo = (string)(dr["Tipo"]);
                    colaborador.Email = (string)(dr["Email"]);
                    colaborador.CPF = (string)(dr["CPF"]);
                    colaborador.Telefone = (string)(dr["Telefone"]);
                    colaborador.Senha = (string)(dr["Senha"]);
                }
                return colaborador;
            }
        }

        public List<Colaborador> ObterColaboradorPorEmail(string email)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Colaborador> ObterTodosColaboradores()
        {
            List<Colaborador> colabList = new List<Colaborador>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM COLABORADOR", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    colabList.Add(
                        new Colaborador
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = (string)(dr["Nome"]),
                            Email = (string)(dr["Email"]),
                            Senha = (string)(dr["Senha"]),
                            Tipo = (string)(dr["Tipo"])
                        });
                }
                return colabList;
            }
        }

        public IPagedList<Colaborador> ObterTodosColaboradores(int? pagina)
        {
            int RegistroPorPagina = _conf.GetValue<int>("RegistroPorPagina");

            int NumeroPagina = pagina ?? 1;
            List<Colaborador> ListCat = new List<Colaborador>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from colaborador;", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    ListCat.Add(
                        new Colaborador
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = (string)(dr["Nome"]),
                            Senha = (string)(dr["Senha"]),
                            Email = (string)(dr["Email"]),
                            Tipo = (string)(dr["Senha"])

                        });
                }
                return ListCat.ToPagedList<Colaborador>(NumeroPagina, RegistroPorPagina);
            }
        }
    }
}

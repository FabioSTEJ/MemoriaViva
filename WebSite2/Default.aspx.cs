using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page 
{
    

    protected void btnOrdemAleatoria_Click(object sender, EventArgs e)
    {

    }
    protected void btnOrdemAlfabetica_Click(object sender, EventArgs e)
    {

    }
    protected void btnSalvarPrece_Click(object sender, EventArgs e) 
    { 

    }
    protected void btnAdicionarVitimaModal_Click(object sender, EventArgs e)
    {
        // Lógica para adicionar vítima
        // Capturar os valores dos campos do modal
        string nomeFalecido = nomeFalecidotxt.Value.ToUpper();
        string dataNascimento = dataNascimentotxt.Value;
        string dataFalecimento = dataObitotxt.Value;
        string localObito = localObitotxt.Value.ToString();

        // Criar o objeto de Vítima
        //pegando os valores adicionados as vitimas por meio do text box e salvar em nosso objeto
        Falecido falecido = new Falecido()
        {

            nome_vitima = nomeFalecido,
            data_nasc = Convert.ToDateTime( dataNascimento),
            data_obito = Convert.ToDateTime(dataFalecimento),
            local_obito = localObito
        };
        //adicionando o objeto ao banco de dados
        AdicionarFalecido(falecido);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "fecharModal", "$('#adicionarVitima').modal('hide');", true);
        Response.Write("<script>alert('Vítima adicionada com sucesso!');</script>");

        //Limpar os campos depois que a vitima for adicionada
        nomeFalecidotxt.Value = "";
        dataNascimentotxt.Value = "";
        dataObitotxt.Value = "";
        localObitotxt.Value = "";
    }

    protected void btnAcenderVela_Click(object sender, EventArgs e)
    {

    }

// classe para os metodos do banco
    
    public class Falecido
    {
        public int id_vitima { get; set; }
        public string nome_vitima { get; set; }
        public DateTime data_nasc { get; set; }
        public DateTime data_obito { get; set; }
        public string local_obito { get; set; }
    }

// metodo para adicionar ao banco

    private void AdicionarFalecido(Falecido falecido)
    {

        string conexaoString = System.Configuration.ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

        string inserir = "INSERT INTO Vitima (nome_vitima, data_nasc, data_obito, local_obito) VALUES (@Nome, @DataNascimento, @DataFalecimento, @LocalObito)";

        using (SqlConnection conexao = new SqlConnection(conexaoString))
        {
            using (SqlCommand comando = new SqlCommand(inserir, conexao))
            {

                comando.Parameters.AddWithValue("@Nome", falecido.nome_vitima);
                comando.Parameters.AddWithValue("@DataNascimento", falecido.data_nasc.ToString("yyyy/MM/dd"));
                comando.Parameters.AddWithValue("@DataFalecimento", falecido.data_obito.ToString("yyyy/MM/dd"));
                comando.Parameters.AddWithValue("LocalObito", falecido.local_obito);

                conexao.Open();
                comando.ExecuteNonQuery();
            }
        }
    }

}

// metodo para exibir o banco

    private List<Falecido> GetFalecidos()
    {
        string conexaoString = System.Configuration.ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

        var falecidos = new List<Falecido>();

        string consulta = "SELECT IdVitima, Nome, DataNascimento, DataFalecimento, LocalObito FROM Vitima";

        using (SqlConnection conexao = new SqlConnection(conexaoString))
        {
            using (SqlCommand comando = new SqlCommand(consulta, conexao))
            {
                conexao.Open();
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        falecidos.Add(new Falecido
                        {
                            id_vitima = Convert.ToInt32(leitor["id_vitima"]),
                            nome_vitima = leitor["nome_vitima"].ToString(),
                            data_nasc = Convert.ToDateTime(leitor["data_nasc"]),
                            data_obito = Convert.ToDateTime (leitor["data_obito"]),
                            local_obito = leitor["local_obito"].ToString()
                        });
                    }
                }
            }
        }
        return falecidos;
    }

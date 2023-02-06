using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcacaoDePonto.Repositorio
{
    public class Contexto
    {
        internal readonly SqlConnection _connection;

        public Contexto()
        {
            _connection = new SqlConnection("Server=DESKTOP-LTSA6LQ;Integrated Security=true;");

        }
        public void AbrirConexao()
        {
            _connection.Open();
        }
        public void FecharConexao()
        {
            _connection.Close();
        }
    }
}

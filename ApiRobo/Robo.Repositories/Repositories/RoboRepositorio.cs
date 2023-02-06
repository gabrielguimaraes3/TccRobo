using MarcacaoDePonto.Repositorio;
using Robo.Buscar;
using Robo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Robo.Repositories.Repositories
{
    public class RoboRepositorio : Contexto
    {

        private readonly RastrearProdutosService _roboRastrear;
        public RoboRepositorio()
        {

            _roboRastrear = new RastrearProdutosService();
        }
        public List<Produto> ListarProdutos(string? nome)
        {
            string comandoSql = @"SELECT 
                                   NomeProduto,  Preco, Link, DataBusca, PrecoAntigo
                                  FROM 
                                   Produto ";
            if (!string.IsNullOrWhiteSpace(nome))
                comandoSql += " WHERE NomeProduto LIKE @NomeProduto";

            using (var cmd = new SqlCommand(comandoSql, _connection))
            {
                if (!string.IsNullOrWhiteSpace(nome))
                    cmd.Parameters.AddWithValue("@NomeProduto", "%" + nome + "%");

                using (var rdr = cmd.ExecuteReader())
                {
                    var produtos = new List<Produto>();
                    while (rdr.Read())
                    {
                        var produto = new Produto();
                        produto.NomeProduto = Convert.ToString(rdr["NomeProduto"]);
                        produto.Preco = Convert.ToDouble(rdr["Preco"]);
                        produto.Link = Convert.ToString(rdr["Link"]);
                        produto.DataBusca = Convert.ToDateTime(rdr["DataBusca"]);
                        produto.PrecoAntigo = rdr["PrecoAntigo"] == DBNull.Value ? null : Convert.ToDouble(rdr["PrecoAntigo"]);

                        produtos.Add(produto);
                    }
                    return produtos;
                }
            }
        }

        public void RoboInserir()
        {
            var lista = _roboRastrear.Rastreio().ToList();

            foreach (var item in lista)
            {
                string comandoSql = "";
                var precoAntigo = BuscarPrecoAntigo(item.NomeProduto);

                if (precoAntigo == item.Preco)
                    continue;

                
                if(precoAntigo != item.Preco)
                {
                    comandoSql = @"INSERT INTO Produto
                           (NomeProduto,  Preco, Link, DataBusca)
                         VALUES
                           (@NomeProduto, @Preco, @Link, @DataBusca);";
                }
                else 
                {
                    comandoSql = @"UPDATE Produto SET 
                            Preco = @Preco,
                            PrecoAntigo = @PrecoAntigo,
                            DataBusca = @DataBusca
                         WHERE NomeProduto = @NomeProduto";
                }

                using (var cmd = new SqlCommand(comandoSql, _connection))
                {
                    cmd.Parameters.AddWithValue("@NomeProduto", item.NomeProduto);
                    cmd.Parameters.AddWithValue("@Preco", item.Preco.ToString("N2", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@PrecoAntigo", precoAntigo.ToString("N2", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@Link", item.Link);
                    cmd.Parameters.AddWithValue("@DataBusca", item.DataBusca);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private double BuscarPrecoAntigo(string nomeProduto)
        {
            string comandoSql = @"SELECT Preco FROM Produto WHERE NomeProduto = @NomeProduto";

            using (var cmd = new SqlCommand(comandoSql, _connection))
            {
                cmd.Parameters.AddWithValue("@NomeProduto", nomeProduto);
                var preco = cmd.ExecuteScalar();
                return preco == null ? 0 : Convert.ToDouble(preco);
            }
        }
        //public bool PrecoIgual(Produto produto)
        //{
        //    string comandoSql = @"SELECT COUNT(*) FROM Produto WHERE NomeProduto = (@NomeProduto) AND Preco = (@Preco);";

        //    using (var cmd = new SqlCommand(comandoSql, _connection))
        //    {
        //        cmd.Parameters.AddWithValue("@NomeProduto", produto.NomeProduto);
        //        cmd.Parameters.AddWithValue("@Preco", produto.Preco);
        //        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        //    }
        //}
    }
}

using HtmlAgilityPack;
using Robo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Robo.Buscar
{
    public class RastrearProdutosService : Produto
    {
        public List<Produto> Rastreio()
        {


            var urlBase = "https://www.lojasurfers.com.br/";
            var client = new HttpClient();
            var result = client.GetAsync("https://www.lojasurfers.com.br/vestuario?pagina=1").Result;

            Utf8EncodingProvider.Register();
            var html = result.Content.ReadAsStringAsync().Result;

            var totalPagina = 5;
            var paginas = Enumerable.Range(1, totalPagina);




            var listaProdutos = new List<Produto>();
            foreach (var pagina in paginas)
            {
                result = client.GetAsync("https://www.lojasurfers.com.br/vestuario?pagina=" + pagina).Result;
                html = result.Content.ReadAsStringAsync().Result;

                var doc = new HtmlDocument();
                doc.LoadHtml(html);


                var produtos = doc.DocumentNode.SelectNodes("//div[contains(@class, 'listagem-item')]");



                foreach (var produto in produtos)
                {
                    var model = new Produto();

                    var ativo = produto.SelectNodes(".//div[contains(@class, 'acoes-produto-responsiva')]");
                    if (ativo is null)
                        continue;
                    var elementoA = produto.Descendants("a").First();

                    var linkProduto = elementoA.Attributes["href"].Value;

                    var nomeProduto = produto.SelectNodes(".//a[contains(@class, 'nome-produto cor-secundaria')]").First().InnerText.Replace("\n", "");

                    var preco = produto.SelectNodes(".//span[contains(@class, 'desconto-a-vista')]").First().InnerText.Replace("\n", "").Replace(" ", "").Replace("R$", "");

                    model.NomeProduto = nomeProduto;
                    var precoText = EditarPreco(preco);
                    model.Preco = Convert.ToDouble(precoText);
                    model.Link = linkProduto;

                    listaProdutos.Add(model);
                }


               
            }
            return listaProdutos;
        }
        public string EditarPreco(string precoText)
        {
            var preco = precoText.Replace("ou", "").Replace("viaPix", "");
            return preco.ToString();
        }
    }

    public class Utf8EncodingProvider : EncodingProvider
    {
        public override Encoding GetEncoding(string name)
        {
            return name == "utf8" ? Encoding.UTF8 : null;
        }

        public override Encoding GetEncoding(int codepage)
        {
            return null;
        }

        public static void Register()
        {
            Encoding.RegisterProvider(new Utf8EncodingProvider());
        }
    }
   

}


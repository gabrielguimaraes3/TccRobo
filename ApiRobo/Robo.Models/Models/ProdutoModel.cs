using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robo.Models
{
    public class Produto
    {
        public string NomeProduto { get; set; }
        public double Preco { get; set; }
        public string Link { get; set; }
        public DateTime DataBusca { get; set; } = DateTime.Now;
        public double? PrecoAntigo { get; set; }

    }
}

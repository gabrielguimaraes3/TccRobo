using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcacaoDePonto.Models.Exeption
{
    public class ValidacoesExcepition : Exception
    {
        public ValidacoesExcepition() { }

        public ValidacoesExcepition(string message)
            : base(message) { }

        public ValidacoesExcepition(string message, Exception inner)
            : base(message, inner) { }
    }
}

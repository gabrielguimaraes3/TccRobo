using Robo.Buscar;
using Robo.Models;
using Robo.Repositories.Repositories;

namespace Robo.Services.Services
{
    public class RoboServices
    {
        private readonly RoboRepositorio _repositorio;
        public RoboServices()
        {
            _repositorio = new RoboRepositorio();
        }

        public List<Produto> Listar(string? nome)
        {
            try
            {
                _repositorio.AbrirConexao();
                return _repositorio.ListarProdutos(nome);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Inserir()
        {
            try
            {
                _repositorio.AbrirConexao();

                _repositorio.RoboInserir();
            }
            finally
            {
                _repositorio.FecharConexao();
            }
            ;

        }
    }
}
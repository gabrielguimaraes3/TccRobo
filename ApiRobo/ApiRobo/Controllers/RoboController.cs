using MarcacaoDePonto.Models.Exeption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robo.Services;
using Robo.Services.Services;

namespace ApiRobo.Controllers
{

    public class RoboController : ControllerBase
    {
        private readonly RoboServices _services;

        public RoboController()
        {
            _services = new RoboServices();
        }

        [HttpGet("robo")]
        public IActionResult Listar([FromQuery] string? nome)
        {
            return StatusCode(200, _services.Listar(nome));
        }

        [HttpPost("robo")]
        public IActionResult Inserir()
        {
            try
            {
                _services.Inserir();
                return StatusCode(200);
            }
            catch (ValidacoesExcepition ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}

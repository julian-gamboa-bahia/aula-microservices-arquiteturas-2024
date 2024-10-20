using Microsoft.AspNetCore.Mvc;
using RabbitMQDemo.Services;


namespace RabbitMQDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RabbitMQController : ControllerBase
    {
        private readonly RabbitMQService _rabbitMQService;

        public RabbitMQController(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] string message)
        {
            try
            {
                _rabbitMQService.SendMessage("fila_otimizar", message);
                return Ok($"[x] Enviado: {message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }

        [HttpGet("consume")]
        public IActionResult StartConsumer()
        {
            try
            {
                _rabbitMQService.StartConsumer("fila_otimizar");
                return Ok("Consumidor iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }
    }
}

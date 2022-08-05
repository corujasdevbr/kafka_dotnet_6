using Confluent.Kafka;
using Corujasdev.Messaging.Kafka.Api.Producer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Corujasdev.Messaging.Kafka.Api.Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetsController : ControllerBase
    {
       
        private readonly ILogger<PetsController> _logger;
        private ProducerConfig _config;

        public PetsController(ILogger<PetsController> logger, ProducerConfig config)
        {
            _logger = logger;
            _config = config;
            _config.BootstrapServers = "localhost:9092";
        }

        [HttpPost("send")]
        public async Task<ActionResult> Post(string topic, [FromBody] Pet pet)
        {
            string serializedEmployee = JsonConvert.SerializeObject(pet);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedEmployee });
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(result);
            }
        }
    }
}
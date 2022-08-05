using Confluent.Kafka;
using Serilog;

var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

logger.Information("Testando o consumo de mensagens com Kafka");

string bootstrapServers = "localhost:9092";
string nomeTopic = "new-pet";

logger.Information($"BootstrapServers = {bootstrapServers}");
logger.Information($"Topic = {nomeTopic}");

var config = new ConsumerConfig
{
    BootstrapServers = bootstrapServers,
    GroupId = $"{nomeTopic}-group-0",
    AllowAutoCreateTopics = true,
    AutoOffsetReset = AutoOffsetReset.Earliest
};

CancellationTokenSource cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};


    using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
    {
        consumer.Subscribe(nomeTopic);

        try
        {
            while (true)
            {
                var cr = consumer.Consume(cts.Token);
                logger.Information(
                    $"Mensagem lida: {cr.Message.Value}");
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
            logger.Warning("Cancelada a execução do Consumer...");
        }
    }

using BrokerConsumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

RabbitMQService service = new RabbitMQService();
var factory = service.GetFactory();
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();
await channel.QueueDeclareAsync(queue: "users", durable: false, exclusive: false, autoDelete: false, arguments: null);
while (true)
{
    service.Recieve(connection, channel);

    if (Console.ReadKey().Key == ConsoleKey.Enter) { break; }
}
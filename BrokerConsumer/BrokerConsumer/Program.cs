using BrokerConsumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

RabbitMQService service = new RabbitMQService();
while (true)
{
    service.Recieve();
    if (Console.ReadKey().Key == ConsoleKey.Enter) { break; }
}
using RabbitMQ.Client;
using System.Text;

namespace FurnitureShop
{
    public class RabbitMQService : IRabbitMQService
    {
        ConnectionFactory factory = new ConnectionFactory();
        public RabbitMQService() 
        {
            factory = new ConnectionFactory() { Uri = new Uri("amqps://bzjfwhny:o8wJbjylS7cGo9YmyeqO4qAiVUqeby8A@rat.rmq2.cloudamqp.com/bzjfwhny") };
        }
        public async void SendMessage(string message)
        {
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "users", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
        }
    }
}

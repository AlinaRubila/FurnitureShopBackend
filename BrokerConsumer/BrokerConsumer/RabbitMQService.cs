using RabbitMQ.Client;
using System;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Threading.Channels;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;


namespace BrokerConsumer
{
    public class RabbitMQService
    {
        ConnectionFactory _factory = new ConnectionFactory();
        string _path = @"C:\Users\angry\OneDrive\Рабочий стол\курсач\FurnitureShopBackend\BrokerConsumer\BrokerConsumer\users_log.txt";
        public RabbitMQService()
        {
            _factory = new ConnectionFactory() { Uri = new Uri("amqps://bzjfwhny:o8wJbjylS7cGo9YmyeqO4qAiVUqeby8A@rat.rmq2.cloudamqp.com/bzjfwhny") };
        }
        public async void Recieve()
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "users", durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            Console.WriteLine("Waiting for messages.");
            string info = "";
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                info += message;
                Console.WriteLine($" [x] Received {message}");
                return Task.CompletedTask;
            };
            await File.AppendAllTextAsync( _path, info );
            await channel.BasicConsumeAsync("users", autoAck: true, consumer: consumer);
        }
    }
}

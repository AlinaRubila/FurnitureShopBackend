using RabbitMQ.Client;
using System;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Threading.Channels;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using System.IO;


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
        public ConnectionFactory GetFactory()
        {
            return _factory;
        }
        public async void Recieve(IConnection connection, IChannel channel)
        {
            Console.WriteLine("Waiting for messages.");
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received {message}");
                await File.AppendAllTextAsync(_path, $"{message}\n");
            };
            await channel.BasicConsumeAsync("users", autoAck: true, consumer: consumer);
        }
    }
}

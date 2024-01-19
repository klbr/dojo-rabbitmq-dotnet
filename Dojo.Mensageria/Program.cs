using RabbitMQ.Client;
using Dojo.Mensageria.Model;
using System.Text;

namespace Dojo.Mensageria
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client Application started");
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
            Publisher publisher = Publisher.getPublisher();

            while (true)
            {
                (int number, int number2) = publisher.getNumbers();
                for (int i = number; i <= number2; i++)
                {
                    var body = Encoding.UTF8.GetBytes(i.ToString());
                    channel.BasicPublish(exchange: string.Empty, routingKey: "hello", basicProperties: null, body: body);
                    Console.WriteLine("Enviado o numero: " + i);
                }
                
            }
        }
    }
}
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
            Publisher publisher = Publisher.getPublisher();
            int number = publisher.getNumber();
            
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(number.ToString());

            channel.BasicPublish(exchange: string.Empty, routingKey: "hello", basicProperties: null, body: body);


            Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
        }
    }
}
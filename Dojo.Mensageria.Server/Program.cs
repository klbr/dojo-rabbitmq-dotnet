using RabbitMQ.Client;

namespace Dojo.Mensageria.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server Application started");
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
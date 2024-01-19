using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

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

            channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);


                //Console.WriteLine($" [x] Received {message}");
                var resposta = EPrimo(int.Parse(message));
                if(resposta)
                    Console.WriteLine("Resposta: numero:{0} é primo",message);
            };
            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        private static bool EPrimo(int numero)
        {
            if (numero <= 1 )
            {
                return false;
            }
                for (int i = 2; i < numero; i++)
            {
                if (numero % i == 0)
                    return false;

            }
            
            return true;
        }
    }

    
}
using RabbitMQ.Client;
using Dojo.Mensageria.Model;
using System.Text;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;

namespace Dojo.Mensageria
{
    internal class Program
    {
        static readonly string QueueName = "hello";

        class RpcClient
        {
            private readonly IConnection connection;
            private readonly IModel channel;

            ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

            public RpcClient()
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                connection = factory.CreateConnection();
                channel = connection.CreateModel();

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
                channel.BasicConsume(queue: QueueName,
                                autoAck: true,
                                consumer: consumer);
            }

            public Task<string> CallAsync(int number)
            {
                IBasicProperties props = channel.CreateBasicProperties();
                var correlationId = Guid.NewGuid().ToString();
                props.CorrelationId = correlationId;
                props.ReplyTo = QueueName;
                var body = Encoding.UTF8.GetBytes(number.ToString());
                var completationSource = new TaskCompletionSource<string>();
                callbackMapper.TryAdd(correlationId, completationSource);
                channel.BasicPublish(exchange: string.Empty, routingKey: QueueName, basicProperties: props, body: body);
                return completationSource.Task;

            }

            private void Consumer_Received(object? sender, BasicDeliverEventArgs e)
            {
                if (!callbackMapper.TryRemove(e.BasicProperties.CorrelationId, out var result))
                {
                    return;
                }

                var body = e.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                result.TrySetResult(response);
            }
        }

        static async void Main(string[] args)
        {
            Console.WriteLine("Client Application started");
            var factory = new ConnectionFactory { HostName = "localhost" };
            Publisher publisher = Publisher.getPublisher();
            var rpcClient = new RpcClient();

            while (true)
            {
                (int number, int number2) = publisher.getNumbers();
                for (int i = number; i <= number2; i++)
                {
                    Console.WriteLine("Enviado o numero: " + i);
                    var response = await rpcClient.CallAsync(i);
                    bool result = bool.Parse(response);
                    Console.WriteLine("Resultado:{0}", result ? "Sim" : "Não");
                }
                
            }
        }
    }
}
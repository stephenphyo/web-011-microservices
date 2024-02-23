using System.Text;
using System.Text.Json;
using PlatformService.DTOs;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusPublisher : IMessageBusPublisher
    {
        /* Properties */
        private readonly IConnection _connection;
        private readonly IModel _channel;

        /* Constructor */
        public MessageBusPublisher()
        {
            var factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
                Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"))
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQConnectionShutdown;

                Console.WriteLine("---RabbitMQ Message Bus Connection Successful---");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Message Bus Connection Failed: {e.Message}");
            }
        }

        /* Methods */
        public void PublishNewPlatform(PlatformPublishedDTO platform)
        {
            var message = JsonSerializer.Serialize(platform);
            if (_connection.IsOpen)
            {
                Console.WriteLine("---RabbitMQ Connection is Open---");
                PublishMessage(message);
            }
            else
            {
                Console.WriteLine("---RabbitMQ Connection is Close---");
            }
        }

        /* Private Methods */
        private void PublishMessage(string message)
        {
            var messageBody = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: messageBody
            );

            Console.WriteLine("---Message Published Successfully---");
        }
        private void DisposeChannel()
        {
            Console.WriteLine("RabbitMQ Message Bus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        private void RabbitMQConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("---RabbitMQ Connection Shutdown---");
        }
    }
}
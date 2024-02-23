using System.Text;
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        /* Properties */
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;


        /* Constructor */
        public MessageBusSubscriber(IEventProcessor eventProcessor)
        {
            _eventProcessor = eventProcessor;
            InitializeRabbitMQ();
        }

        /* Private Methods */
        private void InitializeRabbitMQ()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
                    Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"))
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");

                Console.WriteLine("---Listening on RabbitMQ Message Bus");
                _connection.ConnectionShutdown += RabbitMQConnectionShutdown;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Message Bus Connection Failed: {e.Message}");
            }
        }

        private void RabbitMQConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("---RabbitMQ Connection Shutdown---");
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var subscriber = new EventingBasicConsumer(_channel);
            subscriber.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("---Event Received---");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(
                queue: _queueName,
                consumer: subscriber,
                autoAck: true
            );

            return Task.CompletedTask;
        }
    }
}
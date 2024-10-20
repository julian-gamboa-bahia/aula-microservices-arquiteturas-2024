using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQDemo.Services
{
    public class RabbitMQService
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQService()
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq" };
            Connect();
        }

        public void Connect()
        {
            try
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    _connection = _factory.CreateConnection();
                    _channel = _connection.CreateModel();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar ao RabbitMQ: {ex.Message}");
                // Implementar lógica de retries ou reconexão com um tempo de espera
            }
        }


        public void SendMessage(string queue, string message)
        {
            if (_channel == null || !_channel.IsOpen)
            {
                Connect();
            }

            _channel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                 routingKey: queue,
                                 basicProperties: null,
                                 body: body);
        }

        public void StartConsumer(string queue)
        {
            if (_channel == null || !_channel.IsOpen)
            {
                Connect();
            }

            _channel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Recebido: {message}");
            };

            _channel.BasicConsume(queue: queue,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}

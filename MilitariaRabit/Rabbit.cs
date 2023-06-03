using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;

namespace MilitariaRabit
{
    public class Rabbit : IDisposable
    {
        private ConnectionFactory _connectionFactory;
        private IConnection? _connection;
        private IModel? _model;

        public Rabbit(string hostName = "localhost")
        {
            _connectionFactory = new ConnectionFactory() { HostName = hostName };
        }

        //Zmiana hosta
        public void ChangeHost(string hostName)
        {
            CloseEqueue();
            _connectionFactory = new ConnectionFactory() { HostName = hostName };
        }
        //zamknięcie otwartych połączeń 
        public void CloseEqueue()
        {
            if (_connection != null)
                if (_connection.IsOpen)
                    _connection.Close();

            if (_model != null)
                if (_model.IsOpen)
                    _model.Close();

        }

        //tworzenie nowej kolejki
        public void CreateEqueue(string queue)
        {
            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();
            _model.QueueDeclare(queue: queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
        }
        //Wysłanie nowej wiadomości
        public void SendMessage(string json, string routingKey = "")
        {
            if (string.IsNullOrWhiteSpace(routingKey))
            {
                routingKey = _model.CurrentQueue;
            }

            var body = Encoding.UTF8.GetBytes(json);
            _model.BasicPublish(exchange: "",
                    routingKey: routingKey,
                    basicProperties: null,
                    body: body);
        }
        //pobranie wiadomości
        public void GetMessage(string queue, Action<string> action)
        {
            string message = "";
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    message = Encoding.UTF8.GetString(body.ToArray());
#if DEBUG
                    //wyświetlenie wiadomości w trybie DEBUG
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\tTest działania biblioteki");
                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.Black;
#endif
                    //akcja którą użytkownik definiuje w cliencie
                    action(message);
                }
                catch (Exception)
                {
                    throw;
                }
            };
            _model.BasicConsume(queue: queue,
                    autoAck: true,
                    consumer: consumer);
        }


        public void Dispose()
        {
            _connectionFactory = null;
            CloseEqueue();
        }
    }
}

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Security;

namespace MDA.Messenger.RabbitMQ
{
    public sealed class Consumer
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IModel _channel;

        public Consumer()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "rattlesnake-01.rmq.cloudamqp.com",
                VirtualHost = "cswkixly",
                UserName = "cswkixly",
                Password = "MHvokiAUUQ-30kep2zd6bxRMS8Dy6XSx",
                Port = 5671,
                RequestedHeartbeat = TimeSpan.FromSeconds(10),
                Ssl = new SslOption
                {
                    Enabled = true,
                    AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch
                                             | SslPolicyErrors.RemoteCertificateChainErrors,
                    Version = System.Security.Authentication.SslProtocols.Tls11
                              | System.Security.Authentication.SslProtocols.Tls12
                }
            };
            _channel = _connectionFactory.CreateConnection().CreateModel();

        }

        /// <summary>
        /// Получение сообщения
        /// </summary>
        /// <param name="receiveCallBack">Объект сообщения</param>
        public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallBack)
        {
            _channel.ExchangeDeclare(exchange: "rest", type: ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                exchange: "rest",
                routingKey: "");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += receiveCallBack;
            _channel.BasicConsume(queueName, true, consumer);
        }
    }
}

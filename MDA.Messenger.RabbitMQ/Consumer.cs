using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Security;

namespace MDA.Messenger.RabbitMQ
{
    public sealed class Consumer
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IModel _channel;
        private readonly string _queue;

        public Consumer(string queue)
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
            _queue = queue;
        }

        public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallBack)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += receiveCallBack;
            _channel.BasicConsume(_queue, true, consumer);
        }
    }
}

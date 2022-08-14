using System.Net.Security;
using System.Text;
using RabbitMQ.Client;

namespace MDA.Messenger.RabbitMQ
{
    
    public sealed class Producer
    {
        private readonly  ConnectionFactory _connectionFactory;

        public Producer()
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
        }

        /// <summary>
        /// Отправка сообщение в очередь
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public void SendToQueue(string message)
        {

            var msg = Encoding.UTF8.GetBytes(message);
            if (msg.Length == 0) return;

            try
            {
                using (IConnection connection = _connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare("rest", ExchangeType.Fanout);
                        channel.BasicPublish(exchange: "rest", routingKey: "", body:msg);
                        channel.Close();
                    }

                    Console.WriteLine($"The message has been sent!");
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                
            }
        }
    }
}
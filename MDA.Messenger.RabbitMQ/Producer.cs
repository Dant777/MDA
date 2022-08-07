using System.Net.Security;
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

        public void SendToQueue(byte[] msg, string queue)
        {
            if(msg.Length == 0) return;

            try
            {
                using (IConnection connection = _connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.BasicPublish(exchange:"", routingKey: queue, body:msg);
                        channel.Close();
                    }

                    Console.WriteLine($"Success {queue}");
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
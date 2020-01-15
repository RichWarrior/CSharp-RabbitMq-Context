using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMq.Repository
{
    public  class SRabbitMq : IRabbitMq
    {
        IConnection context;
        public SRabbitMq(string hostName,int port,string userName,string password)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password =password
            };
            context = connectionFactory.CreateConnection();
        }
        public bool Publisher(string queueName, string body)
        {
            bool _rtn = true;
            try
            {
                using (IModel channel = context.CreateModel())
                {
                    channel.QueueDeclare(queueName, false, false, false, null);
                    byte[] arr = Encoding.UTF8.GetBytes(body);
                    channel.BasicPublish("", queueName, null, arr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return _rtn;
        }     

        public EventingBasicConsumer Consumer()
        {
            IModel channel = null;
            EventingBasicConsumer consumer = null;
            try
            {
                channel = context.CreateModel();
                consumer = new EventingBasicConsumer(channel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (channel != null)
                {
                    channel.Close();
                    channel.Dispose();
                    consumer = null;
                }
            }
            return consumer;
        }

        public IModel Consumer(EventingBasicConsumer consumer, string queueName, bool autoAck)
        {
            IModel channel = null;
            try
            {
                channel = context.CreateModel();
                channel.QueueDeclare(queueName, false, false, false, null);
                channel.BasicConsume(queueName, autoAck, consumer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (channel != null)
                {
                    channel.Close();
                    channel.Dispose();
                }
            }
            return channel;
        }
        public bool CreateQueue(string queueName)
        {
            bool _rtn = true;
            try
            {
                using (IModel channel = context.CreateModel())
                {
                    channel.QueueDeclare(queueName, false, false, false, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _rtn = false;
            }
            return _rtn;
        }

        public void Dispose()
        {
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
            context.Dispose();
        }
    }
}

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace RabbitMq.Repository
{
    public interface IRabbitMq :IDisposable
    {
        /// <summary>
        /// RabbitMq Kuyruğa Veri Eklemek İçin Kullanılır.
        /// </summary>
        /// <param name="queueName">Kuyruk Adı</param>
        /// <param name="body">Kuyruğa Konulacak Datanın İçeriği</param>
        /// <returns></returns>
        bool Publisher(string queueName, string body);
        /// <summary>
        /// Kuyruktaki Dataları Handle Etmek İçin Kullanılır.
        /// </summary>
        /// <returns></returns>
        EventingBasicConsumer Consumer();
        /// <summary>
        /// Kuyruktaki Dataları Handle Etme Eventini Kullanır.
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="queueName"></param>
        IModel Consumer(EventingBasicConsumer consumer, string queueName, bool autoAck);
        /// <summary>
        /// Kuyruk Oluşturmak İçin Kullanılır.
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        bool CreateQueue(string queueName);
    }
}

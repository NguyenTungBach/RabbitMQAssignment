using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQSend.Queue.IQueue;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQSend.Queue
{
    class QueueArticle: IQueueArticle
    {
        public void Sender(EventQueueArticle eventQueue)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "crawler",
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            Console.WriteLine(" [*] Waiting for messages. ");

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventQueue));
            channel.BasicPublish(exchange: "",
                                 routingKey: "crawler",
                                 basicProperties: null,
                                 body: body);
        }
    }
}

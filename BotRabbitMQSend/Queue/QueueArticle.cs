using BotRabbitMQSend.Queue.IQueue;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQSend.Queue
{
    class QueueArticle : IQueueArticle
    {
        public void Sender(EventQueueArticle eventQueue)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
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
}

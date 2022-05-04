using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQReceived.Queue.IQueue;
using RabbitMQReceived.Service;
using RabbitMQReceived.Service.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQReceived.Queue
{
    class QueueArticle : IQueueArticle
    {
        private IArticleService articleService;

        public QueueArticle()
        {
            articleService = new ArticleService();
        }

        public void Receiver()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "crawler",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var i = 0;
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                EventQueueArticle eventQueue = JsonConvert.DeserializeObject<EventQueueArticle>(message);
                var article = articleService.GetArticle(eventQueue);
                articleService.Save(article);
                Console.WriteLine(" [x] Received{0}:  {1}", i++, article.Url);
            };
            channel.BasicConsume(queue: "crawler",
                                 autoAck: true,
                                 consumer: consumer);
            Console.ReadLine();
        }

        
    }
}

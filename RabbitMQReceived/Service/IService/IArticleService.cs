using RabbitMQReceived.Entity;
using RabbitMQReceived.Queue;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQReceived.Service.IService
{
    interface IArticleService
    {
        Article Save(Article article);
        Article GetArticle(EventQueueArticle eventQueue);
    }
}

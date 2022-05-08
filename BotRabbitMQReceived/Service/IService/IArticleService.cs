using BotRabbitMQReceived.Entity;
using BotRabbitMQReceived.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQReceived.Service.IService
{
    interface IArticleService
    {
        Article Save(Article article);
        Article GetArticle(EventQueueArticle eventQueue);
    }
}

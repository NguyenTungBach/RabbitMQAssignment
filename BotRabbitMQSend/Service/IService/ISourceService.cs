using BotRabbitMQSend.Entity;
using BotRabbitMQSend.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQSend.Service.IService
{
    interface ISourceService
    {
        List<Source> GetAll();
        HashSet<EventQueueArticle> GetSubLink(Source source);
    }
}

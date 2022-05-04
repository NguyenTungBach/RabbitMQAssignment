using RabbitMQSend.Entity;
using RabbitMQSend.Queue;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQSend.Service.IService
{
    interface ISourceService
    {
        List<Source> GetAll();
        HashSet<EventQueueArticle> GetSubLink(Source source);
    }
}

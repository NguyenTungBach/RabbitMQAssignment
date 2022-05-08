using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQSend.Queue.IQueue
{
    interface IQueueArticle
    {
        void Sender(EventQueueArticle eventQueue);
    }
}

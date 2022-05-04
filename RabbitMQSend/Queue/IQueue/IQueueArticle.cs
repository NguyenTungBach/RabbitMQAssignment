using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQSend.Queue.IQueue
{
    interface IQueueArticle
    {
        public void Sender(EventQueueArticle eventQueue);
    }
}

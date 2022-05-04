using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQReceived.Queue.IQueue
{
    interface IQueueArticle
    {
        public void Receiver();
    }
}

using BotRabbitMQReceived.Queue;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQReceived
{
    class BotAppReceived : IJob
    {
        private QueueArticle queueArticle;
        public BotAppReceived()
        {
            queueArticle = new QueueArticle();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            queueArticle.Receiver();

        }
    }
}

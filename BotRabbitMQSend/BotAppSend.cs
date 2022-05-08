using BotRabbitMQSend.Queue;
using BotRabbitMQSend.Queue.IQueue;
using BotRabbitMQSend.Service;
using BotRabbitMQSend.Service.IService;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQSend
{
    class BotAppSend : IJob
    {
        private ISourceService sourceService;
        private IQueueArticle queueArticle;
        public BotAppSend()
        {
            sourceService = new SourceService();
            queueArticle = new QueueArticle();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var listSource = sourceService.GetAll();
            foreach (var source in listSource)
            {
                var listEvent = sourceService.GetSubLink(source);

                foreach (var eventQueue in listEvent)
                {
                    queueArticle.Sender(eventQueue);
                }
            }
        }
    }
}

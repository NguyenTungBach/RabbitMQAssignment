using Quartz;
using Quartz.Impl;
using RabbitMQReceived.Queue;
using RabbitMQReceived.Service;
using RabbitMQReceived.Service.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQReceived
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

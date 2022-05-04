using Quartz;
using Quartz.Impl;
using RabbitMQSend.Queue;
using RabbitMQSend.Queue.IQueue;
using RabbitMQSend.Service;
using RabbitMQSend.Service.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSend
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

        public async Task Start()
        {
            //var listSource = sourceService.GetAll();
            //foreach (var source in listSource)
            //{
            //    var listEvent = sourceService.GetSubLink(source);
                
            //    foreach (var eventQueue in listEvent)
            //    {
            //        queueArticle.Sender(eventQueue);
            //    }
            //}

            //StdSchedulerFactory factory = new StdSchedulerFactory();
            //IScheduler scheduler = await factory.GetScheduler();

            //// and start it off
            //await scheduler.Start();

            //IJobDetail job = JobBuilder.Create<ResetJob>()
            //    .WithIdentity("job1", "group1")
            //    .Build();

            //// Trigger the job to run now, and then repeat every 10 seconds
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("trigger1", "group1")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInSeconds(10)
            //        .RepeatForever())
            //    .Build();

            //// Tell quartz to schedule the job using our trigger
            //await scheduler.ScheduleJob(job, trigger);

            //// some sleep to show what's happening
            //await Task.Delay(TimeSpan.FromSeconds(2));

            //// and last shut down the scheduler when you are ready to close your program
            //await scheduler.Shutdown();
        }
    }

}

using BotRabbitMQSend.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQSend.Repository.IRepository
{
    interface ISourceRepository
    {
        List<Source> GetAll();
    }
}

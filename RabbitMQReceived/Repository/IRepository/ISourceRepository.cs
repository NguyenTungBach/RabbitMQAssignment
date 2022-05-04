using RabbitMQReceived.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQReceived.Repository.IRepository
{
    interface ISourceRepository
    {
        List<Source> GetAll();
    }
}

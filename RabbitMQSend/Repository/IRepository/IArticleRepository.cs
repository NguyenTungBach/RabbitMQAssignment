﻿using RabbitMQSend.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQSend.Repository.IRepository
{
    interface IArticleRepository
    {
        List<Article> GetAll();
        Article GetArticleByUrl(string urlSource);
    }
}

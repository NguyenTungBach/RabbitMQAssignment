using RabbitMQReceived.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQReceived.Repository.IRepository
{
    interface IArticleRepository
    {
        List<Article> GetAll();
        Article GetArticleByUrl(string urlSource);
        Article Save(Article article);
    }
}

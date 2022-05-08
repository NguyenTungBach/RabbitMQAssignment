using BotRabbitMQReceived.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQReceived.Repository.IRepository
{
    interface IArticleRepository
    {
        List<Article> GetAll();
        Article GetArticleByUrl(string urlSource);
        Article Save(Article article);
    }
}

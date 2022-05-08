using BotRabbitMQReceived.Entity;
using BotRabbitMQReceived.Queue;
using BotRabbitMQReceived.Repository;
using BotRabbitMQReceived.Repository.IRepository;
using BotRabbitMQReceived.Service.IService;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQReceived.Service
{
    class ArticleService : IArticleService
    {
        private IArticleRepository articleRepository;

        public ArticleService()
        {
            articleRepository = new ArticleRepository();
        }
        public Article GetArticle(EventQueueArticle eventQueue)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                var web = new HtmlWeb();
                HtmlDocument doc = web.Load(eventQueue.Url);
                var title = doc.QuerySelector(eventQueue.SelectorTitle)?.InnerText;
                var description = doc.QuerySelector(eventQueue.SelectorDescrition)?.InnerText;
                var imageNode = doc.QuerySelector(eventQueue.SelectorThumbnail)?.Attributes["data-src"].Value;
                var author = doc.QuerySelector(eventQueue.SelectorAuthor)?.InnerText;
                string thumbnail = "";
                if (imageNode != null)
                {
                    thumbnail = imageNode;
                }
                else
                {
                    thumbnail = "https://mucinmanhtai.com/wp-content/themes/BH-WebChuan-032320/assets/images/default-thumbnail-400.jpg";
                }
                var contentNode = doc.QuerySelectorAll(eventQueue.SelectorContent);
                StringBuilder contentBuilder = new StringBuilder();
                foreach (var content in contentNode)
                {
                    contentBuilder.Append(content.InnerText);
                }

                Article article = new Article()
                {
                    Url = eventQueue.Url,
                    Title = title,
                    Description = description,
                    Content = contentBuilder.ToString(),
                    Thumbnail = thumbnail,
                    Author = author,
                    SourceId = eventQueue.SourceId,
                    CreatedAt = DateTime.Now,
                };
                return article;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public Article Save(Article article)
        {
            return articleRepository.Save(article);
        }
    }
}

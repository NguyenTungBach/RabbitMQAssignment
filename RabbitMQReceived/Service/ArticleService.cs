﻿using HtmlAgilityPack;
using RabbitMQReceived.Entity;
using RabbitMQReceived.Queue;
using RabbitMQReceived.Repository;
using RabbitMQReceived.Repository.IRepository;
using RabbitMQReceived.Service.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQReceived.Service
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
                var title = doc.QuerySelector(eventQueue.SelectorTitle).InnerText;
                var description = doc.QuerySelector(eventQueue.SelectorDescrition).InnerText;
                var imageNode = doc.QuerySelector(eventQueue.SelectorThumbnail);
                var author = doc.QuerySelector(eventQueue.SelectorAuthor)?.InnerText;
                string thumbnail = "";
                if (imageNode != null)
                {
                    thumbnail = imageNode.Attributes["data-src"].Value;
                }
                else
                {
                    thumbnail = "";
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

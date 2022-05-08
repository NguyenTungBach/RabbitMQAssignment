using BotRabbitMQSend.Entity;
using BotRabbitMQSend.Queue;
using BotRabbitMQSend.Repository;
using BotRabbitMQSend.Service.IService;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQSend.Service
{
    class SourceService : ISourceService
    {
        private SourceRepository sourceRepository;
        private ArticleRepository articleRepository;
        public SourceService()
        {
            sourceRepository = new SourceRepository();
            articleRepository = new ArticleRepository();
        }

        public List<Source> GetAll()
        {
            articleRepository.TruncateArticle();
            return sourceRepository.GetAll();
        }

        public HashSet<EventQueueArticle> GetSubLink(Source source)
        {
            var web = new HtmlWeb();
            HtmlDocument doc = web.Load(source.Url);
            var nodeList = doc.QuerySelectorAll(source.SelectorUrl);
            HashSet<EventQueueArticle> eventQueues = new HashSet<EventQueueArticle>();
            int num = 0;
            foreach (var node in nodeList)
            {
                var link = node.Attributes["href"].Value;
                if (string.IsNullOrEmpty(link) || link.Contains("#box_comment_vne"))
                {
                    continue;
                }
                // Check urlSub đã có chưa nếu có rồi bỏ qua
                var existSubUrl = articleRepository.GetArticleByUrl(link);
                if (existSubUrl != null)
                {
                    continue;
                }
                num++;
                EventQueueArticle s = new EventQueueArticle(link, source);
                eventQueues.Add(s);
            }
            Console.WriteLine("So: " + num);
            return eventQueues;
        }
    }
}

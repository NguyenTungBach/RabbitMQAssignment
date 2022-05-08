using BotRabbitMQReceived.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQReceived.Queue
{
    class EventQueueArticle
    {
        public string Url { get; set; }
        public string SelectorTitle { get; set; }
        public string SelectorDescrition { get; set; }
        public string SelectorContent { get; set; }
        public string SelectorThumbnail { get; set; }
        public string SelectorAuthor { get; set; }
        public int SourceId { get; set; }

        public EventQueueArticle()
        {
        }

        public EventQueueArticle(string url, Source source)
        {
            this.Url = url;
            this.SelectorTitle = source.SelectorTitle;
            this.SelectorDescrition = source.SelectorDescription;
            this.SelectorContent = source.SelectorContent;
            this.SelectorThumbnail = source.SelectorThumbnail;
            this.SelectorAuthor = source.SelectorAuthor;
            this.SourceId = source.Id;
        }
    }
}

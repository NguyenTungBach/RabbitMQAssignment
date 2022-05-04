using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQSend.Entity
{
    class Article
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public string Author { get; set; }
        public int SourceId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

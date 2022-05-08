using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQSend.Entity
{
    class Source
    {
        public int Id { get; set; }
        public string Url { get; set; } // vnexpress.net/the-thao
        public string SelectorUrl { get; set; } // h1 .title-new, Các bài viết chọn trong trang thể thao vnxpess
        public string SelectorTitle { get; set; }
        public string SelectorDescription { get; set; }
        public string SelectorContent { get; set; }
        public string SelectorThumbnail { get; set; }
        public string SelectorAuthor { get; set; }
        public int CategoryId { get; set; }
    }
}

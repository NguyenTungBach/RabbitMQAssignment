using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RabbitMQAssignment.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
        [Column(TypeName = "ntext")]
        public string Thumbnail { get; set; }
        public string Author { get; set; }
        public int SourceId { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public string Tag { get; set; }
        //public DateTime UpdatedAt { get; set; }
        //public int Status { get; set; }
        [ForeignKey("SourceId")]
        public virtual Source Source { get; set; } //Link Bài viết này có nguồn từ trang nào VD lấy từ trang VN express
    }
}
using BotRabbitMQSend.Entity;
using BotRabbitMQSend.Repository.IRepository;
using BotRabbitMQSend.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRabbitMQSend.Repository
{
    class ArticleRepository : IArticleRepository
    {
        private string QueryAllArticle = "SELECT * FROM Articles";
        private string QueryGetArticleByUrl = "SELECT * FROM Articles WHERE Url = '@Url'";
        private string QueryTruncateArticle = "Truncate TABLE Articles";

        public List<Article> GetAll()
        {
            List<Article> articles = new List<Article>();
            try
            {
                using (var cnn = ConnectionHelper.GetConnectSql())
                {
                    cnn.Open();
                    var command = new SqlCommand(QueryAllArticle, cnn);
                    var data = command.ExecuteReader();
                    while (data.Read())
                    {
                        articles.Add(CreateArticle(data));
                    }
                    return articles;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ArticleRepository GetAll error: " + e.Message);
                throw;
            }
        }

        public void TruncateArticle()
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnectSql())
                {
                    cnn.Open();
                    var command = new SqlCommand(QueryTruncateArticle, cnn);
                    var data = command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ArticleRepository TruncateArticle error: " + e.Message);
                throw;
            }
        }

        public Article GetArticleByUrl(string urlSource)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnectSql())
                {
                    cnn.Open();
                    var command = new SqlCommand(QueryGetArticleByUrl, cnn);
                    command.Prepare();
                    command.Parameters.AddWithValue("@Url", urlSource);
                    var data = command.ExecuteReader();
                    return data.Read() ? CreateArticle(data) : null;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetArticleByUrl error :" + e.Message);
                throw;
            }
        }

        private Article CreateArticle(SqlDataReader data)
        {
            return new Article()
            {
                //Id = data.GetInt32(0),
                Url = data.GetString(1),
                Title = data.GetString(2),
                Description = data.GetString(3),
                Content = data.GetString(4),
                Thumbnail = data.GetString(5),
                Author = data.GetString(6),
                SourceId = data.GetInt32(7),
                CreatedAt = data.GetDateTime(8),
            };
        }
    }
}

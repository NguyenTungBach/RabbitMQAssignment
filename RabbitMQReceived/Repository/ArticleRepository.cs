using RabbitMQReceived.Entity;
using RabbitMQReceived.Repository.IRepository;
using RabbitMQReceived.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RabbitMQReceived.Repository
{
    class ArticleRepository : IArticleRepository
    {
        private string QueryAllArticle = "SELECT * FROM Articles";
        private string QueryGetArticleByUrl = "SELECT * FROM Articles WHERE UrlSource = @UrlSource";
        private string InsertQuery = "INSERT INTO Articles( Url, Title, Description, Content, Thumbnail, Author, SourceId, CreatedAt )" +
            " VALUES ( @Url, @Title, @Description, @Content, @Thumbnail,@Author, @SourceId, @CreatedAt ) ";
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
                Console.WriteLine(e.Message);
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
                    command.Parameters.AddWithValue("@UrlSource", urlSource);
                    var data = command.ExecuteReader();
                    return data.Read() ? CreateArticle(data) : null;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public Article Save(Article article)
        {
            try
            {
                using (var cnn = ConnectionHelper.GetConnectSql())
                {
                    cnn.Open();
                    var command = new SqlCommand(InsertQuery, cnn);
                    command.Prepare();
                    command.Parameters.AddWithValue("@Url", article.Url);
                    command.Parameters.AddWithValue("@Title", article.Title);
                    command.Parameters.AddWithValue("@Description", article.Description);
                    command.Parameters.AddWithValue("@Content", article.Content);
                    command.Parameters.AddWithValue("@Thumbnail", article.Thumbnail);
                    command.Parameters.AddWithValue("@Author", article.Author ?? "");
                    command.Parameters.AddWithValue("@SourceId", article.SourceId);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    command.ExecuteNonQuery();
                    return article;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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

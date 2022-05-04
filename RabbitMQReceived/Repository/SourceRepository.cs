﻿using RabbitMQReceived.Entity;
using RabbitMQReceived.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RabbitMQReceived.Repository
{
    class SourceRepository
    {
        private string QueryAllSource = "SELECT * FROM Sources";
        public List<Source> GetAll()
        {
            List<Source> sources = new List<Source>();
            try
            {
                using var cnn = ConnectionHelper.GetConnectSql();
                cnn.Open();
                var command = new SqlCommand(QueryAllSource, cnn);
                var data = command.ExecuteReader();
                while (data.Read())
                {
                    var source = new Source()
                    {
                        Id = data.GetInt32(0),
                        Url = data.GetString(1),
                        SelectorUrl = data.GetString(2),
                        SelectorTitle = data.GetString(3),
                        SelectorDescription = data.GetString(4),
                        SelectorContent = data.GetString(5),
                        SelectorThumbnail = data.GetString(6),
                        SelectorAuthor = data.GetString(7),
                        CategoryId = data.GetInt32(8),
                    };
                    sources.Add(source);
                    Console.WriteLine(source.ToString());
                }
                return sources;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

    }
}

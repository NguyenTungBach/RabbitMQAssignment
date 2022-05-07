using Elasticsearch.Net;
using Nest;
using RabbitMQAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RabbitMQAssignment.Util
{
    public class ElasticSearchService
    {
        public static ElasticClient searchClient;
        public static string IndexName = "articles";
        public static string DefaultIndexName = "example-index";
        public static string ElasticSearchUser = "elastic";
        public static string ElasticSearchPass = "kVaX4UEPjUu5jlP1FoYsooqR";
        public static string CloudId = "TestElastic:c291dGhlYXN0YXNpYS5henVyZS5lbGFzdGljLWNsb3VkLmNvbTo5MjQzJDYxMjVkNDNmNWU1NTQyZDI4MDI0ZWZmOTc2MTE5OTZkJDRhNmIxMjE2NjA2YjRhYWFiODRmMjIyMTRmNzFhZTYy";

        public static ElasticClient GetInstance()
        {
            if (searchClient == null)
            {
                var setting = new ConnectionSettings(CloudId,
                    new BasicAuthenticationCredentials(ElasticSearchUser, ElasticSearchPass))
                    .DefaultIndex(DefaultIndexName)
                    .DefaultMappingFor<Article>(i =>i.IndexName(IndexName));
                searchClient = new ElasticClient(setting);
            }

            return searchClient;
        }
    }
}
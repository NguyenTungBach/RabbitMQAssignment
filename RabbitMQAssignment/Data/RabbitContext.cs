using RabbitMQAssignment.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RabbitMQAssignment.Data
{
    public class RabbitContext : DbContext
    {
        public RabbitContext() : base("name=RabbitMQDB")
        {
        }

        public DbSet<Source> Sources { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}
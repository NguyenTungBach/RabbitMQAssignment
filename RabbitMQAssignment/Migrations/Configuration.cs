namespace RabbitMQAssignment.Migrations
{
    using RabbitMQAssignment.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RabbitMQAssignment.Data.RabbitContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RabbitMQAssignment.Data.RabbitContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );

            var categories = new List<Category>
            {
                new Category { Id = 1, Name="Thể thao"},
                new Category { Id = 2, Name="Sức khỏe"},
            };
            categories.ForEach(s => context.Categories.AddOrUpdate(s));

            var sources = new List<Source>
            {
                new Source { Id = 1, Url = "https://vnexpress.net/suc-khoe", SelectorUrl = "h3.title-news a", SelectorTitle = "h1.title-detail", SelectorDescription="p.description", SelectorContent = "article.fck_detail", SelectorThumbnail=".fig-picture picture img", SelectorAuthor="article.fck_detail p.author_mail", CategoryId=1},
            };
            sources.ForEach(s => context.Sources.AddOrUpdate(s));

            context.SaveChanges();
        }
    }
}

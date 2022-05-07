namespace RabbitMQAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Title = c.String(),
                        Description = c.String(storeType: "ntext"),
                        Content = c.String(storeType: "ntext"),
                        Thumbnail = c.String(storeType: "ntext"),
                        Author = c.String(),
                        SourceId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(),
                        Tag = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sources", t => t.SourceId, cascadeDelete: true)
                .Index(t => t.SourceId);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        SelectorUrl = c.String(),
                        SelectorTitle = c.String(),
                        SelectorDescription = c.String(),
                        SelectorContent = c.String(),
                        SelectorThumbnail = c.String(),
                        SelectorAuthor = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.Sources", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Sources", new[] { "CategoryId" });
            DropIndex("dbo.Articles", new[] { "SourceId" });
            DropTable("dbo.Categories");
            DropTable("dbo.Sources");
            DropTable("dbo.Articles");
        }
    }
}

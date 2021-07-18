using FluentMigrator;

namespace WorkerService.Migrations
{
    // How does this know about connection string?..
    [Migration(1)]
    public class AddLinkTable : Migration 
    {
        public override void Up()
        {
            // throw new System.NotImplementedException();
            Create.Table("links")
                .WithColumn("id").AsInt64().PrimaryKey()
                .WithColumn("title").AsString()
                .WithColumn("description").AsString()
                .WithColumn("uri").AsString();
        }

        public override void Down()
        {
            // throw new System.NotImplementedException();
            Delete.Table("links");
        }
    }
}
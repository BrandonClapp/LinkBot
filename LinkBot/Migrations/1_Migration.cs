using FluentMigrator;
using FluentMigrator.Postgres;

namespace LinkBot.Migrations
{
    // How does this know about connection string?..
    [Migration(1)]
    public class AddLinkTable : Migration 
    {
        public override void Up()
        {
            // throw new System.NotImplementedException();
            Create.Table("links")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("title").AsString()
                .WithColumn("description").AsString().Nullable()
                .WithColumn("uri").AsString().Unique()
                .WithColumn("image_uri").AsString().Nullable()
                .WithColumn("created_at").AsDateTime();
        }

        public override void Down()
        {
            // throw new System.NotImplementedException();
            Delete.Table("links");
        }
    }
}
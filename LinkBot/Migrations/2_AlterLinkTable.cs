using FluentMigrator;

namespace LinkBot.Migrations
{
    [Migration(2)]
    public class AlterLinkTable : Migration 
    {
        public override void Up()
        {
            Alter.Table("links")
                .AlterColumn("title").AsString().Nullable() // In case some links don't have parsable titles (i.e. JS Rendered Title).
                .AddColumn("server_id").AsInt64().Nullable()
                .AddColumn("server_name").AsString().Nullable()
                .AddColumn("channel_id").AsInt64().Nullable()
                .AddColumn("channel_name").AsString().Nullable()
                .AddColumn("author_id").AsInt64().Nullable()
                .AddColumn("author_name").AsString().Nullable()
                .AddColumn("tags").AsString().Nullable();
        }

        public override void Down()
        {
            Alter.Table("links")
                .AlterColumn("title").AsString().NotNullable();

            Delete.Column("server_id").FromTable("links");
            Delete.Column("server_name").FromTable("links");
            Delete.Column("channel_id").FromTable("links");
            Delete.Column("channel_name").FromTable("links");
            Delete.Column("author_id").FromTable("links");
            Delete.Column("author_name").FromTable("links");
            Delete.Column("tags").FromTable("links");
        }
    }
}
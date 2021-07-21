using FluentMigrator;

namespace LinkBot.Migrations
{
    [Migration(3)]
    public class RemoveTagsAlterColumnTypes : Migration
    {
        public override void Up()
        {
            Alter.Table("links")
                .AlterColumn("server_id").AsString().Nullable()
                .AlterColumn("channel_id").AsString().Nullable()
                .AlterColumn("author_id").AsString().Nullable();

            Delete.Column("tags").FromTable("links");
        }

        public override void Down()
        {
            Alter.Table("links")
                .AlterColumn("server_id").AsInt64().Nullable()
                .AlterColumn("channel_id").AsInt64().Nullable()
                .AlterColumn("author_id").AsInt64().Nullable()
                .AddColumn("tags").AsString().Nullable();

        }
    }
}
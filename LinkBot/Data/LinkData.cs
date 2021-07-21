using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using LinkBot.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Npgsql;

namespace LinkBot.Data
{
    public class LinkData
    {
        private readonly string _connString;
        
        public LinkData(IOptions<BotConfig> config)
        {
            _connString = config.Value.ConnectionString;
        }

        public async Task<IEnumerable<LinkPreview>> GetLinks()
        {
            try
            {
                await using var conn = new NpgsqlConnection(_connString);
                var links = await conn.QueryAsync<LinkPreview>("SELECT * FROM links");
                return links;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task SaveLink(LinkPreview link)
        {
            try
            {
                await using var conn = new NpgsqlConnection(_connString);
                await conn.ExecuteAsync(@"
                  insert into links (title, description, uri, image_uri, created_at, server_id, server_name, channel_id, channel_name, author_id, author_name)
                    values (@Title, @Desc, @Uri, @ImageUri, @CreatedAt, @ServerId, @ServerName, @ChannelId, @ChannelName, @AuthorId, @AuthorName) 
                    ON CONFLICT DO NOTHING;
            ", new
                {
                    Title = link.Title,
                    Desc = link.Description,
                    Image = link.ImageUri,
                    Uri = link.Uri,
                    ImageUri = link.ImageUri,
                    CreatedAt = link.CreatedAt,
                    ServerId = link.ServerId,
                    ServerName = link.ServerName,
                    ChannelId = link.ChannelId,
                    ChannelName = link.ChannelName,
                    AuthorId = link.AuthorId,
                    AuthorName = link.AuthorName
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
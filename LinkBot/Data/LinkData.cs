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

        // Can we inject HttpClient into here? How?
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
                // TODO: SQL make sure the uri doesn't already exist
                await conn.ExecuteAsync(@"
                  insert into links (title, description, uri, image_uri, created_at)
                    values (@Title, @Desc, @Uri, @ImageUri, @CreatedAt) 
                    ON CONFLICT DO NOTHING;
            ", new
                {
                    Title = link.Title,
                    Desc = link.Description,
                    Image = link.ImageUri,
                    Uri = link.Uri,
                    ImageUri = link.ImageUri,
                    CreatedAt = link.CreatedAt
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using WorkerService.Models;

namespace WorkerService
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
                await using var conn = new SqlConnection(_connString);
                var links = await conn.QueryAsync<LinkPreview>("SELECT * FROM links");
                return links;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
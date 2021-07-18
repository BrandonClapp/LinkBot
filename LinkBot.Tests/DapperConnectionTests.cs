using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;
using Xunit;

namespace LinkBot.Tests
{
    public class DapperConnectionTests
    {
        [Fact]
        public async Task CanQueryDb()
        {
            var blank = "<insert conn string here>";
            var connString = blank; // Change me

            if (connString == blank)
            {
                return;
            }

            await using var conn = new NpgsqlConnection(connString);
            var response = (await conn.QueryAsync<dynamic>("SELECT * FROM links;")).ToList();
            Assert.NotNull(conn);
        }
    }
}
using Microsoft.Data.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

    }
}

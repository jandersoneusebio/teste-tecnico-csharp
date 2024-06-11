using System.Data;

namespace Questao5.Infrastructure.Database
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}

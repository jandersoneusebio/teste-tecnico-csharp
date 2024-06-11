using Dapper;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using System;
using System.Data;

namespace Questao5.Infrastructure.Database.Repository
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public IdempotenciaRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<IdempotenciaEntity>> GetAll()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                var contas = await dbConnection.QueryAsync<IdempotenciaEntity>("SELECT * FROM idempotencia");
                return contas;
            }
        }

        public async Task<IdempotenciaEntity> GetById(string id)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                var resultado = await dbConnection.QueryFirstOrDefaultAsync<IdempotenciaEntity>("SELECT * FROM idempotencia WHERE chave_idempotencia = @Id ", new { Id = id });
                return resultado;
            }
        }

        public async Task<IdempotenciaEntity> Salvar(IdempotenciaEntity idempotenciaEntity)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {

                var sql = "INSERT INTO idempotencia " +
                          "(chave_idempotencia, requisicao, resultado) " +
                          "VALUES " +
                          "(@Chave_Idempotencia, @Requisicao, @Resultado)";
                await dbConnection.ExecuteAsync(sql, idempotenciaEntity);

                return idempotenciaEntity;
            }

        }
    }
}

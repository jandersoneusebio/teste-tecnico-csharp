using Dapper;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using System;
using System.Data;

namespace Questao5.Infrastructure.Database.Repository
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public MovimentacaoRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<MovimentacaoEntity>> GetAll()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                var movimentacoes = await dbConnection.QueryAsync<MovimentacaoEntity>("SELECT * FROM movimento");
                return movimentacoes;
            }
        }

        public async Task<MovimentacaoEntity> Salvar(MovimentacaoEntity movimentacaoEntity)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                if (movimentacaoEntity.IdMovimento == null)
                {
                    movimentacaoEntity.IdMovimento = Guid.NewGuid().ToString();
                }

                var sql = "INSERT INTO movimento " +
                          "(idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                          "VALUES " +
                          "(@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";
                await dbConnection.ExecuteAsync(sql, movimentacaoEntity);

                return movimentacaoEntity;
            }

            throw new ApplicationException("Entidade não persistida");
        }

        public async Task<double> ConsultarSaldo(string idConta)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                var sql = "SELECT " +
                          "COALESCE(" +
                          "     SUM(CASE WHEN mov.tipomovimento = 'C' THEN mov.valor ELSE 0 END) - SUM(CASE WHEN mov.tipomovimento = 'D' THEN mov.valor ELSE 0 END)" +
                          ", 0) AS SALDO " +
                          "FROM movimento mov WHERE mov.idcontacorrente = @IdConta ";
                var teste = await dbConnection.QueryFirstAsync(sql, new { IdConta = idConta });
                SaldoEntity saldo = await dbConnection.QueryFirstOrDefaultAsync<SaldoEntity>(sql, new { IdConta = idConta });

                return saldo != null ? saldo.Saldo : 0;
            }
        }
    }
}

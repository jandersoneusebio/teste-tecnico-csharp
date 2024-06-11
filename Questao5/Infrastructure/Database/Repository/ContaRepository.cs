using Dapper;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using System;
using System.Data;

namespace Questao5.Infrastructure.Database.Repository
{
    public class ContaRepository : IContaRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public ContaRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<ContaEntity>> GetAll()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                var contas = await dbConnection.QueryAsync<ContaEntity>("SELECT * FROM contacorrente");
                return contas;
            }
        }

        public async Task<ContaEntity> GetById(string id)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                var conta = await dbConnection.QueryFirstOrDefaultAsync<ContaEntity>("SELECT * FROM contacorrente WHERE idcontacorrente = @Id ", new { Id = id });
                return conta;
            }
        }

        public async Task<ContaEntity> GetByNumero(int numeroConta)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                var conta = await dbConnection.QueryFirstAsync<ContaEntity>("SELECT * FROM contacorrente WHERE numero = @NumeroConta ", new { NumeroConta = numeroConta });
                return conta;
            }
        }
    }
}

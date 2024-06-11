using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repository
{
    public interface IContaRepository
    {
        public Task<IEnumerable<ContaEntity>> GetAll();
        public Task<ContaEntity> GetById(string id);
        public Task<ContaEntity> GetByNumero(int numeroConta);
    }
}

using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repository
{
    public interface IIdempotenciaRepository
    {
        public Task<IEnumerable<IdempotenciaEntity>> GetAll();
        public Task<IdempotenciaEntity> GetById(string id);
        public Task<IdempotenciaEntity> Salvar(IdempotenciaEntity idempotenciaEntity);
    }
}

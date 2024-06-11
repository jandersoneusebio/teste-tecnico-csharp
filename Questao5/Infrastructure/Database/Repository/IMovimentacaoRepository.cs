using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repository
{
    public interface IMovimentacaoRepository
    {
        public Task<IEnumerable<MovimentacaoEntity>> GetAll();
        public Task<MovimentacaoEntity> Salvar(MovimentacaoEntity movimentacaoEntity);
        public Task<SaldoEntity> ConsultarSaldo(string idConta);
    }
}

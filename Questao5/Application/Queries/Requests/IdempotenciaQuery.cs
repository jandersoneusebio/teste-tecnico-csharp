using MediatR;

namespace Questao5.Application.Queries.Requests
{
    public class IdempotenciaQuery : IRequest<IdempotenciaQueryResponse>
    {
        public string IdRequisicao { get; set; }
    }
}

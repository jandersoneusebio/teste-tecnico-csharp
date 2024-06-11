using MediatR;

namespace Questao5.Application.Queries.Requests
{
    public class GetContaByIdQuery : IRequest<ContaQueryResponse>
    {
        public string IdContaCorrente { get; set; }

        public GetContaByIdQuery(string idContaCorrente)
        {
            IdContaCorrente = idContaCorrente;
        }
    }
}

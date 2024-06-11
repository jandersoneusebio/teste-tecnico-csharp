using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Exceptions;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Repository;

namespace Questao5.Application.Handlers
{
    public class IdempotenciaHandler : IRequestHandler<IdempotenciaQuery, IdempotenciaQueryResponse>
    {
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        public IdempotenciaHandler(IIdempotenciaRepository idempotenciaRepository)
        {
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<IdempotenciaQueryResponse> Handle(IdempotenciaQuery request, CancellationToken cancellationToken)
        {
            IdempotenciaEntity entity = await _idempotenciaRepository.GetById(request.IdRequisicao);

            if(entity == null)
            {
                return new IdempotenciaQueryResponse();
            }

            IdempotenciaQueryResponse response = new IdempotenciaQueryResponse(entity.Resultado);

            return response;
        }
    }
}

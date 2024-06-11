
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Database.Repository;

namespace Questao5.Application.Handlers
{
    public class ContaHandler : IRequestHandler<GetContaByIdQuery, ContaQueryResponse>
    {
        private readonly IMediator _mediator;
        private readonly IContaRepository _contaRepository;

        public ContaHandler(IMediator mediator, IContaRepository contaRepository)
        {
            _mediator = mediator;
            _contaRepository = contaRepository;
        }

        public async Task<ContaQueryResponse> Handle(GetContaByIdQuery request, CancellationToken cancellationToken)
        {

            ContaEntity entity = await _contaRepository.GetById(request.IdContaCorrente);

            if (entity == null)
            {
                return null;
            }

            ContaQueryResponse response = new ContaQueryResponse(entity.IdContaCorrente, entity.Numero, entity.Nome, entity.Ativo);

            return response;
        }
    }
}

using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Exceptions;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.Repository;

namespace Questao5.Application.Handlers
{
    public class SaldoHandler : IRequestHandler<SaldoCommand, SaldoResponse>
    {
        private readonly IMediator _mediator;
        private readonly IMovimentacaoRepository _movimentacaoRepository;

        public SaldoHandler(IMediator mediator, IMovimentacaoRepository movimentacaoRepository)
        {
            _mediator = mediator;
            _movimentacaoRepository = movimentacaoRepository;
        }

        public async Task<SaldoResponse> Handle(SaldoCommand request, CancellationToken cancellationToken)
        {
            ValidarConta(request.IdentificacaoConta);
            SaldoEntity saldo = await _movimentacaoRepository.ConsultarSaldo(request.IdentificacaoConta);

            return new SaldoResponse
            {
                Saldo = saldo.Saldo,
                NomeTitular = saldo.Nome,
                NumeroConta = saldo.Numero,
                DataHoraResponse = DateTime.Now.ToString()
            };
        }

        private void ValidarConta(string identificacaoConta)
        {
            ContaQueryResponse response = _mediator.Send(new GetContaByIdQuery(identificacaoConta)).Result;

            if (response == null)
            {
                throw new RegraNegocioException(RegraNegocioErro.INVALID_ACCOUNT, Mensagens.Get(RegraNegocioErro.INVALID_ACCOUNT_BALANCE));
            }

            if (response.Ativo != 1)
            {
                throw new RegraNegocioException(RegraNegocioErro.INACTIVE_ACCOUNT, Mensagens.Get(RegraNegocioErro.INACTIVE_ACCOUNT_BALANCE));
            }
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Exceptions;
using Questao5.Application.Notifications;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.Repository;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoHandler : IRequestHandler<MovimentacaoCommand, MovimentacaoResponse>
    {
        private readonly IMediator _mediator;
        private readonly IMovimentacaoRepository _movimentacaoRepository;

        public MovimentacaoHandler(IMediator mediator, IMovimentacaoRepository movimentacaoRepository)
        {
            _mediator = mediator;
            _movimentacaoRepository = movimentacaoRepository;
        }

        public Task<MovimentacaoResponse> Handle(MovimentacaoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ValidarRegrasNegocioMovimentacao(request);

                VerificarIdempotencia(request);

                MovimentacaoEntity entity = MontarMovimentacaoEntity(request);

                _movimentacaoRepository.Salvar(entity);

                MovimentacaoResponse response = new MovimentacaoResponse { IdMovimento = entity.IdMovimento };

                PublicarIdempotenciaNotification(request, response);

                return Task.FromResult(response);
            }
            catch (IdempotenciaException iex)
            {
                return Task.FromResult(iex.Response);
            }

        }

        private MovimentacaoEntity MontarMovimentacaoEntity(MovimentacaoCommand request)
        {
            return new MovimentacaoEntity
            {
                IdContaCorrente = request.IdentificacaoConta,
                DataMovimento = DateTime.Now.ToString(),
                TipoMovimento = request.TipoMovimentacao,
                Valor = request.Valor
            };
        }

        private void ValidarRegrasNegocioMovimentacao(MovimentacaoCommand command)
        {
            ValidarTipoMovimentacao(command.TipoMovimentacao);
            ValidarValor(command.Valor);
            ValidarConta(command.IdentificacaoConta);
        }

        private void ValidarTipoMovimentacao(string tipoMovimentacao)
        {
            if (tipoMovimentacao == null || (tipoMovimentacao.ToUpper() != "C" && tipoMovimentacao.ToUpper() != "D"))
            {
                throw new RegraNegocioException(RegraNegocioErro.INVALID_TYPE);
            }
        }

        private void ValidarValor(double valor)
        {
            if (valor <= 0)
            {
                throw new RegraNegocioException(RegraNegocioErro.INVALID_VALUE);
            }
        }

        private void ValidarConta(string identificacaoConta)
        {
            ContaQueryResponse response = _mediator.Send(new GetContaByIdQuery(identificacaoConta)).Result;

            if (response == null)
            {
                throw new RegraNegocioException(RegraNegocioErro.INVALID_ACCOUNT);
            }

            if (response.Ativo != 1)
            {
                throw new RegraNegocioException(RegraNegocioErro.INACTIVE_ACCOUNT);
            }
        }

        private void PublicarIdempotenciaNotification(MovimentacaoCommand command, MovimentacaoResponse response)
        {
            IdempotenciaNotification notification = new IdempotenciaNotification
            {
                IdRequisicao = command.IdentificacaoRequisicao,
                Requisicao = JsonConvert.SerializeObject(command),
                Resultado = JsonConvert.SerializeObject(response)
            };
            
            _mediator.Publish(notification);
        }

        private void VerificarIdempotencia(MovimentacaoCommand request)
        {
            IdempotenciaQueryResponse response = _mediator.Send(new IdempotenciaQuery { IdRequisicao = request.IdentificacaoRequisicao }).Result;
            
            if(response.IsDuplicado)
            {
                throw new IdempotenciaException(response.Response);
            }
        }
    }
}

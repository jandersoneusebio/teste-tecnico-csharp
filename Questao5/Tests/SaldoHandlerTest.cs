using MediatR;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Exceptions;
using Questao5.Application.Handlers;
using Questao5.Application.Notifications;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.Repository;
using Xunit;

namespace Questao5.Tests
{
    public class SaldoHandlerTest
    {
        private static ContaQueryResponse contaQueryResponseMock = new ContaQueryResponse("F475F943-7067-ED11-A06B-7E5DFA4A16C9", 123, "Fulano", 1);
        private SaldoResponse saldoResponseMock = new SaldoResponse 
        { 
            NumeroConta = contaQueryResponseMock.Numero, 
            NomeTitular = contaQueryResponseMock.Nome,
            Saldo = 3.5,
            DataHoraResponse = DateTime.Now.ToString()
        };

        private SaldoEntity saldoEntityMock = new SaldoEntity
        {
            Numero = contaQueryResponseMock.Numero,
            Nome = contaQueryResponseMock.Nome,
            Saldo = 3.5
        };

        private ContaEntity contaEntityMock = new ContaEntity
        {
            Numero = contaQueryResponseMock.Numero,
            Nome = contaQueryResponseMock.Nome
        };

        [Fact]
        public async Task Handle_DeveConsultarComSucesso()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            var contaRepository = Substitute.For<IContaRepository>();
            var saldoHandler = new SaldoHandler(mediatorMock, movimentacaoRepository, contaRepository);

            var saldoQuery = new SaldoCommand
            {
                IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9"
            };

            var contaQuery = new GetContaByIdQuery(saldoQuery.IdentificacaoConta);

            mediatorMock.Send(contaQuery).ReturnsForAnyArgs(contaQueryResponseMock);
            movimentacaoRepository.ConsultarSaldo(saldoQuery.IdentificacaoConta).ReturnsForAnyArgs(5);
            contaRepository.GetById(saldoQuery.IdentificacaoConta).ReturnsForAnyArgs(contaEntityMock);

            var result = await saldoHandler.Handle(saldoQuery, CancellationToken.None);

            await movimentacaoRepository.Received(1).ConsultarSaldo(Arg.Is<string>(mov => mov == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));

        }

        [Fact]
        public async Task Handle_DeveRetornarInvalidAccount()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            var contaRepository = Substitute.For<IContaRepository>();
            var saldoHandler = new SaldoHandler(mediatorMock, movimentacaoRepository, contaRepository);

            var saldoQuery = new SaldoCommand
            {
                IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9"
            };
            var contaQuery = new GetContaByIdQuery(saldoQuery.IdentificacaoConta);

            mediatorMock.Send(contaQuery).ReturnsNullForAnyArgs();

            var exception = await Assert.ThrowsAsync<RegraNegocioException>(async () => await saldoHandler.Handle(saldoQuery, CancellationToken.None));


            Assert.Equal(RegraNegocioErro.INVALID_ACCOUNT, exception.Erro);

            await movimentacaoRepository.Received(0).Salvar(Arg.Is<MovimentacaoEntity>(mov => mov.IdContaCorrente == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));
            await mediatorMock.Received(0).Publish(Arg.Is<IdempotenciaNotification>(notification => notification.IdRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C3"));
            
        }

        [Fact]
        public async Task Handle_DeveRetornarInactiveAccount()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            var contaRepository = Substitute.For<IContaRepository>();
            var saldoHandler = new SaldoHandler(mediatorMock, movimentacaoRepository, contaRepository);

            var saldoQuery = new SaldoCommand
            {
                IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9"
            };
            var contaQuery = new GetContaByIdQuery(saldoQuery.IdentificacaoConta);
            var contaQueryResponseMock = new ContaQueryResponse("F475F943-7067-ED11-A06B-7E5DFA4A16C9", 123, "Fulano", 0);

            mediatorMock.Send(contaQuery).ReturnsForAnyArgs(contaQueryResponseMock);

            var exception = await Assert.ThrowsAsync<RegraNegocioException>(async () => await saldoHandler.Handle(saldoQuery, CancellationToken.None));


            Assert.Equal(RegraNegocioErro.INACTIVE_ACCOUNT, exception.Erro);

            await movimentacaoRepository.Received(0).Salvar(Arg.Is<MovimentacaoEntity>(mov => mov.IdContaCorrente == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));
            await mediatorMock.Received(0).Publish(Arg.Is<IdempotenciaNotification>(notification => notification.IdRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C3"));

        }

    }
}

using MediatR;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Questao5.Application.Commands.Requests;
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
    public class MovimentacaoHandlerTest
    {
        private ContaQueryResponse contaQueryResponseMock = new ContaQueryResponse("F475F943-7067-ED11-A06B-7E5DFA4A16C9", 123, "Fulano", 1);

        [Fact]
        public async Task Handle_DeveCreditarComSucesso()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            var movimentacaoHandler = new MovimentacaoHandler(mediatorMock, movimentacaoRepository);
            var idempotenciaQueryResponseMock = new IdempotenciaQueryResponse();
            var movimentacaoCommand = new MovimentacaoCommand
            {
                IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                IdentificacaoRequisicao = "B6BAFC09-6967-ED11-A567-055DFA4A16C2",
                TipoMovimentacao = "C",
                Valor = 5
            };
            var idempotenciaQuery = new IdempotenciaQuery { IdRequisicao = movimentacaoCommand.IdentificacaoRequisicao };
            var contaQuery = new GetContaByIdQuery(movimentacaoCommand.IdentificacaoConta);

            mediatorMock.Send(contaQuery).ReturnsForAnyArgs(contaQueryResponseMock);
            mediatorMock.Send(idempotenciaQuery).ReturnsForAnyArgs(idempotenciaQueryResponseMock);

            var result = await movimentacaoHandler.Handle(movimentacaoCommand, CancellationToken.None);

            var notification = new IdempotenciaNotification
            {
                IdRequisicao = "B6BAFC09-6967-ED11-A567-055DFA4A16C2",
                Requisicao = JsonConvert.SerializeObject(movimentacaoCommand),
                Resultado = JsonConvert.SerializeObject(result)
            };

            await movimentacaoRepository.Received(1).Salvar(Arg.Is<MovimentacaoEntity>(mov => mov.IdContaCorrente == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));
            await mediatorMock.Received(1).Publish(Arg.Is<IdempotenciaNotification>(notification => notification.IdRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C2"));

        }

        [Fact]
        public async Task Handle_DeveDebitarComSucesso()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            var movimentacaoHandler = new MovimentacaoHandler(mediatorMock, movimentacaoRepository);
            var idempotenciaQueryResponseMock = new IdempotenciaQueryResponse();
            var movimentacaoCommand = new MovimentacaoCommand
            {
                IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                IdentificacaoRequisicao = "B6BAFC09-6967-ED11-A567-055DFA4A16C3",
                TipoMovimentacao = "D",
                Valor = 5
            };
            var idempotenciaQuery = new IdempotenciaQuery { IdRequisicao = movimentacaoCommand.IdentificacaoRequisicao };
            var contaQuery = new GetContaByIdQuery(movimentacaoCommand.IdentificacaoConta);

            mediatorMock.Send(contaQuery).ReturnsForAnyArgs(contaQueryResponseMock);
            mediatorMock.Send(idempotenciaQuery).ReturnsForAnyArgs(idempotenciaQueryResponseMock);

            var result = await movimentacaoHandler.Handle(movimentacaoCommand, CancellationToken.None);

            var notification = new IdempotenciaNotification
            {
                IdRequisicao = "B6BAFC09-6967-ED11-A567-055DFA4A16C2",
                Requisicao = JsonConvert.SerializeObject(movimentacaoCommand),
                Resultado = JsonConvert.SerializeObject(result)
            };

            await movimentacaoRepository.Received(1).Salvar(Arg.Is<MovimentacaoEntity>(mov => mov.IdContaCorrente == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));
            await mediatorMock.Received(1).Publish(Arg.Is<IdempotenciaNotification>(notification => notification.IdRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C3"));

        }

        [Fact]
        public async Task Handle_DeveRetornarIdempotencia()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            var movimentacaoHandler = new MovimentacaoHandler(mediatorMock, movimentacaoRepository);
            var contaQueryResponseMock = new ContaQueryResponse("F475F943-7067-ED11-A06B-7E5DFA4A16C9", 123, "Fulano", 1);
            var idempotenciaQueryResponseMock = new IdempotenciaQueryResponse(" { \"idMovimento\": \"123\" } ");

            var movimentacaoCommand = new MovimentacaoCommand
            {
                IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                IdentificacaoRequisicao = "B6BAFC09-6967-ED11-A567-055DFA4A16C2",
                TipoMovimentacao = "C",
                Valor = 5
            };

            var contaQuery = new GetContaByIdQuery(movimentacaoCommand.IdentificacaoConta);

            var idempotenciaQuery = new IdempotenciaQuery { IdRequisicao = movimentacaoCommand.IdentificacaoRequisicao };

            mediatorMock.Send(contaQuery).ReturnsForAnyArgs(contaQueryResponseMock);
            mediatorMock.Send(idempotenciaQuery).ReturnsForAnyArgs(idempotenciaQueryResponseMock);

            var result = await movimentacaoHandler.Handle(movimentacaoCommand, CancellationToken.None);

            var notification = new IdempotenciaNotification
            {
                IdRequisicao = "B6BAFC09-6967-ED11-A567-055DFA4A16C2",
                Requisicao = JsonConvert.SerializeObject(movimentacaoCommand),
                Resultado = JsonConvert.SerializeObject(result)
            };

            await movimentacaoRepository.Received(0).Salvar(Arg.Is<MovimentacaoEntity>(mov => mov.IdContaCorrente == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));
            await mediatorMock.Received(0).Publish(Arg.Is<IdempotenciaNotification>(notification => notification.IdRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C2"));

        }

        [Fact]
        public async Task Handle_DeveRetornarInvalidAccount()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            var movimentacaoHandler = new MovimentacaoHandler(mediatorMock, movimentacaoRepository);
            var movimentacaoCommand = new MovimentacaoCommand
            {
                IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                IdentificacaoRequisicao = "B6BAFC09-6967-ED11-A567-055DFA4A16C3",
                TipoMovimentacao = "D",
                Valor = 5
            };
            var contaQuery = new GetContaByIdQuery(movimentacaoCommand.IdentificacaoConta);

            mediatorMock.Send(contaQuery).ReturnsNullForAnyArgs();

            var exception = await Assert.ThrowsAsync<RegraNegocioException>(async () => await movimentacaoHandler.Handle(movimentacaoCommand, CancellationToken.None));


            Assert.Equal(RegraNegocioErro.INVALID_ACCOUNT, exception.Erro);

            await movimentacaoRepository.Received(0).Salvar(Arg.Is<MovimentacaoEntity>(mov => mov.IdContaCorrente == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));
            await mediatorMock.Received(0).Publish(Arg.Is<IdempotenciaNotification>(notification => notification.IdRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C3"));
            
        }

        [Fact]
        public async Task Handle_DeveRetornarInactiveAccount()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            var movimentacaoHandler = new MovimentacaoHandler(mediatorMock, movimentacaoRepository);
            var movimentacaoCommand = new MovimentacaoCommand
            {
                IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                IdentificacaoRequisicao = "B6BAFC09-6967-ED11-A567-055DFA4A16C3",
                TipoMovimentacao = "D",
                Valor = 5
            };
            var contaQuery = new GetContaByIdQuery(movimentacaoCommand.IdentificacaoConta);
            var contaQueryResponseMock = new ContaQueryResponse("F475F943-7067-ED11-A06B-7E5DFA4A16C9", 123, "Fulano", 0);

            mediatorMock.Send(contaQuery).ReturnsForAnyArgs(contaQueryResponseMock);

            var exception = await Assert.ThrowsAsync<RegraNegocioException>(async () => await movimentacaoHandler.Handle(movimentacaoCommand, CancellationToken.None));


            Assert.Equal(RegraNegocioErro.INACTIVE_ACCOUNT, exception.Erro);

            await movimentacaoRepository.Received(0).Salvar(Arg.Is<MovimentacaoEntity>(mov => mov.IdContaCorrente == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));
            await mediatorMock.Received(0).Publish(Arg.Is<IdempotenciaNotification>(notification => notification.IdRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C3"));

        }

        [Fact]
        public async Task Handle_DeveRetornarInvalidValue()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            var movimentacaoHandler = new MovimentacaoHandler(mediatorMock, movimentacaoRepository);
            var movimentacaoCommand = new MovimentacaoCommand
            {
                IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                IdentificacaoRequisicao = "B6BAFC09-6967-ED11-A567-055DFA4A16C3",
                TipoMovimentacao = "D",
                Valor = -1
            };

            var exception = await Assert.ThrowsAsync<RegraNegocioException>(async () => await movimentacaoHandler.Handle(movimentacaoCommand, CancellationToken.None));

            Assert.Equal(RegraNegocioErro.INVALID_VALUE, exception.Erro);

            await movimentacaoRepository.Received(0).Salvar(Arg.Is<MovimentacaoEntity>(mov => mov.IdContaCorrente == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));
            await mediatorMock.Received(0).Publish(Arg.Is<IdempotenciaNotification>(notification => notification.IdRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C3"));

        }

        [Fact]
        public async Task Handle_DeveRetornarInvalidType()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            var movimentacaoHandler = new MovimentacaoHandler(mediatorMock, movimentacaoRepository);
            var movimentacaoCommand = new MovimentacaoCommand
            {
                IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                IdentificacaoRequisicao = "B6BAFC09-6967-ED11-A567-055DFA4A16C3",
                TipoMovimentacao = "E",
                Valor = 5
            };

            var exception = await Assert.ThrowsAsync<RegraNegocioException>(async () => await movimentacaoHandler.Handle(movimentacaoCommand, CancellationToken.None));

            Assert.Equal(RegraNegocioErro.INVALID_TYPE, exception.Erro);

            await movimentacaoRepository.Received(0).Salvar(Arg.Is<MovimentacaoEntity>(mov => mov.IdContaCorrente == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));
            await mediatorMock.Received(0).Publish(Arg.Is<IdempotenciaNotification>(notification => notification.IdRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C3"));

        }
    }
}

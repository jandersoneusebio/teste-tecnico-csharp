using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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
using Questao5.Domain.ViewModels;
using Questao5.Infrastructure.Database.Repository;
using Questao5.Infrastructure.Services.Controllers;
using Xunit;

namespace Questao5.Tests
{
    public class MovimentacaoControllerTest
    {
        MovimentacaoResponse movimentacaoResponse = new MovimentacaoResponse { IdMovimento = "123" };

        MovimentacaoCommand movimentacaoCommand = new MovimentacaoCommand
        {
            IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
            IdentificacaoRequisicao = "B6BAFC09-6967-ED11-A567-055DFA4A16C2",
            TipoMovimentacao = "C",
            Valor = 5
        };

        [Fact]
        public async Task Handle_DeveSensibilizarComSucesso()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var loggerMock = Substitute.For<ILogger<MovimentacaoController>>();
            var movimentacaoController = new MovimentacaoController(loggerMock, mediatorMock);

            mediatorMock.Send(movimentacaoCommand).ReturnsForAnyArgs(movimentacaoResponse);

            var result = await movimentacaoController.Sensibilizar(movimentacaoCommand);

            await mediatorMock.Received(1).Send(Arg.Is<MovimentacaoCommand>(notification => notification.IdentificacaoRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C2"));

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, actionResult.StatusCode);

            var objectResult = Assert.IsType<MovimentacaoResponse>(actionResult.Value);
            
            Assert.Equal(movimentacaoResponse.IdMovimento, objectResult.IdMovimento);

        }

        [Fact]
        public async Task Handle_DeveRetornarBadRequest_InvalidAccount()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var loggerMock = Substitute.For<ILogger<MovimentacaoController>>();
            var movimentacaoController = new MovimentacaoController(loggerMock, mediatorMock);

            mediatorMock.Send(movimentacaoCommand).Throws(new RegraNegocioException(RegraNegocioErro.INVALID_ACCOUNT));

            var result = await movimentacaoController.Sensibilizar(movimentacaoCommand);

            await mediatorMock.Received(1).Send(Arg.Is<MovimentacaoCommand>(notification => notification.IdentificacaoRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C2"));

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);

            var objectResult = Assert.IsType<ErrorResponse>(actionResult.Value);

            Assert.Equal("INVALID_ACCOUNT", objectResult.Type);

        }

        [Fact]
        public async Task Handle_DeveRetornarBadRequest_InactiveAccount()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var loggerMock = Substitute.For<ILogger<MovimentacaoController>>();
            var movimentacaoController = new MovimentacaoController(loggerMock, mediatorMock);

            mediatorMock.Send(movimentacaoCommand).Throws(new RegraNegocioException(RegraNegocioErro.INACTIVE_ACCOUNT));

            var result = await movimentacaoController.Sensibilizar(movimentacaoCommand);

            await mediatorMock.Received(1).Send(Arg.Is<MovimentacaoCommand>(notification => notification.IdentificacaoRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C2"));

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);

            var objectResult = Assert.IsType<ErrorResponse>(actionResult.Value);

            Assert.Equal("INACTIVE_ACCOUNT", objectResult.Type);

        }

        [Fact]
        public async Task Handle_DeveRetornarBadRequest_InvalidValue()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var loggerMock = Substitute.For<ILogger<MovimentacaoController>>();
            var movimentacaoController = new MovimentacaoController(loggerMock, mediatorMock);

            mediatorMock.Send(movimentacaoCommand).Throws(new RegraNegocioException(RegraNegocioErro.INVALID_VALUE));

            var result = await movimentacaoController.Sensibilizar(movimentacaoCommand);

            await mediatorMock.Received(1).Send(Arg.Is<MovimentacaoCommand>(notification => notification.IdentificacaoRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C2"));

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);

            var objectResult = Assert.IsType<ErrorResponse>(actionResult.Value);

            Assert.Equal("INVALID_VALUE", objectResult.Type);

        }

        [Fact]
        public async Task Handle_DeveRetornarBadRequest_InvalidType()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var loggerMock = Substitute.For<ILogger<MovimentacaoController>>();
            var movimentacaoController = new MovimentacaoController(loggerMock, mediatorMock);

            mediatorMock.Send(movimentacaoCommand).Throws(new RegraNegocioException(RegraNegocioErro.INVALID_TYPE));

            var result = await movimentacaoController.Sensibilizar(movimentacaoCommand);

            await mediatorMock.Received(1).Send(Arg.Is<MovimentacaoCommand>(notification => notification.IdentificacaoRequisicao == "B6BAFC09-6967-ED11-A567-055DFA4A16C2"));

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);

            var objectResult = Assert.IsType<ErrorResponse>(actionResult.Value);

            Assert.Equal("INVALID_TYPE", objectResult.Type);

        }
    }
}

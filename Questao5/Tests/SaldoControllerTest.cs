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
    public class SaldoControllerTest
    {

        private static string DataHoraResponse = DateTime.Now.ToString();

        SaldoResponse movimentacaoResponse = new SaldoResponse 
        {
            Saldo = 3.5,
            DataHoraResponse = DataHoraResponse,
            NomeTitular = "Fulano",
            NumeroConta = 123
        };

        SaldoCommand saldoCommand = new SaldoCommand
        {
            IdentificacaoConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9"
        };

        private static string IdConta = "F475F943-7067-ED11-A06B-7E5DFA4A16C9";

        [Fact]
        public async Task Handle_DeveConsultarSaldoComSucesso()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var loggerMock = Substitute.For<ILogger<SaldoController>>();
            var movimentacaoController = new SaldoController(loggerMock, mediatorMock);

            mediatorMock.Send(saldoCommand).ReturnsForAnyArgs(movimentacaoResponse);

            var result = await movimentacaoController.ConsultarSaldo(IdConta);

            await mediatorMock.Received(1).Send(Arg.Is<SaldoCommand>(notification => notification.IdentificacaoConta == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, actionResult.StatusCode);

            var objectResult = Assert.IsType<SaldoResponse>(actionResult.Value);
            
            Assert.Equal(3.5, objectResult.Saldo);
            Assert.Equal(DataHoraResponse, objectResult.DataHoraResponse);
            Assert.Equal("Fulano", objectResult.NomeTitular);
            Assert.Equal(123, objectResult.NumeroConta);

        }
        
        [Fact]
        public async Task Handle_DeveRetornarBadRequest_InvalidAccount()
        {
            var mediatorMock = Substitute.For<IMediator>();
            var loggerMock = Substitute.For<ILogger<SaldoController>>();
            var movimentacaoController = new SaldoController(loggerMock, mediatorMock);

            mediatorMock.Send(saldoCommand).ThrowsForAnyArgs(new RegraNegocioException(RegraNegocioErro.INVALID_ACCOUNT));

            var result = await movimentacaoController.ConsultarSaldo(IdConta);

            await mediatorMock.Received(1).Send(Arg.Is<SaldoCommand>(notification => notification.IdentificacaoConta == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));

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
            var loggerMock = Substitute.For<ILogger<SaldoController>>();
            var movimentacaoController = new SaldoController(loggerMock, mediatorMock);

            mediatorMock.Send(saldoCommand).ThrowsForAnyArgs(new RegraNegocioException(RegraNegocioErro.INACTIVE_ACCOUNT));

            var result = await movimentacaoController.ConsultarSaldo(IdConta);

            await mediatorMock.Received(1).Send(Arg.Is<SaldoCommand>(notification => notification.IdentificacaoConta == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));

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
            var loggerMock = Substitute.For<ILogger<SaldoController>>();
            var movimentacaoController = new SaldoController(loggerMock, mediatorMock);

            mediatorMock.Send(saldoCommand).ThrowsForAnyArgs(new RegraNegocioException(RegraNegocioErro.INVALID_VALUE));

            var result = await movimentacaoController.ConsultarSaldo(IdConta);

            await mediatorMock.Received(1).Send(Arg.Is<SaldoCommand>(notification => notification.IdentificacaoConta == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));

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
            var loggerMock = Substitute.For<ILogger<SaldoController>>();
            var movimentacaoController = new SaldoController(loggerMock, mediatorMock);

            mediatorMock.Send(saldoCommand).ThrowsForAnyArgs(new RegraNegocioException(RegraNegocioErro.INVALID_TYPE));

            var result = await movimentacaoController.ConsultarSaldo(IdConta);

            await mediatorMock.Received(1).Send(Arg.Is<SaldoCommand>(notification => notification.IdentificacaoConta == "F475F943-7067-ED11-A06B-7E5DFA4A16C9"));

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);

            var objectResult = Assert.IsType<ErrorResponse>(actionResult.Value);

            Assert.Equal("INVALID_TYPE", objectResult.Type);

        }
        
    }
}

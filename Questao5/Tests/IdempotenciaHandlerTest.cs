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
    public class IdempotenciaHandlerTest
    {

        [Fact]
        public async Task Handle_DeveRetornarDuplicidade()
        {
            var idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();
            var idempotenciaHandler = new IdempotenciaHandler(idempotenciaRepository);

            var idempotenciaQuery = new IdempotenciaQuery
            {
                IdRequisicao = "A475F943-7067-ED11-A06B-7E5DFA4A16C0"
            };

            var idempotenciaEntityMock = new IdempotenciaEntity
            {
                Chave_Idempotencia = "A475F943-7067-ED11-A06B-7E5DFA4A16C0",
                Requisicao = "123",
                Resultado = " { \"idMovimento\": \"456\" } "
            };

            idempotenciaRepository.GetById(idempotenciaQuery.IdRequisicao).ReturnsForAnyArgs(idempotenciaEntityMock);

            var result = await idempotenciaHandler.Handle(idempotenciaQuery, CancellationToken.None);

            await idempotenciaRepository.Received(1).GetById(Arg.Is<string>(idem => idem == "A475F943-7067-ED11-A06B-7E5DFA4A16C0"));

            Assert.True(result.IsDuplicado);

        }

        [Fact]
        public async Task Handle_NaoDeveRetornarDuplicidade()
        {
            var idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();
            var idempotenciaHandler = new IdempotenciaHandler(idempotenciaRepository);

            var idempotenciaQuery = new IdempotenciaQuery
            {
                IdRequisicao = "A475F943-7067-ED11-A06B-7E5DFA4A16C0"
            };

            IdempotenciaEntity idempotenciaEntityMock = null;

            idempotenciaRepository.GetById(idempotenciaQuery.IdRequisicao).ReturnsForAnyArgs(idempotenciaEntityMock);

            var result = await idempotenciaHandler.Handle(idempotenciaQuery, CancellationToken.None);

            await idempotenciaRepository.Received(1).GetById(Arg.Is<string>(idem => idem == "A475F943-7067-ED11-A06B-7E5DFA4A16C0"));

            Assert.False(result.IsDuplicado);

        }

    }
}

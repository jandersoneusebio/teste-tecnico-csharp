using MediatR;
using Questao5.Application.Notifications;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Repository;

namespace Questao5.Application.Handlers.Events
{
    public class IdempotenciaEventHandler : INotificationHandler<IdempotenciaNotification>
    {
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        public IdempotenciaEventHandler(IIdempotenciaRepository idempotenciaRepository)
        {
            _idempotenciaRepository = idempotenciaRepository;
        }

        public Task Handle(IdempotenciaNotification notification, CancellationToken cancellationToken)
        {
            IdempotenciaEntity entity = new IdempotenciaEntity
            {
                Chave_Idempotencia = notification.IdRequisicao,
                Requisicao = notification.Requisicao,
                Resultado = notification.Resultado

            };

            _idempotenciaRepository.Salvar(entity);

            return Task.CompletedTask;
        }
    }
}

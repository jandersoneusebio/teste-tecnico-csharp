using MediatR;

namespace Questao5.Application.Notifications
{
    public class IdempotenciaNotification : INotification
    {
        public string IdRequisicao { get; set; }
        public string Requisicao { get; set; }
        public string Resultado { get; set; }

    }
}

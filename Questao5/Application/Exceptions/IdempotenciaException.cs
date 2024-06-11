using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;

namespace Questao5.Application.Exceptions
{
    public class IdempotenciaException : ApplicationException
    {
        public IdempotenciaException(MovimentacaoResponse response)
        {
            Response = response;
        }
        public MovimentacaoResponse Response { get ; set; }
    }
}

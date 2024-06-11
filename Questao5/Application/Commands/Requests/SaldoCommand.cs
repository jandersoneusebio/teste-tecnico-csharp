using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class SaldoCommand : IRequest<SaldoResponse>
    {
        public string IdentificacaoConta { get; set; }

    }
}

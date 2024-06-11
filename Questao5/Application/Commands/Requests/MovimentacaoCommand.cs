using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoCommand : IRequest<MovimentacaoResponse>
    {
        public MovimentacaoCommand()
        {

        }

        public string IdentificacaoRequisicao { get; set; }
        public string IdentificacaoConta { get; set; }
        public double Valor { get; set; }
        public string TipoMovimentacao { get; set; }

    }
}

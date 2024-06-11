using Questao5.Domain.Entities;

namespace Questao5.Application.Commands.Responses
{
    public class SaldoResponse
    {
        public int NumeroConta { get; set; }
        public string NomeTitular { get; set; }
        public string DataHoraResponse { get; set; }
        public double Saldo { get; set; }
    }
}

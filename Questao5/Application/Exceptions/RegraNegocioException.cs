using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;

namespace Questao5.Application.Exceptions
{
    public class RegraNegocioException : ApplicationException
    {
        public RegraNegocioException(RegraNegocioErro regraNegocioErro)
        {
            Erro = regraNegocioErro;
            Mensagem = Mensagens.Get(regraNegocioErro);
        }
        public RegraNegocioException(RegraNegocioErro regraNegocioErro, string mensagem)
        {
            Erro = regraNegocioErro;
            Mensagem = mensagem;
        }
        public string Mensagem {  get; set; }
        public RegraNegocioErro Erro { get ; set; }

        public override string ToString()
        {
            return $"Erro negocial: {Erro.ToString()} - {Mensagem}";
        }
    }
}

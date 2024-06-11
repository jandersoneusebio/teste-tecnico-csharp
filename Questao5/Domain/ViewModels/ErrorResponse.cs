using Questao5.Application.Exceptions;

namespace Questao5.Domain.ViewModels
{
    public class ErrorResponse
    {
        public ErrorResponse(string type, string message)
        {
            Type = type;
            Message = message;
        }

        public ErrorResponse(RegraNegocioException regraNegocioException)
        {
            Type = regraNegocioException.Erro.ToString();
            Message = regraNegocioException.Mensagem ;
        }

        public string Type { get; set; }
        public string Message { get; set; }

    }
}

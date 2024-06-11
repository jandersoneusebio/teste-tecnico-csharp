using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Language
{
    public class Mensagens
    {
        private static readonly Dictionary<RegraNegocioErro, string> mensagens = new Dictionary<RegraNegocioErro, string>
        {
            { RegraNegocioErro.INVALID_ACCOUNT,          "Apenas contas correntes cadastradas podem recebedor movimentação."},
            { RegraNegocioErro.INACTIVE_ACCOUNT,         "Apenas contas correntes ativas podem receber movimentação."},
            { RegraNegocioErro.INVALID_VALUE,            "Apenas valores positivos podem ser recebidos."},
            { RegraNegocioErro.INVALID_TYPE,             "Apenas os tipos 'débito' (D) ou 'crédito' (C) podem ser aceitos."},
            { RegraNegocioErro.INVALID_ACCOUNT_BALANCE,  "Apenas contas correntes cadastradas podem consultar o saldo."},
            { RegraNegocioErro.INACTIVE_ACCOUNT_BALANCE, "Apenas contas correntes ativas podem consultar o saldo"},
        };

        public static string Get(RegraNegocioErro erro)
        {
            return mensagens.TryGetValue(erro, out var mensagem) ? mensagem : "";
        }
    }
}

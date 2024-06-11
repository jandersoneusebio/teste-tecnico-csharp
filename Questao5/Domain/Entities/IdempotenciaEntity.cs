namespace Questao5.Domain.Entities
{
    public class IdempotenciaEntity
    {

        public IdempotenciaEntity()
        {
            
        }

        public IdempotenciaEntity(string chaveIdempotencia, string requisicao, string resultado)
        {
            Chave_Idempotencia = chaveIdempotencia;
            Requisicao = requisicao;
            Resultado = resultado;
        }

        public string Chave_Idempotencia { get; set; }
        public string Requisicao { get; set; }
        public string Resultado { get; set; }

    }
}

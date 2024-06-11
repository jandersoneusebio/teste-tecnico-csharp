namespace Questao5.Domain.Entities
{
    public class MovimentacaoEntity
    {
        public string IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public string DataMovimento { get; set; }
        public string TipoMovimento { get; set; }
        public double Valor { get; set; }

        public MovimentacaoEntity()
        {
            
        }

        public MovimentacaoEntity(string idMovimento, string idContaCorrente, string dataMovimento, string tipoMovimento, double valor)
        {
            IdMovimento = idMovimento;
            IdContaCorrente = idContaCorrente;
            DataMovimento = dataMovimento;
            TipoMovimento = tipoMovimento;
            Valor = valor;
        }
    }
}

using System.Globalization;

namespace Questao1
{
    class ContaBancaria {
        private static readonly double TAXA_SAQUE = 3.50;
        public int Numero { get; private set; }
        public string Titular { get; set; }
        public double Saldo { get; private set; }

        public ContaBancaria(int numero, string titular, double depositoInicial = 0)
        {
            Numero = numero;
            Titular = titular;
            Deposito(depositoInicial);
        }

        public void Deposito(double quantia)
        {
            Saldo = (Saldo + quantia);
        }

        public void Saque(double quantia)
        {
            // Não foi criado uma lógica para impedir saques de saldos já negativos dado que não foi
            // solicitado esta verificação na questão. Apenas a observação que o saldo pode ser negativo
            // ao realizar um saque / taxa
            Saldo = (Saldo - quantia) - TAXA_SAQUE;
        }

        override public string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo:F2}";
        }

    }
}

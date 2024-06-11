namespace Questao5.Application.Queries;

public class ContaQueryResponse
{

    public ContaQueryResponse(string idContaCorrente, int numero, string nome, int ativo)
    {
        IdContaCorrente = idContaCorrente;
        Numero = numero;
        Nome = nome;
        Ativo = ativo;
    }

    public string IdContaCorrente { get; set; }
    public int Numero { get; set; }
    public string Nome { get; set; }
    public int Ativo { get; set; }

}

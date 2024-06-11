using Newtonsoft.Json;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Queries;

public class IdempotenciaQueryResponse
{

    public IdempotenciaQueryResponse()
    {
        IsDuplicado = false;
    }

    public IdempotenciaQueryResponse(string response)
    {
        Response = JsonConvert.DeserializeObject<MovimentacaoResponse>(response);
        IsDuplicado = true;
    }

    public bool IsDuplicado { get; set; }
    public MovimentacaoResponse Response { get; set; }

}

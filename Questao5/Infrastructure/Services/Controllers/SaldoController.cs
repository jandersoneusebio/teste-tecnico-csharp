using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Exceptions;
using Questao5.Domain.Entities;
using Questao5.Domain.ViewModels;
using Questao5.Infrastructure.Database.Repository;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaldoController : ControllerBase
    {

        private readonly ILogger<MovimentacaoController> _logger;
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        private readonly IMediator _mediator;

        public SaldoController(
            ILogger<MovimentacaoController> logger,
            IMovimentacaoRepository movimentacaoRepository,
            IMediator mediator
        )
        {
            _logger = logger;
            _movimentacaoRepository = movimentacaoRepository;
            _mediator = mediator;
        }

        /// <summary>
        /// Consulta o saldo de uma conta corrente.
        /// </summary>
        /// <param name="idConta"></param>
        /// <returns>Os dados e o saldo da conta corrente consultada</returns>
        [HttpGet("consultar/{idConta}")]
        public async Task<IActionResult> Sensibilizar(string idConta)
        {
            try
            {
                var resultado = await _mediator.Send(new SaldoCommand { IdentificacaoConta = idConta });

                return Ok(resultado);
            }
            catch (RegraNegocioException rnex)
            {
                _logger.LogWarning(rnex.ToString());
                return StatusCode(400, new ErrorResponse(rnex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro no servidor");
                return StatusCode(500, "Ocorreu um erro no servidor");
            }

        }
    }
}
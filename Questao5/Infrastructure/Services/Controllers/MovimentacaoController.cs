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
    public class MovimentacaoController : ControllerBase
    {

        private readonly ILogger<MovimentacaoController> _logger;
        private readonly IMediator _mediator;

        public MovimentacaoController(
            ILogger<MovimentacaoController> logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Sensibiliza uma conta corrente com a operação de débito ou crédito.
        /// </summary>
        /// <param name="movimentacaoCommand"></param>
        /// <returns>O identificador da movimentação</returns>
        [HttpPost("sensibilizar")]
        public async Task<IActionResult> Sensibilizar(MovimentacaoCommand movimentacaoCommand)
        {
            try
            {
                var resultado = await _mediator.Send(movimentacaoCommand);

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
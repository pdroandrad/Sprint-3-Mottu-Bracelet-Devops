using Microsoft.AspNetCore.Mvc;
using MottuBracelet.Services;
using MottuBracelet.Model;
using MottuBracelet.DTO;

namespace MottuBracelet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoricoPatioController : ControllerBase
    {
        private readonly ServicoHistoricoPatios _servico;

        public HistoricoPatioController(ServicoHistoricoPatios servico)
        {
            _servico = servico;
        }

        [HttpGet]
        public async Task<ActionResult<List<HistoricoPatioHateoasDto>>> ObterTodos(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var historicos = await _servico.ObterPaginadoAsync(pageNumber, pageSize);
            var total = await _servico.ContarAsync();
            Response.Headers.Add("X-Total-Count", total.ToString());

            var urlBase = $"{Request.Scheme}://{Request.Host}/api";
            var historicosDto = historicos.Select(h => _servico.MontarHistoricoComLinks(h, urlBase)).ToList();

            return Ok(historicosDto);
        }

        [HttpGet("{id:int}", Name = "ObterHistoricoPatio")]
        public async Task<ActionResult<HistoricoPatioHateoasDto>> ObterPorId(int id)
        {
            var historico = await _servico.ObterPorIdAsync(id);
            if (historico == null) return NotFound();

            var urlBase = $"{Request.Scheme}://{Request.Host}/api";
            var historicoHateoas = _servico.MontarHistoricoComLinks(historico, urlBase);

            return Ok(historicoHateoas);
        }

        [HttpPost]
        public async Task<ActionResult<HistoricoPatioHateoasDto>> Criar(HistoricoPatio historico)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _servico.CriarAsync(historico);

            var urlBase = $"{Request.Scheme}://{Request.Host}/api";
            var historicoHateoas = _servico.MontarHistoricoComLinks(historico, urlBase);

            return CreatedAtRoute("ObterHistoricoPatio", new { id = historico.Id }, historicoHateoas);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, HistoricoPatio historicoAtualizado)
        {
            var atualizado = await _servico.AtualizarAsync(id, historicoAtualizado);
            return atualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Remover(int id)
        {
            var removido = await _servico.RemoverAsync(id);
            return removido ? NoContent() : NotFound();
        }
    }
}

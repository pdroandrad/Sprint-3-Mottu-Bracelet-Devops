using Microsoft.AspNetCore.Mvc;
using MottuBracelet.Services;
using MottuBracelet.Model;
using MottuBracelet.DTO;

namespace MottuBracelet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatioController : ControllerBase
    {
        private readonly ServicoPatios _servico;

        public PatioController(ServicoPatios servico)
        {
            _servico = servico;
        }

        [HttpGet]
        public async Task<ActionResult<List<PatioHateoasDto>>> ObterTodos(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var patios = await _servico.ObterPaginadoAsync(pageNumber, pageSize);
            var total = await _servico.ContarAsync();
            Response.Headers.Add("X-Total-Count", total.ToString());

            var urlBase = $"{Request.Scheme}://{Request.Host}/api";
            var patiosDto = patios.Select(p => _servico.MontarPatioComLinks(p, urlBase)).ToList();

            return Ok(patiosDto);
        }

        [HttpGet("{id:int}", Name = "ObterPatio")]
        public async Task<ActionResult<PatioHateoasDto>> ObterPorId(int id)
        {
            var patio = await _servico.ObterPorIdAsync(id);
            if (patio == null) return NotFound();

            var urlBase = $"{Request.Scheme}://{Request.Host}/api";
            var patioHateoas = _servico.MontarPatioComLinks(patio, urlBase);
            return Ok(patioHateoas);
        }

        [HttpPost]
        public async Task<ActionResult<PatioHateoasDto>> Criar(Patio patio)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _servico.CriarAsync(patio);

            var urlBase = $"{Request.Scheme}://{Request.Host}/api";
            var patioHateoas = _servico.MontarPatioComLinks(patio, urlBase);

            return CreatedAtRoute("ObterPatio", new { id = patio.Id }, patioHateoas);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, Patio patioAtualizado)
        {
            var atualizado = await _servico.AtualizarAsync(id, patioAtualizado);
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

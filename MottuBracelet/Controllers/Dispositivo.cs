using Microsoft.AspNetCore.Mvc;
using MottuBracelet.Services;
using MottuBracelet.Model;
using MottuBracelet.DTO;

namespace MottuBracelet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DispositivoController : ControllerBase
    {
        private readonly ServicoDispositivos _servico;

        public DispositivoController(ServicoDispositivos servico)
        {
            _servico = servico;
        }

        [HttpGet]
        public async Task<ActionResult<List<DispositivoHateoasDto>>> ObterTodos(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var dispositivos = await _servico.ObterPaginadoAsync(pageNumber, pageSize);
            var total = await _servico.ContarAsync();
            Response.Headers.Add("X-Total-Count", total.ToString());

            var urlBase = $"{Request.Scheme}://{Request.Host}/api";
            var dispositivosDto = dispositivos.Select(d => _servico.MontarDispositivoComLinks(d, urlBase)).ToList();

            return Ok(dispositivosDto);
        }

        [HttpGet("{id:int}", Name = "ObterDispositivo")]
        public async Task<ActionResult<DispositivoHateoasDto>> ObterPorId(int id)
        {
            var dispositivo = await _servico.ObterPorIdAsync(id);
            if (dispositivo == null) return NotFound();

            var urlBase = $"{Request.Scheme}://{Request.Host}/api";
            var dispositivoHateoas = _servico.MontarDispositivoComLinks(dispositivo, urlBase);

            return Ok(dispositivoHateoas);
        }

        [HttpPost]
        public async Task<ActionResult<DispositivoHateoasDto>> Criar(Dispositivo dispositivo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _servico.CriarAsync(dispositivo);

            var urlBase = $"{Request.Scheme}://{Request.Host}/api";
            var dispositivoHateoas = _servico.MontarDispositivoComLinks(dispositivo, urlBase);

            return CreatedAtRoute("ObterDispositivo", new { id = dispositivo.Id }, dispositivoHateoas);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, Dispositivo dispositivoAtualizado)
        {
            var atualizado = await _servico.AtualizarAsync(id, dispositivoAtualizado);
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

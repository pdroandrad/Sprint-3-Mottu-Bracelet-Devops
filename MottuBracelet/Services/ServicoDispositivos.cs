using Microsoft.EntityFrameworkCore;
using MottuBracelet.Data;
using MottuBracelet.DTO;
using MottuBracelet.Model;

namespace MottuBracelet.Services
{
    public class ServicoDispositivos
    {
        private readonly AppDbContext _context;

        public ServicoDispositivos(AppDbContext context)
        {
            _context = context;
        }

        // Obter lista paginada de dispositivos
        public async Task<List<Dispositivo>> ObterPaginadoAsync(int pageNumber, int pageSize)
        {
            return await _context.Dispositivos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Contar total de dispositivos
        public async Task<int> ContarAsync()
        {
            return await _context.Dispositivos.CountAsync();
        }

        // Obter dispositivo por ID
        public async Task<Dispositivo?> ObterPorIdAsync(int id)
        {
            return await _context.Dispositivos.FindAsync(id);
        }

        // Criar novo dispositivo
        public async Task<Dispositivo> CriarAsync(Dispositivo dispositivo)
        {
            _context.Dispositivos.Add(dispositivo);
            await _context.SaveChangesAsync();
            return dispositivo;
        }

        // Atualizar dispositivo existente
        public async Task<bool> AtualizarAsync(int id, Dispositivo dispositivoAtualizado)
        {
            var existe = await _context.Dispositivos.AnyAsync(d => d.Id == id);
            if (!existe) return false;

            dispositivoAtualizado.Id = id;
            _context.Entry(dispositivoAtualizado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        // Remover dispositivo
        public async Task<bool> RemoverAsync(int id)
        {
            var dispositivo = await _context.Dispositivos.FindAsync(id);
            if (dispositivo == null) return false;

            _context.Dispositivos.Remove(dispositivo);
            await _context.SaveChangesAsync();
            return true;
        }

        // Montar objeto Hateoas
        public DispositivoHateoasDto MontarDispositivoComLinks(Dispositivo dispositivo, string urlBase)
        {
            return new DispositivoHateoasDto
            {
                Id = dispositivo.Id,
                StatusDispositivo = dispositivo.StatusDispositivo ?? string.Empty,
                MotoId = dispositivo.MotoId,
                PatioId = dispositivo.PatioId,
                Links = new List<LinkDto>
                {
                    new LinkDto { Href = $"{urlBase}/dispositivo/{dispositivo.Id}", Rel = "self", Method = "GET" },
                    new LinkDto { Href = $"{urlBase}/dispositivo/{dispositivo.Id}", Rel = "update", Method = "PUT" },
                    new LinkDto { Href = $"{urlBase}/dispositivo/{dispositivo.Id}", Rel = "delete", Method = "DELETE" }
                }
            };
        }
    }
}

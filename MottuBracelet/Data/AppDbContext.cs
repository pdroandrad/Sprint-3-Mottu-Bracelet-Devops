using Microsoft.EntityFrameworkCore;
using MottuBracelet.Model;

namespace MottuBracelet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Patio> Patios { get; set; }
        public DbSet<Dispositivo> Dispositivos { get; set; }
        public DbSet<Moto> Motos { get; set; }
        public DbSet<HistoricoPatio> HistoricoPatios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapear nomes das tabelas no SQL Server
            modelBuilder.Entity<Moto>().ToTable("MOTO_NET");
            modelBuilder.Entity<Patio>().ToTable("PATIO_NET");
            modelBuilder.Entity<Dispositivo>().ToTable("DISPOSITIVO_NET");
            modelBuilder.Entity<HistoricoPatio>().ToTable("HISTORICOPATIO_NET");

            // Relação 1:1 entre Moto e Dispositivo
            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Dispositivo)
                .WithOne(d => d.Moto)
                .HasForeignKey<Dispositivo>(d => d.MotoId)
                .OnDelete(DeleteBehavior.SetNull);

            // Endereço é um tipo próprio dentro de Patio
            modelBuilder.Entity<Patio>()
                .OwnsOne(p => p.Endereco);
        }
    }
}

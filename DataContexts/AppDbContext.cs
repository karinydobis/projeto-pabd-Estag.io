using ApiServico.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiServico.DataContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Estudante> Estudantes { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Vaga> Vagas { get; set; }
        public DbSet<Candidatura> Candidaturas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacionamento Estudante <-> Vaga (N:N) via Candidatura
            modelBuilder.Entity<Candidatura>()
                .HasOne(c => c.Estudante)
                .WithMany(e => e.Candidaturas)
                .HasForeignKey(c => c.EstudanteId);

            modelBuilder.Entity<Candidatura>()
                .HasOne(c => c.Vaga)
                .WithMany(v => v.Candidaturas)
                .HasForeignKey(c => c.VagaId);
        }
    }
}

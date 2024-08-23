using Microsoft.EntityFrameworkCore;
using PolyglotPersistence.Domain;

namespace Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Prontuario> Prontuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paciente>()
                .HasOne(paciente => paciente.Prontuario)
                .WithOne()
                .HasForeignKey<Prontuario>(prontuario => prontuario.PacienteId);
        }
    }
}
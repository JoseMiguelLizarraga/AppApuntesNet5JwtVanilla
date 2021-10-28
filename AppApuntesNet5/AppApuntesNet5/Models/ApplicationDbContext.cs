using AppApuntesNet5.Dto;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace AppApuntesNet5.Models
{
    public class ApplicationDbContext : IdentityDbContext<UsuarioAutenticado>  // Asi es para usar Json Web Tokens
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Referencia a las clases de dominio

        public DbSet<ApuntesCategoria> ApuntesCategoria { get; set; }
        public DbSet<ApuntesDetalleTema> ApuntesDetalleTema { get; set; }
        public DbSet<ApuntesTema> ApuntesTema { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioCargo> UsuarioCargo { get; set; }

    }
}



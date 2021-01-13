//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AppApuntesNet5.Dto;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
Esto fue creado como una clase comun y corriente.
Para este ejemplo es usada una base de datos en memoria. Debe configurarse en el archivo
Startup.cs en el metodo ConfigureServices.
*/



namespace AppApuntesNet5.Models
{
    //public class ApplicationDbContext: DbContext     ...Asi es normalmente

    public class ApplicationDbContext: IdentityDbContext<UsuarioAutenticado>      // Asi es para usar Json Web Tokens
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Aca se pone la referencia a las clases entidades del proyecto

        public DbSet<ApuntesCategoria> ApuntesCategoria { get; set; }
        public DbSet<ApuntesDetalleTema> ApuntesDetalleTema { get; set; }
        public DbSet<ApuntesTema> ApuntesTema { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioCargo> UsuarioCargo { get; set; }

        //public virtual DbSet<Pais> Pais { get; set; }

        //public virtual DbSet<Provincia> Provincia { get; set; }


    }
}



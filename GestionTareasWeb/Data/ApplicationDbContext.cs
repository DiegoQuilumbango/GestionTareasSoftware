using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modelo.Software;

namespace GestionTareasWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Modelo.Software.Tarea> Tarea { get; set; } = default!;
        public DbSet<Modelo.Software.Proyecto> Proyecto { get; set; } = default!;
        public DbSet<Modelo.Software.Usuario> Usuario { get; set; } = default!;
    }
}

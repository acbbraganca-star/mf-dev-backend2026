using Microsoft.EntityFrameworkCore;
using mf_dev_backend2026.Models;

namespace mf_dev_backend2026.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public  DbSet<Veículo> Veiculos { get; set; }

        public DbSet<Consumo> Consumos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

    }

}



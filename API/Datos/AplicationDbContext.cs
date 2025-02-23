using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Datos
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Producto { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using SirpoLR4.Models;
using System.Collections.Generic;

namespace SirpoLR4.DataAccess
{
    public class CharterDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public CharterDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Charter> Charters => Set<Charter>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Database"));
        }
    }
}

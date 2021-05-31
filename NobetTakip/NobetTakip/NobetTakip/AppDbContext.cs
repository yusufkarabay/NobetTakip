using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NobetTakip.Core.Models;

namespace NobetTakip
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Personel>()
                .HasIndex(b => b.MailAddress)
                .IsUnique();

            modelBuilder.Entity<Isletme>()
               .HasIndex(u => u.IsletmeKod)
               .IsUnique();
        }

        public DbSet<Personel> Personels { get; set; }
        public DbSet<Nobet> Nobets { get; set; }
        public DbSet<Isletme> Isletmeler { get; set; }
    }
}

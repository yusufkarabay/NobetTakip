using Microsoft.EntityFrameworkCore;
using NobetTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            /*modelBuilder
                .Entity<Nobet>()
                .Property(n => n.Nobetciler)
                .HasConversion(
                    v => string.Join(',', v.Nobetciler.ToList().Select(m => m.NobetId)),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));*/

            // personel sınıfı için mail adresi bilgisinin eşsiz olmasını sağla
            // kayıt ekranında kullanıcının girdiği şifreleri kontrol et ve ona göre kayıt yap.

            modelBuilder.Entity<Nobet>()
                .HasMany(b => b.Nobetciler)
                .WithOne();

            modelBuilder.Entity<Isletme>()
               .HasIndex(u => u.IsletmeKod)
               .IsUnique();
        }

        public DbSet<Personel> Personels { get; set; }
        public DbSet<Nobet> Nobets { get; set; }
    }
}

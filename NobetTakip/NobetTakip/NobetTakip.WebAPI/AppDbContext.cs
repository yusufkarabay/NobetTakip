using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NobetTakip.Core.Models;

namespace NobetTakip.WebAPI
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

            modelBuilder.Entity<Personel>()
                .HasKey(p => new { p.PersonelId });

            modelBuilder.Entity<Personel>()
               .HasIndex(p => p.MailAddress)
               .IsUnique();

            modelBuilder.Entity<Isletme>()
                .HasKey(i => new { i.IsletmeId });

            modelBuilder.Entity<Isletme>()
               .HasIndex(u => u.IsletmeKod)
               .IsUnique();

            modelBuilder.Entity<Nobet>()
                .HasKey(n => new { n.NobetId });

        }

        public DbSet<Personel> Personels { get; set; }
        public DbSet<Nobet> Nobets { get; set; }
        public DbSet<Isletme> Isletmeler { get; set; }
        public DbSet<Bildirim> Bildirimler { get; set; }
        public DbSet<Degisim> Degisimler { get; set; }
    }
}

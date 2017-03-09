using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CQRSCode.WriteModel.Referential
{
    public class RefDBContext : DbContext
    {
        String _connectionString;

        public RefDBContext(String connectionString)
        {
            _connectionString = connectionString;
            Database.Migrate();
        }

        public DbSet<CategoryId> CategoriesIds { get; set; }
        public DbSet<ProductId> ProductsIds { get; set; }
        public DbSet<OfferId> OffersIds { get; set; }
        public DbSet<MerchantId> MerchantsIds { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CategoryId>()
                .HasKey(c => new { c.Id });
            
            builder.Entity<ProductId>()
                .HasKey(p => new { p.Id });
            builder.Entity<ProductId>()
                .HasIndex(p => new { p.EAN });
            builder.Entity<ProductId>()
                .HasIndex(p => new { p.ISBN });

            builder.Entity<OfferId>()
                .HasKey(o => new { o.Id });
            builder.Entity<OfferId>()
                .HasIndex(o => new { o.MerchandId, o.ProductId });
            builder.Entity<OfferId>()
                .HasIndex(o => new { o.MerchandId, o.SKU });

            builder.Entity<MerchantId>()
                .HasKey(m => new { m.Id });
            builder.Entity<MerchantId>()
                .HasIndex(m => new { m.Email });
            builder.Entity<MerchantId>()
                .HasIndex(m => new { m.Name });

            base.OnModelCreating(builder);
        }
    }
}

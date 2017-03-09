using System;
using Microsoft.EntityFrameworkCore;

namespace CQRSCode.WriteModel.EventStore.Postgre
{
    public class EventsDBContext : DbContext
    {
        String _connectionString;

        public EventsDBContext(String connectionString)
        {
            _connectionString = connectionString;
            Database.Migrate();
        }

        public DbSet<EventModel> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EventModel>()
                .HasKey(em => new { em.ID });
            
            builder.Entity<EventModel>()
                .HasIndex(em => new { em.AggregateId, em.SequenceNumber })
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}

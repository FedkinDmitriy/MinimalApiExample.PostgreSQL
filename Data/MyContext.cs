using Microsoft.EntityFrameworkCore;
using MinimalApiExample.PostgreSQL.Data.Models;

namespace MinimalApiExample.PostgreSQL.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users"); // Явное указание таблицы
                // Соответствие столбцам
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.firstName).HasColumnName("firstName");
                entity.Property(p => p.lastName).HasColumnName("lastName");
                entity.Property(p => p.dateOfBirth).HasColumnName("dateOfBirth");
            });
        }
    }
}
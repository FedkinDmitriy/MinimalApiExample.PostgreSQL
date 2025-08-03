using Microsoft.EntityFrameworkCore;
using MinimalApiExample.PostgreSQL.Data.Models;

namespace MinimalApiExample.PostgreSQL.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        internal DbSet<User> Users { get; set; }
        internal DbSet<Blog> Blogs { get; set; }

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

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("blogs");
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.Title).HasColumnName("title");
                entity.Property(p => p.CreatedDate).HasColumnName("created");
                entity.Property(p => p.Context).HasColumnName("context");
                entity.Property(p => p.UserId).HasColumnName("user_id");


                entity.HasOne( b => b.User).WithMany(u => u.Blogs).HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
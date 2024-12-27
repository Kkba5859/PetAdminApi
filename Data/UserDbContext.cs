using Microsoft.EntityFrameworkCore;
using PetAdminApi.Models;

namespace PetAdminApi.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        // Определение DbSet для работы с сущностями пользователей
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация сущностей
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            // Уникальность имени пользователя
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Другие настройки (если потребуется)
        }
    }
}

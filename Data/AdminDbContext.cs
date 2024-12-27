using Microsoft.EntityFrameworkCore;
using PetAdminApi.Models;

namespace PetAdminApi.Data
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
        {
        }

        // Определение DbSet для работы с сущностями администраторов
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация сущностей
            modelBuilder.Entity<Admin>().HasKey(a => a.Id);

            // Уникальность имени администратора
            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Username)
                .IsUnique();

            // Другие настройки (если потребуется)
        }
    }
}

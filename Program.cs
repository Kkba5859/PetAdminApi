using Microsoft.EntityFrameworkCore;
using PetAdminApi.Data;

namespace PetAdminApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Инициализация SQLite
            SQLitePCL.Batteries.Init();

            var builder = WebApplication.CreateBuilder(args);

            // Добавление сервисов в контейнер
            builder.Services.AddControllers();

            // Добавление DbContext для Admin и User баз данных
            builder.Services.AddDbContext<AdminDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("AdminDbConnection")));

            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("UserDbConnection")));

            // Конфигурация аутентификации JWT
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = builder.Configuration["Jwt:Authority"];
                    options.Audience = builder.Configuration["Jwt:Audience"];
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                });

            // Настройка CORS (разрешаем запросы с клиента)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorApp", policy =>
                {
                    policy.WithOrigins("http://localhost:5001")  // Указываем адрес вашего Blazor-приложения
                          .AllowAnyHeader()                    // Разрешаем любые заголовки
                          .AllowAnyMethod();                    // Разрешаем любые HTTP методы
                });
            });

            // Swagger/OpenAPI документация
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Включение CORS
            app.UseCors("AllowBlazorApp");

            // Конфигурация HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Включение аутентификации и авторизации
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

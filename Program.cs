using Microsoft.EntityFrameworkCore;
using PetAdminApi.Data;

namespace PetAdminApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ������������� SQLite
            SQLitePCL.Batteries.Init();

            var builder = WebApplication.CreateBuilder(args);

            // ���������� �������� � ���������
            builder.Services.AddControllers();

            // ���������� DbContext ��� Admin � User ��� ������
            builder.Services.AddDbContext<AdminDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("AdminDbConnection")));

            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("UserDbConnection")));

            // ������������ �������������� JWT
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = builder.Configuration["Jwt:Authority"];
                    options.Audience = builder.Configuration["Jwt:Audience"];
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                });

            // ��������� CORS (��������� ������� � �������)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorApp", policy =>
                {
                    policy.WithOrigins("http://localhost:5001")  // ��������� ����� ������ Blazor-����������
                          .AllowAnyHeader()                    // ��������� ����� ���������
                          .AllowAnyMethod();                    // ��������� ����� HTTP ������
                });
            });

            // Swagger/OpenAPI ������������
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // ��������� CORS
            app.UseCors("AllowBlazorApp");

            // ������������ HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // ��������� �������������� � �����������
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

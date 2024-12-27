using Microsoft.EntityFrameworkCore;
using PetAdminApi.Data;

namespace PetAdminApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SQLitePCL.Batteries.Init();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Add DbContexts with connection strings from configuration
            builder.Services.AddDbContext<AdminDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("AdminDbConnection")));

            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("UserDbConnection")));

            // Configure JWT Authentication
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = builder.Configuration["Jwt:Authority"];
                    options.Audience = builder.Configuration["Jwt:Audience"];
                    options.RequireHttpsMetadata = false;
                });

            


            // Swagger/OpenAPI documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Enable Authentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

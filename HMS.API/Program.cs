
using HMS.API.Registrations;
using HMS.BL.Extensions;
using HMS.BL.Helpers;
using HMS.Core.Models;
using HMS.DAL.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace HMS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionHandler>();
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer
                (builder.Configuration.GetConnectionString("local")));

            builder.Services.AddIdentity<User, IdentityRole>(x =>
            {
                x.Password.RequireUppercase = false;
                x.Password.RequireLowercase = false;
                x.Password.RequiredLength = 3;
                x.Password.RequireDigit = false;
                x.Password.RequireNonAlphanumeric = false;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddApplicationServices();
            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            //app.UseUserSeed(); //SeedData
            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

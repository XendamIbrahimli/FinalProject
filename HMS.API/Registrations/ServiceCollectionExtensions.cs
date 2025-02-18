using HMS.BL.Helpers;
using HMS.BL.Services;
using HMS.Core.Repositories;
using HMS.Core.Services;
using HMS.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HMS.API.Registrations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
        {


            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPatienceRepository, PatienceRepository>();

            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAppointmentService, AppointmentService>();

            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            services.Configure<JwtOptions>(config.GetSection(JwtOptions.Jwt));

            JwtOptions jwtOptions = new JwtOptions();
            jwtOptions.Audience = config.GetSection("JwtOptions")["Audience"]!;
            jwtOptions.Issuer = config.GetSection("JwtOptions")["Issuer"]!;
            jwtOptions.Secret = config.GetSection("JwtOptions")["Secret"]!;

            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = secKey,
                    ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }
    }
}

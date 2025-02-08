using HMS.BL.Services;
using HMS.Core.Repositories;
using HMS.Core.Services;
using HMS.DAL.Repositories;

namespace HMS.API.Registrations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPatienceRepository, PatienceRepository>();

            services.AddScoped<IDepartmentService, DepartmentService>();
            return services;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.healper;
using API.interfaces;
using API.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class AppicationServicesExtensions
    {
        public static IServiceCollection AddAppicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<IPhotoServices, PhotoServices>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<LogUserActivity>();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);  
            services.AddDbContext<DataContext>(options =>{
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));});
            return services;
        }
    }
}
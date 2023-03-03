using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.entities;
using API.healper;
using API.interfaces;
using API.services;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class AppicationServicesExtensions
    {
        public static IServiceCollection AddAppicationServices(this IServiceCollection services, IConfiguration config)
        {



            services.AddSingleton<PresenceTracker>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenServices, TokenServices>();

            services.AddScoped<IunitOfWork,UnitOfWork>();
            services.AddScoped<IPhotoServices, PhotoServices>();
            services.AddScoped<LogUserActivity>();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
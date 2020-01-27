using GreekTheater.API.Core.Entities;
using GreekTheater.API.Core.Repositories;
using GreekTheater.API.Core.Services;
using GreekTheater.API.Core.Services.Data_Shaping;
using GreekTheater.API.Core.Services.Pagination;
using GreekTheater.API.Core.Services.Sorting;
using GreekTheater.API.Persistence.Repositories;
using GreekTheater.API.Persistence.Services;
using GreekTheater.API.Persistence.Services.Data_Shaping;
using GreekTheater.API.Persistence.Services.Pagination;
using GreekTheater.API.Persistence.Services.Sorting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.Extension_Methods
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(serviceProvider =>
            {
                var actionContext = serviceProvider.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = serviceProvider.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            services.AddScoped<IPaginationService<Performance>, PerformancePaginationService>();
            services.AddScoped<IRootPaginationService, RootPaginationService>();
            services.AddScoped<IDependentPaginationService<Performance>, PerformancePaginationService>();
            services.AddScoped<IDataShapingService<Performance>, PerformanceShapingService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IPerformanceRepository, PerformanceRepository>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

            return services;
        }
    }
}

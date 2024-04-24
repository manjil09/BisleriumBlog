using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Infrastructure.Data;
using BisleriumBlog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BisleriumBlog.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //use connection string from appsettings.json
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AppConnectionString")));

            services.AddTransient<IBlogRepository, BlogRepository>();

            return services;
        }
    }
}

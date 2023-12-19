using System.CodeDom;
using System.Configuration;
using Persistence_Layer_Common.DB;
using Persistence_Layer_Common.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence_Layer.Extensions{


    public static class Extensions {

        public static IServiceCollection AddSafqaPostgres(this IServiceCollection services,IConfiguration configuration){
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            
            return services;
        }

        public static IServiceCollection AddRepository(this IServiceCollection services){
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            return services;
        }

        
    }

}
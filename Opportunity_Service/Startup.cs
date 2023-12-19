using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using Persistence_Layer_Common.DB;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opportunity_Service.Consumers;
using Persistence_Layer.Extensions;

namespace Opportunity_Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMassTransit((configurator) => {
                configurator.AddRequestClient<CheckUserFunds>(new Uri("exchange:user_funds"));
                configurator.UsingRabbitMq((ctx,cfg)=>{
                    cfg.ConfigureEndpoints(ctx);
                });
            });

            services.AddSafqaPostgres(Configuration);
            services.AddRepository();
            services.AddControllers();
            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI();

        }
    }
}

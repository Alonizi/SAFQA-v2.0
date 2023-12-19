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
using Wallet_Service.Consumers;

namespace Wallet_Service
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
            services.AddMassTransit(m=> {
                m.AddConsumer<NewOpportunityCreatedConsumer>()
                    .Endpoint(e=>e.Name="queue:New_Opportunity_To_Wallet_Event");
                m.AddConsumer<NewUserCreatedConsumer>()
                    .Endpoint(e=>e.Name = "queue:New_User_To_Wallet_Event");
                m.AddConsumer<CheckUserFundsConsumer>()
                    .Endpoint(e=>e.Name = "user_funds" );

                m.UsingRabbitMq((ctx,cfg)=>
                {
                    cfg.ConfigureEndpoints(ctx);
                    });
            });
            
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence_Layer.Extensions;
using SMS_Service.Consumers;

namespace SMS_Service
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
                configurator.AddConsumer<OpportunityCreatedConsumer>()
                    .Endpoint(e=>e.Name="queue:New_Opportunity_To_SMS_Event");
                configurator.AddConsumer<InvesmentCreatedConsumer>()
                    .Endpoint(e=>e.Name = "queue:New_Invesment_To_SMS_Event");
                configurator.AddConsumer<NewUserCreatedConsumer>()
                    .Endpoint(e=>e.Name = "queue:New_User_To_SMS_Event");
                configurator.AddConsumer<NewUserCreatedConsumer>();

                configurator.UsingRabbitMq((ctx,cfg)=>{
                    cfg.ConfigureEndpoints(ctx);
                });
            });            
            
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

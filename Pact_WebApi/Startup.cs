using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;
using Pact_WebApi.DTOs;
using Pact_WebApi.Entities;
using Pact_WebApi.Middleware;
using Pact_WebApi.Repositories;
using Pact_WebApi.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Pact_WebApi
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
            services.AddDbContext<PactDbContext>(builder =>
            {
                builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISeedDataService, SeedDataService>();

            

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Packt API",
                    Version = "v1"
                });
                options.SwaggerDoc("v2", new Info
                {
                    Title = "Packt API",
                    Version = "v2"
                });
            });

            services.AddMvc(config =>
            {
                config.ReturnHttpNotAcceptable = true;
                
                config.InputFormatters.Add(new XmlSerializerInputFormatter(config));
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();

            //}
            
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/plain";
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorFeature != null)
                    {
                        var logger = loggerFactory.CreateLogger("Global Exception Logger");
                        logger.LogError(500, errorFeature.Error, errorFeature.Error.Message);
                    }

                    await context.Response.WriteAsync("An error has occurred. Please try again...");
                });
            });

            env.ConfigureNLog("nlog.config");
            loggerFactory.AddNLog();

            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<Customer, CustomerDTO>().ReverseMap();
                config.CreateMap<Customer, CreateCustomerDTO>().ReverseMap();
                config.CreateMap<Customer, UpdateCustomerDTO>().ReverseMap();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Packt API v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Packt API v2");
            });

            app.UseHttpsRedirection();
            app.AddSeedData();
            app.UseMvc();
        }
    }
}

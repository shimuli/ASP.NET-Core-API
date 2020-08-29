using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkApi.Data;
using ParkApi.Mappers;
using ParkApi.Repo;
using ParkApi.Repo.IRepo;

namespace ParkApi
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
            services.AddDbContext<AplicationDbContent>(options => options.UseSqlServer
            (Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(ParkMapper));
            services.AddScoped<INationalParkRepo, NationalParkRepo>();
            services.AddScoped<ITrailRepo, TrailRepo>();
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ParkOpenApiSpec", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title="Kenyanparks",
                    Version= "1",
                    Description =" National Park Open Api",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "shimulicedric@gmail.com",
                        Name = "Shimuli Cedric",
                        Url = new Uri("https://github.com/shimuli/ASP.NET-Core-API")
                    },

                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT Licence",
                        Url = new Uri("https://github.com/shimuli/ASP.NET-Core-API")
                    }
                });

              /*  options.SwaggerDoc("ParkOpenApiSpecTrails", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Trails Api",
                    Version = "1",
                    Description = " Trails",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "shimulicedric@gmail.com",
                        Name = "Shimuli Cedric",
                        Url = new Uri("https://github.com/shimuli/ASP.NET-Core-API")
                    },

                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT Licence",
                        Url = new Uri("https://github.com/shimuli/ASP.NET-Core-API")
                    }
                });
              */
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(cmlCommentsFullPath);
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ParkOpenApiSpec/swagger.json", "Parks Api National Parks");
               // options.SwaggerEndpoint("/swagger/ParkOpenApiSpecTrails/swagger.json", "Parks Api Trails");
                options.RoutePrefix = "";
            });
           
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

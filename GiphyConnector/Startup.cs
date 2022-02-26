using GiphyAdapterLib;
using GiphyAdapterLib.Caching;
using GiphyConnectorService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace GiphyConnectorService
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
            services.AddControllers();

            services.AddSingleton<IGifService, GifService>();
            services.AddSingleton<IGiphyAdapter, GiphyAdapter>();
            services.AddSingleton<IGiphyResponseCaching, GiphyResponseCaching>();

            //if (IsDevelopment())
            services.AddDistributedMemoryCache();
            //else
            //{
            //configure real distributed cache
            //}

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GifContentService API", Version = "v1" });
            });

            services.AddHttpClient<IGiphyAdapter, GiphyAdapter>(client =>
            {
                client.BaseAddress = new Uri("https://api.giphy.com");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GifContentService API V1");
            });
        }
    }
}

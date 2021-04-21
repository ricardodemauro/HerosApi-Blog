using HerosApi.Configuration;
using HerosApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace HerosApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<DatabaseConfiguration>(
                _configuration.GetSection("DatabaseSettings"));

            services.AddSingleton<IDatabaseConfiguration>(sp =>
                sp.GetRequiredService<IOptions<DatabaseConfiguration>>()?.Value);

            services.AddTransient<HeroService>();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                endpoints.MapSwagger();

                var pipeline = endpoints
                        .CreateApplicationBuilder()
                        .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Heros Api"))
                        .Build();

                endpoints.Map(pattern: "/swagger/*", pipeline);

                endpoints.MapDefaultControllerRoute();
            });

            app.UseSwaggerUI();
        }
    }
}

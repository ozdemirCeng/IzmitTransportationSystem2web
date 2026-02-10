using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using IzmitTransportationSystem.Services;

namespace IzmitTransportationSystem
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            // CORS yapılandırması
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            
            // Swagger/OpenAPI yapılandırması
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            
            // Servislerin DI'a kaydedilmesi
            services.AddSingleton<TransportationDataService>();
            services.AddScoped<RoutePlannerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Varsayılan dosyaları (ör. index.html) sunar
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            
            // CORS middleware
            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // (İsteğe bağlı) Fallback route: 
            // Hiçbir eşleşme yoksa index.html dosyasını sunmak için:
            app.Run(async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });
        }
    }
}

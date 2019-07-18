using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TimeTrackerLukaN.Data;
using TimeTrackerLukaN.Extensions;
using TimeTrackerLukaN.Models.Validation;

namespace TimeTrackerLukaN
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
            services.AddDbContext<TimeTrackerDbContext>(options => 
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddJwtBearerAuthentication(Configuration);

            services.AddOpenApi();

            services.AddCors();

            services.AddControllers().AddFluentValidation(
                options => options.RegisterValidatorsFromAssemblyContaining<UserInputModelValidator>());

            services.AddHealthChecks()
                .AddSqlite(Configuration.GetConnectionString("DefaultConnection"));

            services.AddHealthChecksUI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<LimitingMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // WARNING: Demo purposes only
            // You should limit to a specific origin list
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                );

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHealthChecksUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", 
                    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
            });
        }
        
    }
}

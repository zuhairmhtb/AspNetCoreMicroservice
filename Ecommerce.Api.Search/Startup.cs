using Ecommerce.Api.Search.Interfaces;
using Ecommerce.Api.Search.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Polly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Search
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
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddHttpClient("OrderService", config => {
                config.BaseAddress = new Uri(Configuration["Services:Orders"]);
            }).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500))); // Retry 5 times max eac afer every 0.5 secnds
            services.AddHttpClient("ProductService", config => {
                config.BaseAddress = new Uri(Configuration["Services:Products"]);
            }).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500))); // Retry 5 times max eac afer every 0.5 secnds
            services.AddHttpClient("CustomerService", config => {
                config.BaseAddress = new Uri(Configuration["Services:Customers"]);
            }).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500))); // Retry 5 times max eac afer every 0.5 secnds
            services.AddRazorPages();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


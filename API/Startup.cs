using API.Hubs;
using API.Services;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowCredentials().AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:8080");
                });
            });

            services.AddControllers();
            services.AddSignalR();
            services.AddSwaggerGen();
            services.AddSingleton<IDataFetcher, DataFetcher>();
            services.AddHostedService<PriceUpdateService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PriceHub>("/priceHub");
            });
        }
    }
}

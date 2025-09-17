using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using API.Data;
using API.Contracts.Models.Validators;
using API.Configuration;
using API.Data.Repos;

namespace API
{
    public class Startup
    {
        /// <summary>
        /// Загрузка конфигурации из файла Json
        /// </summary>
        private IConfiguration Configuration
        { get; } = new ConfigurationBuilder()
          .AddJsonFile("Properties/ApiOptions.json")
            .AddJsonFile("appsettings.json")
          .Build();

        public Startup()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);

            // Нам не нужны представления, но в MVC бы здесь стояло AddControllersWithViews()
            services.AddControllers();

            // Добавляем новый сервис
            services.Configure<HomeOptions>(Configuration);

            // Swagger
            services.AddOpenApiDocument();

            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();

            // FluentValidation для ручной проверки
            //services.AddScoped<IValidator<AddDeviceRequest>, AddDeviceRequestValidator>();

            // FluentValidation для авто проверки На сайте документации не рекомнедовано использовать авто проверку
            services.AddValidatorsFromAssemblyContaining<AddDeviceRequestValidator>();

            // Automapper
            var assembly = Assembly.GetAssembly(typeof(MappingProfile));
            services.AddAutoMapper(assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Add OpenAPI 3.0 document serving middleware
                // Available at: http://localhost:<port>/swagger/v1/swagger.json
                app.UseOpenApi();

                // Add web UIs to interact with the document
                // Available at: http://localhost:<port>/swagger
                app.UseSwaggerUi(); // UseSwaggerUI Protected by if (env.IsDevelopment())
            }

            app.UseRouting();
            // app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            // Сопоставляем маршруты с контроллерами
            app.UseEndpoints(endpoints =>
            endpoints.MapControllers());
        }
    }
}

using ProcessorPdf.Infra.DIP;
using WebApi.Config;
using WebApi.Extensions;
using WebApi.HubSignalR;
using WebApi.Services;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddContext(builder.Configuration);
            builder.Services.AddScoped<IPdfProcessorService, PdfProcessorService>();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddRabitMqService();

            builder.Services.AddDependencies();
            builder.Services.AddSignalR();

            builder.AddCrossOrigin(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger", "WebApi v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseCors(ApiConfiguration.CorsPolicyName);

            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapHub<ProcessorPdfHub>("/ProcessorHub");
            app.MapControllers();

            app.Run();
        }
    }
}

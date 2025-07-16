using MassTransit;
using Worker.Processor.Consumer;

namespace WebApi.Extensions
{
    internal static class BuildExtension
    {
        public static void AddRabitMqService(this IServiceCollection services)
        {
            services.AddTransient<MessageBroker.IPublishMessage, MessageBroker.PublishMessage>();
            // Add RabbitMQ service configuration here
            // Example: services.AddSingleton<IRabbitMqService, RabbitMqService>();
            services.AddMassTransit(config =>
            {
                config.AddConsumer<PdfFileRequestedEventConsumer>();

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri("amqp://localhost:5672"), host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }

        public static void AddCrossOrigin(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var backendUrl = configuration["CorsConfig:BackendUrl"] ?? string.Empty;
            var frontendUrl = configuration["CorsConfig:FrontendUrl"] ?? string.Empty;
            var corsPolicyName = configuration["CorsConfig:CorsPolicyName"] ?? string.Empty;

            builder.Services.AddCors(
                options => options.AddPolicy(
                    corsPolicyName,
                        policy => policy.WithOrigins([
                            backendUrl,
                            frontendUrl])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    ));
        }
    }
}

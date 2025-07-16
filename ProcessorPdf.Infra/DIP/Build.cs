using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PdfProcessor.Core.Repository;
using ProcessorPdf.Infra.Data;
using ProcessorPdf.Infra.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorPdf.Infra.DIP
{
    public static class Build
    {
        public static IServiceCollection AddContext(this IServiceCollection service, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            service.AddDbContext<ProcessorPdfDbContext>(opts => opts.UseSqlServer(connectionString));
            return service;
        }
        public static IServiceCollection AddDependencies(this IServiceCollection service)
        {
            service.AddScoped<IPdfProcessorRepository, PdfProcessorRepository>();
            return service;
        }

    }
}

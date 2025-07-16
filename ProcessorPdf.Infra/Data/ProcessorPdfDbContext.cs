using Microsoft.EntityFrameworkCore;
using ProcessorPdf.Infra.Model;

namespace ProcessorPdf.Infra.Data
{
    public class ProcessorPdfDbContext : DbContext
    {
        public ProcessorPdfDbContext(DbContextOptions<ProcessorPdfDbContext> options)
            : base(options)
        {
        }
        // Define DbSets for your entities here
        public DbSet<PdfFileModel> PdfFile { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entity mappings here
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}

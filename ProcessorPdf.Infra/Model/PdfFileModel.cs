using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorPdf.Infra.Model
{
    [Table("PdfFile")]
    public class PdfFileModel
    {
        public Guid Id { get; set; }
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime ProcessedAt { get; set; }
        public EPdfFileStatus Status { get; set; }
    }
    public enum EPdfFileStatus
    {
        Pending,
        Completed,
        Failed
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfProcessor.Core
{
    public class BaseEntity
    {
        public BaseEntity(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }
    }
}

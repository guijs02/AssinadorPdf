namespace PdfProcessor.Core.Entity
{
    public class PdfFile : BaseEntity
    {
        public PdfFile(Guid id, string path, string name, EPdfFileStatus status) : base(id)
        {
            Path = path;
            Name = name;
            ProcessedAt = DateTime.Now;
            Status = status;

            Validate();
        }
        public string Path { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public DateTime ProcessedAt { get; private set; }
        public EPdfFileStatus Status { get; private set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new Exception("File name can't be null or empty");
            }

            if (string.IsNullOrEmpty(Status.ToString()))
            {
                throw new Exception("Status can't be null or empty");
            }
        }
    }
    public enum EPdfFileStatus
    {
        Pending,
        Completed,
        Failed
    }
}

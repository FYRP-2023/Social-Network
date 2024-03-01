using AyurConnect_Service_1.Types;

namespace AyurConnect_Service_1.DTOs
{
    public class ContentDto
    {
        public long Id { get; set; }

        public string UserId { get; set; } = null!;

        public string Header { get; set; } = null!;

        public string Body { get; set; } = null!;

        public ContentType ContentType { get; set; }
    }
}
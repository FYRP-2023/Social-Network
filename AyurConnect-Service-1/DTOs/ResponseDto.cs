namespace AyurConnect_Service_1.DTOs
{
    public class ResponseDto
    {
        public long ContentId { get; set; }

        public long ParentResponseId { get; set; }

        public long ResponseId { get; set; }

        public string UserId { get; set; } = null!;

        public string Body { get; set; } = null!;
    }
}
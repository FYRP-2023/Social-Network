using AyurConnect_Service_1.Types;

namespace AyurConnect_Service_1.DTOs
{
    public class FilterDto
    {
        public ContentType ContentType { get; set; }

        public SortType DateSortType { get; set; }

        public string? UserId { get; set; }
    }
}
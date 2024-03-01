namespace AyurConnect_Service_1.Models
{
    public abstract class Common
    {
        public long Id { get; set; }

        public string UserId { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
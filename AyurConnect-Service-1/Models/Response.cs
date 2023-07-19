using System.ComponentModel.DataAnnotations;

namespace AyurConnect_Service_1.Models
{
    public class Response : Common
    {
        [MaxLength(510)] public string Body { get; set; } = null!;

        public virtual Content Content { get; set; } = null!;

        public virtual Response? ParentResponse { get; set; }
    }
}
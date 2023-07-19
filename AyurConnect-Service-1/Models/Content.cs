using AyurConnect_Service_1.Types;
using System.ComponentModel.DataAnnotations;

namespace AyurConnect_Service_1.Models
{
    public class Content : Common
    {
        [MaxLength(255)] public string Header { get; set; } = null!;

        [MaxLength(2040)] public string Body { get; set; } = null!;

        public ContentType ContentType { get; set; }

        public virtual ICollection<Response> Responses { get; set; } = new HashSet<Response>();
    }
}
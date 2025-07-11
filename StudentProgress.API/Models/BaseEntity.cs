using System.ComponentModel.DataAnnotations;

namespace StudentProgress.API.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime InsertAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public string? InsertBy { get; set; }
        public string? UpdateBy { get; set; }
        public string TenantId { get; set; }
    }
}

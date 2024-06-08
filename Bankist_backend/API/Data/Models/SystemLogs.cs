using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Data.Models
{
    public class SystemLogs
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public Account User { get; set; }
        public string? QueryPath { get; set; }
        public string? PostData { get; set; }
        public DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
        public string? ExceptionMessage { get; set; }
        public bool IsException { get; set; }

    }
}

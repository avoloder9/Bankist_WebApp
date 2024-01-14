using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    [Table("AutentificationToken")]
    public class AutentificationToken
    {
        [Key]
        public int id { get; set; }
        public string value { get; set; }

        [ForeignKey(nameof(account))]
        public int accountId { get; set; }
        public Account account { get; set; }

        public DateTime autentificationTimestamp { get; set; }
        public string? ipAddress { get; set; }
    }
}

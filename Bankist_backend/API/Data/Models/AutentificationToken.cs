using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public string? TwoFKey { get; set; }
        public bool Is2FAUnlocked { get; set; }
    }
}

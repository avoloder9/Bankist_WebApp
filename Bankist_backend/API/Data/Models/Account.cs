using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Data.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }

        [JsonIgnore]
        public string password { get; set; }

        [JsonIgnore]
        public User? User => this as User;

        [JsonIgnore]
        public Bank? Bank => this as Bank;

        public bool isBank => Bank != null;
        public bool isUser => User != null;

    }
}

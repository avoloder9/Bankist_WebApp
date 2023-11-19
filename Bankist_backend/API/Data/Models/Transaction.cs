using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class Transaction
    {
        [Key]
        public int transactionId { get; set; }
        public DateTime transactionDate { get; set; }
        public float amount { get; set; } 
        public string type { get; set; }
        public string status { get; set; }

        public int bankId { get; set; }
        [ForeignKey(nameof(bankId))]
        public Bank bank { get; set; }

        public int userId { get; set; }
        [ForeignKey(nameof(userId))]
        public User user { get; set; }
    }
}

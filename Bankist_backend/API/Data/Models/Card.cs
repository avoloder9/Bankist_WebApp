using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class Card
    {
        [Key]
        public int cardNumber { get; set; }
        public DateTime expirationDate { get; set; }
        public DateTime issueDate { get; set; }
        public float amount { get; set; }

        public int bankId { get; set; }
        [ForeignKey(nameof(bankId))]
        public Bank bank { get; set; }

        public int userId { get; set; }
        [ForeignKey(nameof(userId))]
        public User user { get; set; }

    }
}

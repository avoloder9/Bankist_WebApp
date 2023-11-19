using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class Loyalty
    {
        [Key]
        public int loyaltyId { get; set; }
        public string status { get; set; }
        public int totalPoints { get; set; }
        public int pointToPromotion {  get; set; }

        public int bankId { get; set; }
        [ForeignKey(nameof(bankId))]
        public Bank bank { get; set; }

        public int userId { get; set; }
        [ForeignKey(nameof(userId))]
        public User user { get; set; }
    }
}

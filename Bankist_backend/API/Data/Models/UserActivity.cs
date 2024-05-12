using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class UserActivity
    {
        [Key]
        public int id { get; set; }

        public int userId { get; set; }
        [ForeignKey(nameof(userId))]
        public User user { get; set; }

        public int transactionsCount { get; set; }
        public string accountStatus {  get; set; }
        public int awardsReceived { get; set; } = 0;
    }
}

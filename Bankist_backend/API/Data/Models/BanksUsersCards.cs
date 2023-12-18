using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    [Table("BanksUsersCards")]
    public class BanksUsersCards
    {
        public int id { get; set; }

        public int bankId { get; set; }
        [ForeignKey(nameof(bankId))]
        public Bank bank { get; set; }

        public int userId { get; set; }
        [ForeignKey(nameof(userId))]
        public User user { get; set; }

        public int cardId { get; set; }
        [ForeignKey(nameof(cardId))]
        public Card card { get; set; }

        public DateTime accountIssueDate { get; set; }
    }
}

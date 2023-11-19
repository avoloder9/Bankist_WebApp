using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class BanksUsers
    {
        public int id { get; set; }

        public int bankId { get; set; }
        [ForeignKey(nameof(bankId))]
        public Bank bank { get; set; }

        public int userId { get; set; }
        [ForeignKey(nameof(userId))]
        public User user { get; set; }

        public DateTime accountIssueDate { get; set; }



    }
}

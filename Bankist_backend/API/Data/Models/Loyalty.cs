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

        public int bankUserCardId { get; set; }
        [ForeignKey(nameof(bankUserCardId))]
        public BanksUsersCards bankUserCard { get; set; }
    }
}

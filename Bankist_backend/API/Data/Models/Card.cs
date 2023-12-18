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

        public string cardTypeId {  get; set; }
        [ForeignKey(nameof(cardTypeId))]
        public CardType cardType { get; set; }
    }
}

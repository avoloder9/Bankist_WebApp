using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cardNumber { get; set; }
        public DateTime expirationDate { get; set; }
        public DateTime issueDate { get; set; }
        public float amount { get; set; }
        [Required]
        public int pin {  get; set; }

        public string cardTypeId {  get; set; }
        [ForeignKey(nameof(cardTypeId))]
        public CardType cardType { get; set; }

        public int currencyId { get; set; }
        [ForeignKey(nameof(currencyId))]
        public Currency currency { get; set; }
    }
}

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

        public int? senderCardId { get; set; }
        [ForeignKey(nameof(senderCardId))]
        public Card? senderCard { get; set; }

        public int recieverCardId { get; set; }
        [ForeignKey(nameof(recieverCardId))]
        public Card recieverCard { get; set; }
    }
}

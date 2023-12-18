using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class CardType
    {
        public string CardTypeId {  get; set; }
        public float fees { get; set; }
        public float maxLimit {  get; set; }

        public int currencyId { get; set; }
        [ForeignKey(nameof(currencyId))]
        public Currency currency { get; set; }
    }
}

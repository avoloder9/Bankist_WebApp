using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class Loan
    {
        [Key]
        public int loanId {  get; set; }
        public float amount { get; set; }
        public float interest { get; set; }
        public float rate { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime dueDate { get; set; }
        public int rateCount { get; set; }
        public float totalAmount { get; set; }
        public int ratesPayed { get; set; }
        public float totalAmountPayed { get; set; }
        public float remainingAmount { get; set; }
        public string status { get; set; }
        public int loanTypeId {  get; set; }
        [ForeignKey(nameof(loanTypeId))]
        public LoanType loanType { get; set; }

        public int cardId { get; set; }
        [ForeignKey(nameof(cardId))]
        public Card card { get; set; }
    }
}

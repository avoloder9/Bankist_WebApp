namespace API.Data.Models
{
    public class LoanType
    {
        public int loanTypeId { get; set; }
        public string name {  get; set; }
        public float maxLoanAmount {  get; set; }
        public int maximumRepaymentMonths { get; set; }
    }
}

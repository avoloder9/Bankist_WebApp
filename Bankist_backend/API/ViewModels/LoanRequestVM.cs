namespace API.ViewModels
{
    public class LoanRequestVM
    {
        public int amount {  get; set; }    
        public int senderCardId { get; set; }
        public int rates { get; set; }
        public int loanTypeId { get; set; }
    }
}

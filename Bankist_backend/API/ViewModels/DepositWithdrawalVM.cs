namespace API.ViewModels
{
    public class DepositWithdrawalVM
    {
        public DateTime transactionDate { get; set; }
        public float amount { get; set; }
        public string type { get; set; }

        public int recieverCardId { get; set; }
    }
}

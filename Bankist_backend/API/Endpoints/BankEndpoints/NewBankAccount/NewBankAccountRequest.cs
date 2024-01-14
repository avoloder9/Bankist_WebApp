namespace API.Endpoints.BankEndpoints.NewBankAccount
{
    public class NewBankAccountRequest
    {
        public string name {  get; set; }
        public float amount {  get; set; }
        public string type { get; set; }
        public string currency { get; set; }
    }
}

namespace API.Data.Models
{
    public class Currency
    {
        public int currencyId {  get; set; }
        public string currencyCode { get; set; }
        public string currencyName { get; set; }
        public string symbol {  get; set; }
        public float exchangeRate {  get; set; }
    }
}

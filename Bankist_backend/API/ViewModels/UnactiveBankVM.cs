using API.Data.Models;


namespace API.Endpoints.BankEndpoints.GetUnactiveBanks
{
    public class UnactiveBankVM
    {
        public string Name { get; set; }
        public int NumberOfUsers { get; set; }
    }
    public class GetUnactiveBanksVM
    {
        public List<UnactiveBankVM> Banks { get; set; }
        public GetUnactiveBanksVM()
        {
            Banks = new List<UnactiveBankVM>();
        }
    }
}

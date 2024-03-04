using API.Data.Models;


namespace API.Endpoints.BankEndpoints.GetActiveBanks
{
    public class ActiveBankVM
    {
        public string Name { get; set; }
    }
    public class GetActiveBanksVM
    {
        public List<ActiveBankVM> Banks { get; set; }
        public GetActiveBanksVM()
        {
            Banks = new List<ActiveBankVM>();
        }
    }
}

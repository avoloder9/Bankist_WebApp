using API.Data.Models;


namespace API.Endpoints.BankEndpoints.GetActiveBanks
{
    public class ActiveBank
    {
        public string Name { get; set; }
    }
    public class GetActiveBanksResponse
    {
        public List<ActiveBank> Banks { get; set; }
        public GetActiveBanksResponse()
        {
            Banks = new List<ActiveBank>();
        }
    }
}

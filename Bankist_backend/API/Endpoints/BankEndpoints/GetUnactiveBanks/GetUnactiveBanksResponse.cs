using API.Data.Models;


namespace API.Endpoints.BankEndpoints.GetUnactiveBanks
{
    public class UnactiveBank
    {
        public string Name { get; set; }
        public int NumberOfUsers { get; set; }
    }
    public class GetUnactiveBanksResponse
    {
        public List<UnactiveBank> Banks { get; set; }
        public GetUnactiveBanksResponse()
        {
            Banks = new List<UnactiveBank>();
        }
    }
}

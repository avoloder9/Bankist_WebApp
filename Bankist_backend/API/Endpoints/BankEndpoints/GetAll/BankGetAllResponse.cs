namespace API.Endpoints.BankEndpoints.GetAll
{
    public class BankGetAllResponse
    {
        public List<BankGetAllResponseBank> Banks { get; set; }
    }

    public class BankGetAllResponseBank
    {
        public int bankId { get; set; }
        public string bankName { get; set; }
        public string password { get; set; }
        public float totalCapital { get; set; }
        public int numberOfUsers { get; set; }
    }
}

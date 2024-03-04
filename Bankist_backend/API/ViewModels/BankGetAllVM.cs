namespace API.Endpoints.BankEndpoints.GetAll
{
    public class BankGetAllVM
    {
        public List<BankGetAllVMBank> Banks { get; set; }
    }

    public class BankGetAllVMBank
    {
        public int bankId { get; set; }
        public string bankName { get; set; }
        public string password { get; set; }
        public float totalCapital { get; set; }
        public int numberOfUsers { get; set; }
    }
}

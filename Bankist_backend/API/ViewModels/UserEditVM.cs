namespace API.ViewModels
{
    public class UserEditVM
    {
        public int Id { get; set; }
        public string? userName { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public float transactionLimit { get; set; }
        public float atmLimit { get; set; }
        public float negativeLimit { get; set; }
        
    }
}

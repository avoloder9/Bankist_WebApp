namespace API.Endpoints.UserEndpoints.Post
{
    public class UserAddRequest
    {
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public DateTime birthDate { get; set; }
    }
}

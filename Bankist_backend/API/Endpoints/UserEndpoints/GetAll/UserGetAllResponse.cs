namespace API.Endpoints.UserEndpoints.GetAll
{
    public class UserGetAllResponse
    {
        public List<UserGetAllResponseUser> Users { get; set; }
    }
    public class UserGetAllResponseUser
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public DateTime birthDate { get; set; }
        public DateTime registrationDate { get; set; }
    }
}

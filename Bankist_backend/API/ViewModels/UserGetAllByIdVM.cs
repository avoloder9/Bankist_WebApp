using API.Data.Models;

namespace API.Endpoints.UserEndpoints.GetAll
{
    public class UserGetAllByIdVM
    {
        public List<UserGetAllByIdVMUser> Users { get; set; }
        public string bankName { get; set; }
    }
    public class UserGetAllByIdVMUser
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string password { get; set; }

        public DateTime birthDate { get; set; }
        public DateTime registrationDate { get; set; }
        public float transactionLimit { get; set; }
        public float atmLimit { get; set; }
        public float negativeLimit { get; set; }
    }
}

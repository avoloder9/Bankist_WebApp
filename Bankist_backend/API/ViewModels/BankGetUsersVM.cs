namespace API.ViewModels
{
    public class BankGetUsersVM
    {
        public List<BankGetUsersVMDetails> Banks { get; set; }
    }
    public class BankGetUsersVMDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int CardNumber { get; set; }
        public float Amount { get; set; }
        public DateTime ExpirationDate { get; set; }
    }



}

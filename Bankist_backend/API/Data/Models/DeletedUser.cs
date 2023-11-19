namespace API.Data.Models
{
    public class DeletedUser
    {
        public int deletedUserId { get; set; }
        public string description { get; set; }
        public DateTime deletionDate { get; set; }
    }
}

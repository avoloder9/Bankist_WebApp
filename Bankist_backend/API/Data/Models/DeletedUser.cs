using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    [Table("DeletedUser")]

    public class DeletedUser:User
    {
        public string reason { get; set; }
        public DateTime deletionDate { get; set; }
    }
}

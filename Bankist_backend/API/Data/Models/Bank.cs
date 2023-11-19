using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class Bank
    {
        [Key]
        public int bankId { get; set; }
        public string bankName { get; set; }
        public string password { get; set; }
        public float totalCapital { get; set; }
        public int numberOfUsers { get; set; }
    }
}

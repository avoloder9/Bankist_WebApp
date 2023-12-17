using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    [Table("Bank")]
    public class Bank:Account
    {
        public float totalCapital { get; set; }
        public int numberOfUsers { get; set; }
    }
}

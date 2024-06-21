using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class Config
    {
        public int id;
        public int userId { get; set; }
        [ForeignKey(nameof(userId))]
        public User user { get; set; }
        public string theme { get; set; }
        public string locale {  get; set; }
    }
}

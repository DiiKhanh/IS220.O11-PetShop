using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    public class Comment : DateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Comment_id { get; set; }
        public string User_id { get; set; }
        public int Product_id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string Username { get; set; }
        public bool IsAccept { get; set; } = false;
    }
}

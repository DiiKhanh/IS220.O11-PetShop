using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    public class Voucher : DateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Voucher_id { get; set; }
        public string Code { get; set; }
        public string Discount_type { get; set; }
        public int Discount_value { get; set; }
        public string Start_date { get; set; }
        public string End_date { get; set; }
        public int Max_usage { get; set; } 
        public int Current_usage { get; set; } = 0;
        public bool? IsDeleted { get; set; }
    }
}

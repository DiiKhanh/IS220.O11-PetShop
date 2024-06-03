using System.ComponentModel.DataAnnotations;

namespace PetShop.DTOs
{
    public class VoucherDto
    {
        [Required]
        public string Code { get; set; }
        public string Discount_type { get; set; }
        public int Discount_value { get; set; }
        public string Start_date { get; set; }
        public string End_date { get; set; }
        public int Max_usage { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

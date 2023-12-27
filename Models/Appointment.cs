using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    public class Appointment : DateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Appointment_id { get; set; }
        public string User_id { get; set; }
        public int Dog_item_id {  get; set; }
        public string Date { get; set; }
        public string Hour { get; set; }
        public string Description { get; set; }
        public string Phone_number { get; set; }
        public string User_name { get; set; }
        public string Service { get; set; }
        public string Result { get; set; } = string.Empty;
        public string Status { get; set; }
        public bool Is_cancel { get; set; } = false;
    }
}

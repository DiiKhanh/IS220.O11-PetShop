using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public enum PaymentMethod { ThanhToanKhiNhanHang, ThanhToanTrucTuyen }

    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? InvoiceId { get; set; }
        public int? OrderId { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }

        public Order Order { get; set; } = null!;
    }
}

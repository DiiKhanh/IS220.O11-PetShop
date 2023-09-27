using PetShop.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public enum Status { ChuaThanhToan, DaThanhToan }
    public enum Shipment { ChoXacNhan, DongGoiChoiVanChuyen, DangVanChuyen, GiaoHangThanhCong }

    public class Order : DateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? OrderId { get; set; }
        public string? UserId { get; set; }
        public int Total { get; set; }

        public int ShipInfoId { get; set; }
        public Status? OrderStatus { get; set; }
        public Shipment? ShipmentStatus { get; set; }
        public bool? IsDeleted { get; set; }


        public ApplicationUser User { get; set; } = null!;

        public ShipInfo ShipInfo { get; set; } = null!;

        public Invoice? Invoice { get; set; } = null!;

        public ICollection<OrderDetail> orderDetails { get; set; } = new List<OrderDetail>();

    }
}

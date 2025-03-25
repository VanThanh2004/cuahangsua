using System.ComponentModel.DataAnnotations;

namespace cuahangsua.Models
{
    public class donhang
    {
        [Key]
        public int Id { get; set; } // Id của bản ghi đơn hàng (khóa chính)

        [Required]
        public int ProductId { get; set; } // Id sản phẩm

        [Required]
        public string ProductName { get; set; } = string.Empty; // Tên sản phẩm

        [Required]
        public int Quantity { get; set; } // Số lượng sản phẩm

        [Required]
        public decimal Price { get; set; } // Giá sản phẩm

        // Tính Tổng tiền (Số lượng * Giá)
        public decimal TotalAmount => Quantity * Price;
    }
}
using System.ComponentModel.DataAnnotations;

namespace cuahangsua.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; } // Mã mục giỏ hàng

        [Required]
        public int ProductId { get; set; }      // ID sản phẩm

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; } // Tên sản phẩm

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }       // Số lượng

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }      // Giá sản phẩm

        public string ImageUrl { get; set; }    // Hình ảnh sản phẩm

        // Tổng tiền của mục giỏ hàng (tính tự động)
        [DataType(DataType.Currency)]
        public decimal TotalPrice => Quantity * Price;

        // Thông tin khách hàng
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }    // Tên khách hàng

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }   // Email khách hàng

        // Liên kết đến Product (nếu cần)
        public Product Product { get; set; }

        // Thêm ngày tạo mục giỏ hàng
        [DataType(DataType.Date)]
        public DateTime NgayTao { get; set; } // Ngày tạo mục giỏ hàng

        // Hàm tăng số lượng sản phẩm
        public void IncreaseQuantity(int amount = 1)
        {
            Quantity += amount;
        }

        // Hàm giảm số lượng sản phẩm
        public void DecreaseQuantity(int amount = 1)
        {
            if (Quantity > amount)
                Quantity -= amount;
            else
                Quantity = 1; // Không cho giảm về 0
        }
    }
}
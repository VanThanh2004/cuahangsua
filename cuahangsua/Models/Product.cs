using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuahangsua.Models
{
    [Table("Products")] // Đặt tên bảng trong database
    public class Product
    {
        [Key] // Đánh dấu Id là khóa chính
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được quá 100 ký tự")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được quá 500 ký tự")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; }

        [Required]
        [Url(ErrorMessage = "Vui lòng nhập đường dẫn hợp lệ")]
        public string ImageUrl { get; set; } = "/images/default.png";

        [Required(ErrorMessage = "Danh mục không được để trống")]
        public string Category { get; set; }

        // Trường này giúp theo dõi ngày thêm sản phẩm vào hệ thống
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động sinh giá trị khi tạo mới
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

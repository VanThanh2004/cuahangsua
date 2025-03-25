namespace cuahangsua.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Mật khẩu đã mã hóa
        public bool IsAdmin { get; set; } = false;
    }
}
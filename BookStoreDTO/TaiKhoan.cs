namespace BookStoreDTO
{
    public class TaiKhoan
    {
        public int MaTK { get; set; }
        public string TenDangNhap { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string VaiTro { get; set; } = string.Empty;
        public int MaNV { get; set; }
        public string? TenNV { get; set; }
    }
}
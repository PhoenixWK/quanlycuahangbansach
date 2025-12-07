namespace BookStoreDTO
{
    public class NhanVien
    {
        public int MaNV { get; set; }
        public string TenNV { get; set; } = string.Empty;
        public string? SDT { get; set; }
        public string? DiaChi { get; set; }
        public DateTime NgayVaoLam { get; set; }
    }

    public class NhanVienDTO
    {
        public int MaNV { get; set; }
        public string TenNV { get; set; } = string.Empty;
        public string? SDT { get; set; }
        public string? DiaChi { get; set; }
        public DateTime NgayVaoLam { get; set; }
        public string? TenDangNhap { get; set; }
        public string? VaiTro { get; set; }
    }
}
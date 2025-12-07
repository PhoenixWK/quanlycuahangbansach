using System;

namespace BookStoreDTO
{
    public class KhachHangDTO
    {
        public int MaKH { get; set; }
        public string TenKH { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        public KhachHangDTO()
        {
            NgayTao = DateTime.Now;
        }

        public KhachHangDTO(int maKH, string tenKH, string? email = null, string? soDienThoai = null, string? diaChi = null)
        {
            MaKH = maKH;
            TenKH = tenKH;
            Email = email;
            SoDienThoai = soDienThoai;
            DiaChi = diaChi;
            NgayTao = DateTime.Now;
        }
    }
}
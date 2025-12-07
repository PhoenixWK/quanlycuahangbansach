using System;

namespace BookStoreDTO
{
    public class HoaDonDTO
    {
        public int MaHD { get; set; }
        public DateTime NgayBan { get; set; }
        public decimal TongTien { get; set; }
        public int MaNV { get; set; }
        public int MaKH { get; set; }
        
        // Additional properties for display
        public string? TenNV { get; set; }
        public string? TenKH { get; set; }
    }
}
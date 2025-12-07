namespace BookStoreDTO
{
    public class ChiTietHDDTO
    {
        public int MaCTHD { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public int MaHD { get; set; }
        public string? MaSach { get; set; }
        
        // Additional properties for display
        public string? TenSach { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }
}
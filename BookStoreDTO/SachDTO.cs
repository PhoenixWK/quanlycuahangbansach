using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDTO
{
    public class SachDTO
    {
        public string? MaSach { get; set; }
        public string? TenSach { get; set; }
        public string? MaTG { get; set; }
        public string? TenTacGia { get; set; } // For display purposes
        public string? MaTL { get; set; }
        public string? TenTheLoai { get; set; } // For display purposes
        public decimal? GiaBan { get; set; }
        public int? SoLuongTon { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnh { get; set; }
        public DateTime? NgayXuatBan { get; set; }
        public string? NhaXuatBan { get; set; }
        public int? SoTrang { get; set; }
        public bool? TrangThai { get; set; }

        public SachDTO()
        {
            TrangThai = true;
        }

        public SachDTO(string maSach, string tenSach, string maTG, string maTL, decimal giaBan, int soLuongTon, string moTa, string hinhAnh, DateTime? ngayXuatBan, string nhaXuatBan, int soTrang, bool trangThai)
        {
            MaSach = maSach;
            TenSach = tenSach;
            MaTG = maTG;
            MaTL = maTL;
            GiaBan = giaBan;
            SoLuongTon = soLuongTon;
            MoTa = moTa;
            HinhAnh = hinhAnh;
            NgayXuatBan = ngayXuatBan;
            NhaXuatBan = nhaXuatBan;
            SoTrang = soTrang;
            TrangThai = trangThai;
        }
    }
}
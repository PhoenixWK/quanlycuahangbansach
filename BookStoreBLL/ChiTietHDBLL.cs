using BookStoreDAL;
using BookStoreDTO;
using System;
using System.Collections.Generic;

namespace BookStoreBLL
{
    public class ChiTietHDBLL
    {
        private readonly ChiTietHDDAL chiTietHDDAL;

        public ChiTietHDBLL()
        {
            chiTietHDDAL = new ChiTietHDDAL();
        }

        public bool ThemChiTietHD(ChiTietHDDTO chiTiet)
        {
            try
            {
                // Validation
                if (chiTiet.MaHD <= 0)
                    throw new ArgumentException("Mã hóa đơn không hợp lệ");
                
                if (string.IsNullOrWhiteSpace(chiTiet.MaSach))
                    throw new ArgumentException("Mã sách không hợp lệ");
                
                if (chiTiet.SoLuong <= 0)
                    throw new ArgumentException("Số lượng phải lớn hơn 0");
                
                if (chiTiet.DonGia <= 0)
                    throw new ArgumentException("Đơn giá phải lớn hơn 0");

                return chiTietHDDAL.ThemChiTietHD(chiTiet);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi thêm chi tiết hóa đơn: " + ex.Message);
            }
        }

        public List<ChiTietHDDTO> LayChiTietHoaDon(int maHD)
        {
            try
            {
                if (maHD <= 0)
                    throw new ArgumentException("Mã hóa đơn không hợp lệ");

                return chiTietHDDAL.LayChiTietHoaDon(maHD);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lấy chi tiết hóa đơn: " + ex.Message);
            }
        }

        public bool CapNhatChiTietHD(ChiTietHDDTO chiTiet)
        {
            try
            {
                // Validation
                if (chiTiet.MaCTHD <= 0)
                    throw new ArgumentException("Mã chi tiết hóa đơn không hợp lệ");
                
                if (chiTiet.SoLuong <= 0)
                    throw new ArgumentException("Số lượng phải lớn hơn 0");
                
                if (chiTiet.DonGia <= 0)
                    throw new ArgumentException("Đơn giá phải lớn hơn 0");

                return chiTietHDDAL.CapNhatChiTietHD(chiTiet);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi cập nhật chi tiết hóa đơn: " + ex.Message);
            }
        }

        public List<ChiTietHDDTO> LayChiTietTheoMaHD(int maHD)
        {
            return LayChiTietHoaDon(maHD);
        }

        public bool XoaChiTietHD(int maCTHD)
        {
            try
            {
                if (maCTHD <= 0)
                    throw new ArgumentException("Mã chi tiết hóa đơn không hợp lệ");

                return chiTietHDDAL.XoaChiTietHD(maCTHD);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi xóa chi tiết hóa đơn: " + ex.Message);
            }
        }
    }
}
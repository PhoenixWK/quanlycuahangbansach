using BookStoreDAL;
using BookStoreDTO;
using System;
using System.Collections.Generic;

namespace BookStoreBLL
{
    public class HoaDonBLL
    {
        private readonly HoaDonDAL hoaDonDAL;

        public HoaDonBLL()
        {
            hoaDonDAL = new HoaDonDAL();
        }

        public int ThemHoaDon(HoaDonDTO hoaDon)
        {
            try
            {
                // Validation
                if (hoaDon.MaNV <= 0)
                    throw new ArgumentException("Mã nhân viên không hợp lệ");
                
                if (hoaDon.MaKH <= 0)
                    throw new ArgumentException("Mã khách hàng không hợp lệ");
                
                if (hoaDon.TongTien <= 0)
                    throw new ArgumentException("Tổng tiền phải lớn hơn 0");

                return hoaDonDAL.ThemHoaDon(hoaDon);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi thêm hóa đơn: " + ex.Message);
            }
        }

        public List<HoaDonDTO> LayDanhSachHoaDon()
        {
            try
            {
                return hoaDonDAL.LayDanhSachHoaDon();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lấy danh sách hóa đơn: " + ex.Message);
            }
        }

        public HoaDonDTO? LayHoaDonTheoMa(int maHD)
        {
            try
            {
                if (maHD <= 0)
                    throw new ArgumentException("Mã hóa đơn không hợp lệ");

                return hoaDonDAL.LayHoaDonTheoMa(maHD);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lấy thông tin hóa đơn: " + ex.Message);
            }
        }

        public bool CapNhatHoaDon(HoaDonDTO hoaDon)
        {
            try
            {
                if (hoaDon.MaHD <= 0)
                    throw new ArgumentException("Mã hóa đơn không hợp lệ");
                
                if (hoaDon.MaNV <= 0)
                    throw new ArgumentException("Mã nhân viên không hợp lệ");
                
                if (hoaDon.MaKH <= 0)
                    throw new ArgumentException("Mã khách hàng không hợp lệ");
                
                if (hoaDon.TongTien <= 0)
                    throw new ArgumentException("Tổng tiền phải lớn hơn 0");

                return hoaDonDAL.CapNhatHoaDon(hoaDon);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi cập nhật hóa đơn: " + ex.Message);
            }
        }

        public bool XoaHoaDon(int maHD)
        {
            try
            {
                if (maHD <= 0)
                    throw new ArgumentException("Mã hóa đơn không hợp lệ");

                return hoaDonDAL.XoaHoaDon(maHD);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi xóa hóa đơn: " + ex.Message);
            }
        }
    }
}
using BookStoreDAL;
using BookStoreDTO;

namespace BookStoreBLL
{
    public class SachBLL
    {
        private SachDAL sachDAL;

        public SachBLL()
        {
            sachDAL = new SachDAL();
        }

        public List<SachDTO> LayDanhSachSach()
        {
            try
            {
                return sachDAL.LayDanhSachSach();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lấy danh sách sách: " + ex.Message);
            }
        }

        public List<SachDTO> TimKiemSach(string tuKhoa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tuKhoa))
                {
                    return LayDanhSachSach();
                }
                return sachDAL.TimKiemSach(tuKhoa);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi tìm kiếm sách: " + ex.Message);
            }
        }

        public bool ThemSach(SachDTO sach)
        {
            try
            {
                // Validate business rules
                if (string.IsNullOrWhiteSpace(sach.MaSach))
                    throw new Exception("Mã sách không được để trống!");

                if (string.IsNullOrWhiteSpace(sach.TenSach))
                    throw new Exception("Tên sách không được để trống!");

                if (string.IsNullOrWhiteSpace(sach.MaTG))
                    throw new Exception("Vui lòng chọn tác giả!");

                if (string.IsNullOrWhiteSpace(sach.MaTL))
                    throw new Exception("Vui lòng chọn thể loại!");

                if (sach.GiaBan <= 0)
                    throw new Exception("Giá bán phải lớn hơn 0!");

                if (sach.SoLuongTon < 0)
                    throw new Exception("Số lượng tồn không được âm!");

                // Check if book code already exists
                if (sachDAL.KiemTraMaSachTonTai(sach.MaSach))
                    throw new Exception("Mã sách đã tồn tại!");

                return sachDAL.ThemSach(sach);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi thêm sách: " + ex.Message);
            }
        }

        public bool CapNhatSach(SachDTO sach)
        {
            try
            {
                // Validate business rules
                if (string.IsNullOrWhiteSpace(sach.MaSach))
                    throw new Exception("Mã sách không được để trống!");

                if (string.IsNullOrWhiteSpace(sach.TenSach))
                    throw new Exception("Tên sách không được để trống!");

                if (string.IsNullOrWhiteSpace(sach.MaTG))
                    throw new Exception("Vui lòng chọn tác giả!");

                if (string.IsNullOrWhiteSpace(sach.MaTL))
                    throw new Exception("Vui lòng chọn thể loại!");

                if (sach.GiaBan <= 0)
                    throw new Exception("Giá bán phải lớn hơn 0!");

                if (sach.SoLuongTon < 0)
                    throw new Exception("Số lượng tồn không được âm!");

                return sachDAL.CapNhatSach(sach);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi cập nhật sách: " + ex.Message);
            }
        }

        public bool XoaSach(string maSach)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maSach))
                    throw new Exception("Mã sách không được để trống!");

                return sachDAL.XoaSach(maSach);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi xóa sách: " + ex.Message);
            }
        }

        public SachDTO? LaySachTheoMa(string maSach)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maSach))
                    throw new Exception("Mã sách không được để trống!");

                return sachDAL.LaySachTheoMa(maSach);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lấy thông tin sách: " + ex.Message);
            }
        }

        public string TaoMaSachMoi()
        {
            try
            {
                var danhSach = LayDanhSachSach();
                int soLuong = danhSach.Count;
                string maMoi;
                
                do
                {
                    soLuong++;
                    maMoi = "S" + soLuong.ToString("D3");
                } while (sachDAL.KiemTraMaSachTonTai(maMoi));

                return maMoi;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi tạo mã sách mới: " + ex.Message);
            }
        }
    }
}
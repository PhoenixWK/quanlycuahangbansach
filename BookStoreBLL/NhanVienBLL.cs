using BookStoreDAL;
using BookStoreDTO;

namespace BookStoreBLL
{
    public class NhanVienBLL
    {
        private NhanVienDAL nhanVienDAL;

        public NhanVienBLL()
        {
            nhanVienDAL = new NhanVienDAL();
        }

        // Lấy danh sách tất cả nhân viên
        public List<NhanVienDTO> LayDanhSachNhanVien()
        {
            try
            {
                return nhanVienDAL.LayDanhSachNhanVien();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi lấy danh sách nhân viên: " + ex.Message);
            }
        }

        // Thêm nhân viên mới
        public bool ThemNhanVien(NhanVienDTO nhanVien)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(nhanVien.TenNV))
                    throw new Exception("Tên nhân viên không được để trống!");

                if (nhanVien.NgayVaoLam > DateTime.Now)
                    throw new Exception("Ngày vào làm không thể lớn hơn ngày hiện tại!");

                return nhanVienDAL.ThemNhanVien(nhanVien);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi thêm nhân viên: " + ex.Message);
            }
        }

        // Cập nhật thông tin nhân viên
        public bool CapNhatNhanVien(NhanVienDTO nhanVien)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(nhanVien.TenNV))
                    throw new Exception("Tên nhân viên không được để trống!");

                if (nhanVien.NgayVaoLam > DateTime.Now)
                    throw new Exception("Ngày vào làm không thể lớn hơn ngày hiện tại!");

                return nhanVienDAL.CapNhatNhanVien(nhanVien);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi cập nhật nhân viên: " + ex.Message);
            }
        }

        // Xóa nhân viên
        public bool XoaNhanVien(int maNV)
        {
            try
            {
                return nhanVienDAL.XoaNhanVien(maNV);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi xóa nhân viên: " + ex.Message);
            }
        }

        // Tìm kiếm nhân viên theo tên hoặc số điện thoại
        public List<NhanVienDTO> TimKiemNhanVien(string tuKhoa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tuKhoa))
                    return LayDanhSachNhanVien();

                return nhanVienDAL.TimKiemNhanVien(tuKhoa);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong BLL khi tìm kiếm nhân viên: " + ex.Message);
            }
        }
    }
}
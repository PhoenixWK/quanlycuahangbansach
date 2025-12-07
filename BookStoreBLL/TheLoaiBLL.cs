using BookStoreDAL;
using BookStoreDTO;

namespace BookStoreBLL
{
    public class TheLoaiBLL
    {
        private readonly TheLoaiDAL theLoaiDAL;

        public TheLoaiBLL()
        {
            theLoaiDAL = new TheLoaiDAL();
        }

        public List<TheLoaiDTO> LayDanhSachTheLoai()
        {
            try
            {
                return theLoaiDAL.LayDanhSachTheLoai();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi lấy danh sách thể loại: {ex.Message}");
            }
        }

        public List<TheLoaiDTO> TimKiemTheLoai(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return LayDanhSachTheLoai();
                }

                return theLoaiDAL.TimKiemTheLoai(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi tìm kiếm thể loại: {ex.Message}");
            }
        }

        public bool ThemTheLoai(TheLoaiDTO theLoai)
        {
            try
            {
                // Validate data
                if (string.IsNullOrWhiteSpace(theLoai.TenTL))
                {
                    throw new Exception("Tên thể loại không được để trống!");
                }

                if (theLoai.TenTL.Trim().Length < 2)
                {
                    throw new Exception("Tên thể loại phải có ít nhất 2 ký tự!");
                }

                // Check name uniqueness
                if (theLoaiDAL.KiemTraTenTheLoaiTonTai(theLoai.TenTL.Trim()))
                {
                    throw new Exception("Tên thể loại đã tồn tại trong hệ thống!");
                }

                // Trim and clean data
                theLoai.TenTL = theLoai.TenTL.Trim();

                return theLoaiDAL.ThemTheLoai(theLoai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi thêm thể loại: {ex.Message}");
            }
        }

        public bool CapNhatTheLoai(TheLoaiDTO theLoai)
        {
            try
            {
                // Validate data
                if (string.IsNullOrWhiteSpace(theLoai.TenTL))
                {
                    throw new Exception("Tên thể loại không được để trống!");
                }

                if (theLoai.TenTL.Trim().Length < 2)
                {
                    throw new Exception("Tên thể loại phải có ít nhất 2 ký tự!");
                }

                // Check name uniqueness (exclude current record)
                if (theLoaiDAL.KiemTraTenTheLoaiTonTai(theLoai.TenTL.Trim(), theLoai.MaTL))
                {
                    throw new Exception("Tên thể loại đã tồn tại trong hệ thống!");
                }

                // Trim and clean data
                theLoai.TenTL = theLoai.TenTL.Trim();

                return theLoaiDAL.CapNhatTheLoai(theLoai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi cập nhật thể loại: {ex.Message}");
            }
        }

        public bool XoaTheLoai(int maTL)
        {
            try
            {
                if (maTL <= 0)
                {
                    throw new Exception("Mã thể loại không hợp lệ!");
                }

                return theLoaiDAL.XoaTheLoai(maTL);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi xóa thể loại: {ex.Message}");
            }
        }

        public TheLoaiDTO? LayTheLoaiTheoMa(int maTL)
        {
            try
            {
                if (maTL <= 0)
                {
                    throw new Exception("Mã thể loại không hợp lệ!");
                }

                return theLoaiDAL.LayTheLoaiTheoMa(maTL);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi lấy thông tin thể loại: {ex.Message}");
            }
        }
    }
}
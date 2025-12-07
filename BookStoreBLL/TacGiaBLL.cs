using BookStoreDAL;
using BookStoreDTO;

namespace BookStoreBLL
{
    public class TacGiaBLL
    {
        private readonly TacGiaDAL tacGiaDAL;

        public TacGiaBLL()
        {
            tacGiaDAL = new TacGiaDAL();
        }

        public List<TacGiaDTO> LayDanhSachTacGia()
        {
            try
            {
                return tacGiaDAL.LayDanhSachTacGia();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi lấy danh sách tác giả: {ex.Message}");
            }
        }

        public List<TacGiaDTO> TimKiemTacGia(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return LayDanhSachTacGia();
                }

                return tacGiaDAL.TimKiemTacGia(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi tìm kiếm tác giả: {ex.Message}");
            }
        }

        public bool ThemTacGia(TacGiaDTO tacGia)
        {
            try
            {
                // Validate data
                if (string.IsNullOrWhiteSpace(tacGia.TenTG))
                {
                    throw new Exception("Tên tác giả không được để trống!");
                }

                if (tacGia.TenTG.Trim().Length < 2)
                {
                    throw new Exception("Tên tác giả phải có ít nhất 2 ký tự!");
                }

                // Check name uniqueness
                if (tacGiaDAL.KiemTraTenTacGiaTonTai(tacGia.TenTG.Trim()))
                {
                    throw new Exception("Tên tác giả đã tồn tại trong hệ thống!");
                }

                // Trim and clean data
                tacGia.TenTG = tacGia.TenTG.Trim();

                return tacGiaDAL.ThemTacGia(tacGia);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi thêm tác giả: {ex.Message}");
            }
        }

        public bool CapNhatTacGia(TacGiaDTO tacGia)
        {
            try
            {
                // Validate data
                if (string.IsNullOrWhiteSpace(tacGia.TenTG))
                {
                    throw new Exception("Tên tác giả không được để trống!");
                }

                if (tacGia.TenTG.Trim().Length < 2)
                {
                    throw new Exception("Tên tác giả phải có ít nhất 2 ký tự!");
                }

                // Check name uniqueness (exclude current record)
                if (tacGiaDAL.KiemTraTenTacGiaTonTai(tacGia.TenTG.Trim(), tacGia.MaTG))
                {
                    throw new Exception("Tên tác giả đã tồn tại trong hệ thống!");
                }

                // Trim and clean data
                tacGia.TenTG = tacGia.TenTG.Trim();

                return tacGiaDAL.CapNhatTacGia(tacGia);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi cập nhật tác giả: {ex.Message}");
            }
        }

        public bool XoaTacGia(int maTG)
        {
            try
            {
                if (maTG <= 0)
                {
                    throw new Exception("Mã tác giả không hợp lệ!");
                }

                return tacGiaDAL.XoaTacGia(maTG);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi xóa tác giả: {ex.Message}");
            }
        }

        public TacGiaDTO? LayTacGiaTheoMa(int maTG)
        {
            try
            {
                if (maTG <= 0)
                {
                    throw new Exception("Mã tác giả không hợp lệ!");
                }

                return tacGiaDAL.LayTacGiaTheoMa(maTG);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi lấy thông tin tác giả: {ex.Message}");
            }
        }
    }
}
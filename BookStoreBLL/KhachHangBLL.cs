using BookStoreDAL;
using BookStoreDTO;

namespace BookStoreBLL
{
    public class KhachHangBLL
    {
        private readonly KhachHangDAL khachHangDAL;

        public KhachHangBLL()
        {
            khachHangDAL = new KhachHangDAL();
        }

        public List<KhachHangDTO> LayDanhSachKhachHang()
        {
            try
            {
                return khachHangDAL.LayDanhSachKhachHang();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi lấy danh sách khách hàng: {ex.Message}");
            }
        }

        public List<KhachHangDTO> TimKiemKhachHang(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return LayDanhSachKhachHang();
                }

                return khachHangDAL.TimKiemKhachHang(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi tìm kiếm khách hàng: {ex.Message}");
            }
        }

        public bool ThemKhachHang(KhachHangDTO customer)
        {
            try
            {
                // Validate customer data
                if (string.IsNullOrWhiteSpace(customer.TenKH))
                {
                    throw new Exception("Tên khách hàng không được để trống!");
                }

                // Check email uniqueness if provided
                if (!string.IsNullOrWhiteSpace(customer.Email))
                {
                    if (khachHangDAL.KiemTraEmailTonTai(customer.Email))
                    {
                        throw new Exception("Email đã tồn tại trong hệ thống!");
                    }
                }

                // Set creation time
                customer.NgayTao = DateTime.Now;

                return khachHangDAL.ThemKhachHang(customer);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi thêm khách hàng: {ex.Message}");
            }
        }

        public bool CapNhatKhachHang(KhachHangDTO customer)
        {
            try
            {
                // Validate customer data
                if (string.IsNullOrWhiteSpace(customer.TenKH))
                {
                    throw new Exception("Tên khách hàng không được để trống!");
                }

                // Check email uniqueness if provided
                if (!string.IsNullOrWhiteSpace(customer.Email))
                {
                    if (khachHangDAL.KiemTraEmailTonTai(customer.Email, customer.MaKH))
                    {
                        throw new Exception("Email đã tồn tại trong hệ thống!");
                    }
                }

                return khachHangDAL.CapNhatKhachHang(customer);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi cập nhật khách hàng: {ex.Message}");
            }
        }

        public bool XoaKhachHang(int maKH)
        {
            try
            {
                return khachHangDAL.XoaKhachHang(maKH);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi xóa khách hàng: {ex.Message}");
            }
        }

        public KhachHangDTO? LayKhachHangTheoMa(int maKH)
        {
            try
            {
                return khachHangDAL.LayKhachHangTheoMa(maKH);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi lấy thông tin khách hàng: {ex.Message}");
            }
        }

        public bool KiemTraEmailTonTai(string email, int? excludeId = null)
        {
            try
            {
                return khachHangDAL.KiemTraEmailTonTai(email, excludeId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi trong BLL khi kiểm tra email: {ex.Message}");
            }
        }
    }
}
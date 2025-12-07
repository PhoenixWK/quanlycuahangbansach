using BookStoreDAL;
using BookStoreDTO;

namespace BookStoreBLL
{
    public class TaiKhoanBLL
    {
        private readonly TaiKhoanDAL _taiKhoanDAL;

        public TaiKhoanBLL()
        {
            _taiKhoanDAL = new TaiKhoanDAL();
        }

        public TaiKhoan? DangNhap(string tenDangNhap, string matKhau)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                throw new ArgumentException("Tên đăng nhập không được để trống");
            
            if (string.IsNullOrWhiteSpace(matKhau))
                throw new ArgumentException("Mật khẩu không được để trống");

            // Trim whitespace
            tenDangNhap = tenDangNhap.Trim();
            matKhau = matKhau.Trim();

            return _taiKhoanDAL.DangNhap(tenDangNhap, matKhau);
        }

        public bool TaoTaiKhoan(int maNV, string tenDangNhap, string matKhau, string vaiTro)
        {
            // Validate input
            if (maNV <= 0)
                throw new ArgumentException("Mã nhân viên không hợp lệ");
                
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                throw new ArgumentException("Tên đăng nhập không được để trống");
                
            if (string.IsNullOrWhiteSpace(matKhau))
                throw new ArgumentException("Mật khẩu không được để trống");
                
            if (string.IsNullOrWhiteSpace(vaiTro))
                throw new ArgumentException("Vai trò không được để trống");

            // Validate role
            if (vaiTro != "Admin" && vaiTro != "NhanVien")
                throw new ArgumentException("Vai trò phải là 'Admin' hoặc 'NhanVien'");

            // Check if account already exists for this employee
            if (_taiKhoanDAL.KiemTraTaiKhoanTonTai(maNV))
                throw new InvalidOperationException("Nhân viên đã có tài khoản");

            // Check if username already exists
            if (_taiKhoanDAL.KiemTraTenDangNhapTonTai(tenDangNhap.Trim()))
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại");

            return _taiKhoanDAL.TaoTaiKhoan(maNV, tenDangNhap.Trim(), matKhau.Trim(), vaiTro);
        }

        public bool CapNhatVaiTro(int maNV, string vaiTroMoi)
        {
            // Validate input
            if (maNV <= 0)
                throw new ArgumentException("Mã nhân viên không hợp lệ");
                
            if (string.IsNullOrWhiteSpace(vaiTroMoi))
                throw new ArgumentException("Vai trò mới không được để trống");

            // Validate role
            if (vaiTroMoi != "Admin" && vaiTroMoi != "NhanVien")
                throw new ArgumentException("Vai trò phải là 'Admin' hoặc 'NhanVien'");

            // Check if account exists
            if (!_taiKhoanDAL.KiemTraTaiKhoanTonTai(maNV))
                throw new InvalidOperationException("Nhân viên chưa có tài khoản");

            return _taiKhoanDAL.CapNhatVaiTro(maNV, vaiTroMoi);
        }

        public bool DatLaiMatKhau(int maNV, string matKhauMoi)
        {
            // Validate input
            if (maNV <= 0)
                throw new ArgumentException("Mã nhân viên không hợp lệ");
                
            if (string.IsNullOrWhiteSpace(matKhauMoi))
                throw new ArgumentException("Mật khẩu mới không được để trống");

            // Check if account exists
            if (!_taiKhoanDAL.KiemTraTaiKhoanTonTai(maNV))
                throw new InvalidOperationException("Nhân viên chưa có tài khoản");

            return _taiKhoanDAL.DatLaiMatKhau(maNV, matKhauMoi.Trim());
        }

        public bool XoaTaiKhoan(int maNV)
        {
            // Validate input
            if (maNV <= 0)
                throw new ArgumentException("Mã nhân viên không hợp lệ");

            // Check if account exists
            if (!_taiKhoanDAL.KiemTraTaiKhoanTonTai(maNV))
                throw new InvalidOperationException("Nhân viên chưa có tài khoản");

            return _taiKhoanDAL.XoaTaiKhoan(maNV);
        }

        public bool DoiMatKhau(string tenDangNhap, string matKhauCu, string matKhauMoi)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                throw new ArgumentException("Tên đăng nhập không được để trống");
                
            if (string.IsNullOrWhiteSpace(matKhauCu))
                throw new ArgumentException("Mật khẩu cũ không được để trống");
                
            if (string.IsNullOrWhiteSpace(matKhauMoi))
                throw new ArgumentException("Mật khẩu mới không được để trống");

            // Verify old password
            var taiKhoan = _taiKhoanDAL.DangNhap(tenDangNhap.Trim(), matKhauCu.Trim());
            if (taiKhoan == null)
                throw new UnauthorizedAccessException("Mật khẩu cũ không đúng");

            return _taiKhoanDAL.DatLaiMatKhau(taiKhoan.MaNV, matKhauMoi.Trim());
        }

        public bool KiemTraKetNoiDatabase()
        {
            return DatabaseConnection.TestConnection();
        }
    }
}
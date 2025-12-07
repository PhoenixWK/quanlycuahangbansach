using BookStoreDTO;
using MySql.Data.MySqlClient;

namespace BookStoreDAL
{
    public class TaiKhoanDAL
    {
        public TaiKhoan? DangNhap(string tenDangNhap, string matKhau)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT tk.MaTK, tk.TenDangNhap, tk.MatKhau, tk.VaiTro, tk.MaNV, nv.TenNV
                        FROM TaiKhoan tk
                        INNER JOIN NhanVien nv ON tk.MaNV = nv.MaNV
                        WHERE tk.TenDangNhap = @tenDangNhap AND tk.MatKhau = @matKhau";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tenDangNhap", tenDangNhap);
                        command.Parameters.AddWithValue("@matKhau", matKhau);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TaiKhoan
                                {
                                    MaTK = reader.GetInt32("MaTK"),
                                    TenDangNhap = reader.GetString("TenDangNhap"),
                                    MatKhau = reader.GetString("MatKhau"),
                                    VaiTro = reader.GetString("VaiTro"),
                                    MaNV = reader.GetInt32("MaNV"),
                                    TenNV = reader.GetString("TenNV")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đăng nhập: {ex.Message}");
            }
            return null;
        }

        public bool KiemTraTonTaiTaiKhoan(string tenDangNhap)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @tenDangNhap";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tenDangNhap", tenDangNhap);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra tài khoản: {ex.Message}");
            }
        }

        public bool KiemTraTaiKhoanTonTai(int maNV)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM TaiKhoan WHERE MaNV = @maNV";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maNV", maNV);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra tài khoản nhân viên: {ex.Message}");
            }
        }

        public bool KiemTraTenDangNhapTonTai(string tenDangNhap)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @tenDangNhap";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tenDangNhap", tenDangNhap);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra tên đăng nhập: {ex.Message}");
            }
        }

        public bool TaoTaiKhoan(int maNV, string tenDangNhap, string matKhau, string vaiTro)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO TaiKhoan (MaNV, TenDangNhap, MatKhau, VaiTro) 
                        VALUES (@maNV, @tenDangNhap, @matKhau, @vaiTro)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maNV", maNV);
                        command.Parameters.AddWithValue("@tenDangNhap", tenDangNhap);
                        command.Parameters.AddWithValue("@matKhau", matKhau);
                        command.Parameters.AddWithValue("@vaiTro", vaiTro);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo tài khoản: {ex.Message}");
            }
        }

        public bool CapNhatVaiTro(int maNV, string vaiTroMoi)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = "UPDATE TaiKhoan SET VaiTro = @vaiTroMoi WHERE MaNV = @maNV";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@vaiTroMoi", vaiTroMoi);
                        command.Parameters.AddWithValue("@maNV", maNV);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật vai trò: {ex.Message}");
            }
        }

        public bool DatLaiMatKhau(int maNV, string matKhauMoi)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = "UPDATE TaiKhoan SET MatKhau = @matKhauMoi WHERE MaNV = @maNV";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@matKhauMoi", matKhauMoi);
                        command.Parameters.AddWithValue("@maNV", maNV);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đặt lại mật khẩu: {ex.Message}");
            }
        }

        public bool XoaTaiKhoan(int maNV)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM TaiKhoan WHERE MaNV = @maNV";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maNV", maNV);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa tài khoản: {ex.Message}");
            }
        }

        public bool CapNhatThongTinTaiKhoan(TaiKhoan taiKhoan)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        UPDATE TaiKhoan 
                        SET TenDangNhap = @tenDangNhap, MatKhau = @matKhau, VaiTro = @vaiTro 
                        WHERE MaTK = @maTK";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tenDangNhap", taiKhoan.TenDangNhap);
                        command.Parameters.AddWithValue("@matKhau", taiKhoan.MatKhau);
                        command.Parameters.AddWithValue("@vaiTro", taiKhoan.VaiTro);
                        command.Parameters.AddWithValue("@maTK", taiKhoan.MaTK);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật thông tin tài khoản: {ex.Message}");
            }
        }

        public TaiKhoan? LayTaiKhoanTheoMaNV(int maNV)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT tk.MaTK, tk.TenDangNhap, tk.MatKhau, tk.VaiTro, tk.MaNV, nv.TenNV
                        FROM TaiKhoan tk
                        INNER JOIN NhanVien nv ON tk.MaNV = nv.MaNV
                        WHERE tk.MaNV = @maNV";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maNV", maNV);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TaiKhoan
                                {
                                    MaTK = reader.GetInt32("MaTK"),
                                    TenDangNhap = reader.GetString("TenDangNhap"),
                                    MatKhau = reader.GetString("MatKhau"),
                                    VaiTro = reader.GetString("VaiTro"),
                                    MaNV = reader.GetInt32("MaNV"),
                                    TenNV = reader.GetString("TenNV")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin tài khoản: {ex.Message}");
            }
            return null;
        }
    }
}
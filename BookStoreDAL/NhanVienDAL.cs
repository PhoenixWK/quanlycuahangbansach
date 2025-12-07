using BookStoreDTO;
using MySql.Data.MySqlClient;

namespace BookStoreDAL
{
    public class NhanVienDAL
    {
        public List<NhanVienDTO> LayDanhSachNhanVien()
        {
            List<NhanVienDTO> danhSach = new List<NhanVienDTO>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT nv.MaNV, nv.TenNV, nv.SDT, nv.DiaChi, nv.NgayVaoLam, 
                                    tk.TenDangNhap, tk.VaiTro
                                    FROM NhanVien nv
                                    LEFT JOIN TaiKhoan tk ON nv.MaNV = tk.MaNV
                                    ORDER BY nv.MaNV";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var nhanVien = new NhanVienDTO
                                {
                                    MaNV = reader.GetInt32(0), // MaNV
                                    TenNV = reader.GetString(1), // TenNV
                                    SDT = reader.IsDBNull(2) ? null : reader.GetString(2), // SDT
                                    DiaChi = reader.IsDBNull(3) ? null : reader.GetString(3), // DiaChi
                                    NgayVaoLam = reader.GetDateTime(4), // NgayVaoLam
                                    TenDangNhap = reader.IsDBNull(5) ? null : reader.GetString(5), // TenDangNhap
                                    VaiTro = reader.IsDBNull(6) ? null : reader.GetString(6) // VaiTro
                                };
                                
                                danhSach.Add(nhanVien);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách nhân viên: " + ex.Message);
            }

            return danhSach;
        }

        public bool ThemNhanVien(NhanVienDTO nhanVien)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO NhanVien (TenNV, SDT, DiaChi, NgayVaoLam) 
                                    VALUES (@TenNV, @SDT, @DiaChi, @NgayVaoLam)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TenNV", nhanVien.TenNV);
                        command.Parameters.AddWithValue("@SDT", nhanVien.SDT ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@DiaChi", nhanVien.DiaChi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@NgayVaoLam", nhanVien.NgayVaoLam);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm nhân viên: " + ex.Message);
            }
        }

        public bool CapNhatNhanVien(NhanVienDTO nhanVien)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"UPDATE NhanVien 
                                    SET TenNV = @TenNV, SDT = @SDT, DiaChi = @DiaChi, NgayVaoLam = @NgayVaoLam
                                    WHERE MaNV = @MaNV";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaNV", nhanVien.MaNV);
                        command.Parameters.AddWithValue("@TenNV", nhanVien.TenNV);
                        command.Parameters.AddWithValue("@SDT", nhanVien.SDT ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@DiaChi", nhanVien.DiaChi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@NgayVaoLam", nhanVien.NgayVaoLam);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật nhân viên: " + ex.Message);
            }
        }

        public bool XoaNhanVien(int maNV)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    // Kiểm tra xem nhân viên có tài khoản không
                    string checkQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE MaNV = @MaNV";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@MaNV", maNV);
                        int accountCount = Convert.ToInt32(checkCommand.ExecuteScalar());
                        
                        if (accountCount > 0)
                        {
                            throw new Exception("Không thể xóa nhân viên vì còn tài khoản liên kết!");
                        }
                    }

                    string query = "DELETE FROM NhanVien WHERE MaNV = @MaNV";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaNV", maNV);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa nhân viên: " + ex.Message);
            }
        }

        public List<NhanVienDTO> TimKiemNhanVien(string tuKhoa)
        {
            List<NhanVienDTO> danhSach = new List<NhanVienDTO>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT nv.MaNV, nv.TenNV, nv.SDT, nv.DiaChi, nv.NgayVaoLam, 
                                    tk.TenDangNhap, tk.VaiTro
                                    FROM NhanVien nv
                                    LEFT JOIN TaiKhoan tk ON nv.MaNV = tk.MaNV
                                    WHERE nv.TenNV LIKE @TuKhoa OR nv.SDT LIKE @TuKhoa
                                    ORDER BY nv.MaNV";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TuKhoa", $"%{tuKhoa}%");

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var nhanVien = new NhanVienDTO
                                {
                                    MaNV = reader.GetInt32(0), // MaNV
                                    TenNV = reader.GetString(1), // TenNV
                                    SDT = reader.IsDBNull(2) ? null : reader.GetString(2), // SDT
                                    DiaChi = reader.IsDBNull(3) ? null : reader.GetString(3), // DiaChi
                                    NgayVaoLam = reader.GetDateTime(4), // NgayVaoLam
                                    TenDangNhap = reader.IsDBNull(5) ? null : reader.GetString(5), // TenDangNhap
                                    VaiTro = reader.IsDBNull(6) ? null : reader.GetString(6) // VaiTro
                                };
                                
                                danhSach.Add(nhanVien);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm nhân viên: " + ex.Message);
            }

            return danhSach;
        }
    }
}
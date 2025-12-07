using BookStoreDTO;
using MySql.Data.MySqlClient;

namespace BookStoreDAL
{
    public class KhachHangDAL
    {
        public List<KhachHangDTO> LayDanhSachKhachHang()
        {
            List<KhachHangDTO> danhSach = new List<KhachHangDTO>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    
                    // Try new structure first, fallback to old structure
                    bool useNewStructure = CheckNewStructure(connection);
                    string query;
                    
                    if (useNewStructure)
                    {
                        query = @"SELECT MaKH, TenKH, Email, SoDienThoai, DiaChi, NgayTao, NgayCapNhat 
                                 FROM KhachHang 
                                 ORDER BY NgayTao DESC";
                    }
                    else
                    {
                        query = @"SELECT MaKH, TenKH, SDT, DiaChi 
                                 FROM KhachHang 
                                 ORDER BY MaKH DESC";
                    }

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var khachHang = new KhachHangDTO();
                                
                                if (useNewStructure)
                                {
                                    khachHang.MaKH = reader.GetInt32(0); // MaKH
                                    khachHang.TenKH = reader.GetString(1); // TenKH
                                    khachHang.Email = reader.IsDBNull(2) ? null : reader.GetString(2); // Email
                                    khachHang.SoDienThoai = reader.IsDBNull(3) ? null : reader.GetString(3); // SoDienThoai
                                    khachHang.DiaChi = reader.IsDBNull(4) ? null : reader.GetString(4); // DiaChi
                                    khachHang.NgayTao = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5); // NgayTao
                                    khachHang.NgayCapNhat = reader.IsDBNull(6) ? null : reader.GetDateTime(6); // NgayCapNhat
                                }
                                else
                                {
                                    // Old structure mapping
                                    khachHang.MaKH = reader.GetInt32(0); // MaKH
                                    khachHang.TenKH = reader.GetString(1); // TenKH
                                    khachHang.SoDienThoai = reader.IsDBNull(2) ? null : reader.GetString(2); // SDT -> SoDienThoai
                                    khachHang.DiaChi = reader.IsDBNull(3) ? null : reader.GetString(3); // DiaChi
                                    khachHang.Email = null; // Not available in old structure
                                    khachHang.NgayTao = DateTime.Now; // Default value
                                    khachHang.NgayCapNhat = null;
                                }

                                danhSach.Add(khachHang);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách khách hàng: " + ex.Message);
            }

            return danhSach;
        }

        public List<KhachHangDTO> TimKiemKhachHang(string searchTerm)
        {
            List<KhachHangDTO> danhSach = new List<KhachHangDTO>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    
                    bool useNewStructure = CheckNewStructure(connection);
                    string query;
                    
                    if (useNewStructure)
                    {
                        query = @"SELECT MaKH, TenKH, Email, SoDienThoai, DiaChi, NgayTao, NgayCapNhat 
                                 FROM KhachHang 
                                 WHERE TenKH LIKE @search 
                                    OR Email LIKE @search 
                                    OR SoDienThoai LIKE @search 
                                    OR DiaChi LIKE @search
                                 ORDER BY NgayTao DESC";
                    }
                    else
                    {
                        query = @"SELECT MaKH, TenKH, SDT, DiaChi 
                                 FROM KhachHang 
                                 WHERE TenKH LIKE @search 
                                    OR SDT LIKE @search 
                                    OR DiaChi LIKE @search
                                 ORDER BY MaKH DESC";
                    }

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var khachHang = new KhachHangDTO();
                                
                                if (useNewStructure)
                                {
                                    khachHang.MaKH = reader.GetInt32(0); // MaKH
                                    khachHang.TenKH = reader.GetString(1); // TenKH
                                    khachHang.Email = reader.IsDBNull(2) ? null : reader.GetString(2); // Email
                                    khachHang.SoDienThoai = reader.IsDBNull(3) ? null : reader.GetString(3); // SoDienThoai
                                    khachHang.DiaChi = reader.IsDBNull(4) ? null : reader.GetString(4); // DiaChi
                                    khachHang.NgayTao = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5); // NgayTao
                                    khachHang.NgayCapNhat = reader.IsDBNull(6) ? null : reader.GetDateTime(6); // NgayCapNhat
                                }
                                else
                                {
                                    // Old structure mapping
                                    khachHang.MaKH = reader.GetInt32(0); // MaKH
                                    khachHang.TenKH = reader.GetString(1); // TenKH
                                    khachHang.SoDienThoai = reader.IsDBNull(2) ? null : reader.GetString(2); // SDT -> SoDienThoai
                                    khachHang.DiaChi = reader.IsDBNull(3) ? null : reader.GetString(3); // DiaChi
                                    khachHang.Email = null; // Not available in old structure
                                    khachHang.NgayTao = DateTime.Now; // Default value
                                    khachHang.NgayCapNhat = null;
                                }

                                danhSach.Add(khachHang);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm khách hàng: " + ex.Message);
            }

            return danhSach;
        }

        public bool ThemKhachHang(KhachHangDTO khachHang)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    
                    bool useNewStructure = CheckNewStructure(connection);
                    string query;
                    
                    if (useNewStructure)
                    {
                        query = @"INSERT INTO KhachHang (TenKH, Email, SoDienThoai, DiaChi, NgayTao) 
                                 VALUES (@tenKH, @email, @soDienThoai, @diaChi, @ngayTao)";
                    }
                    else
                    {
                        query = @"INSERT INTO KhachHang (TenKH, SDT, DiaChi) 
                                 VALUES (@tenKH, @soDienThoai, @diaChi)";
                    }

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tenKH", khachHang.TenKH);
                        command.Parameters.AddWithValue("@soDienThoai", khachHang.SoDienThoai);
                        command.Parameters.AddWithValue("@diaChi", khachHang.DiaChi);
                        
                        if (useNewStructure)
                        {
                            command.Parameters.AddWithValue("@email", khachHang.Email);
                            command.Parameters.AddWithValue("@ngayTao", khachHang.NgayTao);
                        }

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm khách hàng: " + ex.Message);
            }
        }

        public bool CapNhatKhachHang(KhachHangDTO khachHang)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    
                    bool useNewStructure = CheckNewStructure(connection);
                    string query;
                    
                    if (useNewStructure)
                    {
                        query = @"UPDATE KhachHang 
                                 SET TenKH = @tenKH, 
                                     Email = @email, 
                                     SoDienThoai = @soDienThoai, 
                                     DiaChi = @diaChi, 
                                     NgayCapNhat = @ngayCapNhat 
                                 WHERE MaKH = @maKH";
                    }
                    else
                    {
                        query = @"UPDATE KhachHang 
                                 SET TenKH = @tenKH, 
                                     SDT = @soDienThoai, 
                                     DiaChi = @diaChi 
                                 WHERE MaKH = @maKH";
                    }

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maKH", khachHang.MaKH);
                        command.Parameters.AddWithValue("@tenKH", khachHang.TenKH);
                        command.Parameters.AddWithValue("@soDienThoai", khachHang.SoDienThoai);
                        command.Parameters.AddWithValue("@diaChi", khachHang.DiaChi);
                        
                        if (useNewStructure)
                        {
                            command.Parameters.AddWithValue("@email", khachHang.Email);
                            command.Parameters.AddWithValue("@ngayCapNhat", DateTime.Now);
                        }

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật khách hàng: " + ex.Message);
            }
        }

        public bool XoaKhachHang(int maKH)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    // Check if customer has any orders first
                    string checkQuery = "SELECT COUNT(*) FROM HoaDon WHERE MaKH = @maKH";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@maKH", maKH);
                        int orderCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (orderCount > 0)
                        {
                            throw new Exception("Không thể xóa khách hàng đã có đơn hàng!");
                        }
                    }

                    string deleteQuery = "DELETE FROM KhachHang WHERE MaKH = @maKH";
                    using (var deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@maKH", maKH);
                        return deleteCommand.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa khách hàng: " + ex.Message);
            }
        }

        public KhachHangDTO? LayKhachHangTheoMa(int maKH)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    
                    bool useNewStructure = CheckNewStructure(connection);
                    string query;
                    
                    if (useNewStructure)
                    {
                        query = @"SELECT MaKH, TenKH, Email, SoDienThoai, DiaChi, NgayTao, NgayCapNhat 
                                 FROM KhachHang 
                                 WHERE MaKH = @maKH";
                    }
                    else
                    {
                        query = @"SELECT MaKH, TenKH, SDT, DiaChi 
                                 FROM KhachHang 
                                 WHERE MaKH = @maKH";
                    }

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maKH", maKH);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var khachHang = new KhachHangDTO();
                                
                                if (useNewStructure)
                                {
                                    khachHang.MaKH = reader.GetInt32(0); // MaKH
                                    khachHang.TenKH = reader.GetString(1); // TenKH
                                    khachHang.Email = reader.IsDBNull(2) ? null : reader.GetString(2); // Email
                                    khachHang.SoDienThoai = reader.IsDBNull(3) ? null : reader.GetString(3); // SoDienThoai
                                    khachHang.DiaChi = reader.IsDBNull(4) ? null : reader.GetString(4); // DiaChi
                                    khachHang.NgayTao = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5); // NgayTao
                                    khachHang.NgayCapNhat = reader.IsDBNull(6) ? null : reader.GetDateTime(6); // NgayCapNhat
                                }
                                else
                                {
                                    // Old structure mapping
                                    khachHang.MaKH = reader.GetInt32(0); // MaKH
                                    khachHang.TenKH = reader.GetString(1); // TenKH
                                    khachHang.SoDienThoai = reader.IsDBNull(2) ? null : reader.GetString(2); // SDT -> SoDienThoai
                                    khachHang.DiaChi = reader.IsDBNull(3) ? null : reader.GetString(3); // DiaChi
                                    khachHang.Email = null; // Not available in old structure
                                    khachHang.NgayTao = DateTime.Now; // Default value
                                    khachHang.NgayCapNhat = null;
                                }

                                return khachHang;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin khách hàng: " + ex.Message);
            }

            return null;
        }

        public bool KiemTraEmailTonTai(string email, int? excludeId = null)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    
                    // Only check email if new structure is available
                    if (!CheckNewStructure(connection))
                    {
                        return false; // Email column doesn't exist in old structure
                    }
                    
                    string query = "SELECT COUNT(*) FROM KhachHang WHERE Email = @email";

                    if (excludeId.HasValue)
                    {
                        query += " AND MaKH != @excludeId";
                    }

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        if (excludeId.HasValue)
                        {
                            command.Parameters.AddWithValue("@excludeId", excludeId.Value);
                        }

                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch
            {
                // If error checking email, assume it doesn't exist
                return false;
            }
        }

        private bool CheckNewStructure(MySqlConnection connection)
        {
            try
            {
                string checkQuery = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
                                     WHERE TABLE_SCHEMA = DATABASE() 
                                     AND TABLE_NAME = 'KhachHang' 
                                     AND COLUMN_NAME = 'Email'";
                
                using (var command = new MySqlCommand(checkQuery, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
            catch
            {
                return false; // Assume old structure if check fails
            }
        }
    }
}
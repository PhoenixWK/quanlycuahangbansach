using BookStoreDTO;
using MySql.Data.MySqlClient;

namespace BookStoreDAL
{
    public class TheLoaiDAL
    {
        public List<TheLoaiDTO> LayDanhSachTheLoai()
        {
            List<TheLoaiDTO> danhSach = new List<TheLoaiDTO>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT MaTL, TenTL 
                                    FROM TheLoai 
                                    ORDER BY TenTL";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var theLoai = new TheLoaiDTO
                                {
                                    MaTL = reader.GetInt32(0), // MaTL
                                    TenTL = reader.GetString(1) // TenTL
                                };

                                danhSach.Add(theLoai);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách thể loại: " + ex.Message);
            }

            return danhSach;
        }

        public List<TheLoaiDTO> TimKiemTheLoai(string searchTerm)
        {
            List<TheLoaiDTO> danhSach = new List<TheLoaiDTO>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT MaTL, TenTL 
                                    FROM TheLoai 
                                    WHERE TenTL LIKE @search
                                    ORDER BY TenTL";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var theLoai = new TheLoaiDTO
                                {
                                    MaTL = reader.GetInt32(0), // MaTL
                                    TenTL = reader.GetString(1) // TenTL
                                };

                                danhSach.Add(theLoai);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm thể loại: " + ex.Message);
            }

            return danhSach;
        }

        public bool ThemTheLoai(TheLoaiDTO theLoai)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO TheLoai (TenTL) VALUES (@tenTL)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tenTL", theLoai.TenTL);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm thể loại: " + ex.Message);
            }
        }

        public bool CapNhatTheLoai(TheLoaiDTO theLoai)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"UPDATE TheLoai 
                                    SET TenTL = @tenTL 
                                    WHERE MaTL = @maTL";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maTL", theLoai.MaTL);
                        command.Parameters.AddWithValue("@tenTL", theLoai.TenTL);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật thể loại: " + ex.Message);
            }
        }

        public bool XoaTheLoai(int maTL)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    // Check if category has any books first
                    string checkQuery = "SELECT COUNT(*) FROM Sach WHERE MaTL = @maTL";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@maTL", maTL);
                        int bookCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (bookCount > 0)
                        {
                            throw new Exception("Không thể xóa thể loại đã có sách trong hệ thống!");
                        }
                    }

                    string deleteQuery = "DELETE FROM TheLoai WHERE MaTL = @maTL";
                    using (var deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@maTL", maTL);
                        return deleteCommand.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa thể loại: " + ex.Message);
            }
        }

        public TheLoaiDTO? LayTheLoaiTheoMa(int maTL)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT MaTL, TenTL 
                                    FROM TheLoai 
                                    WHERE MaTL = @maTL";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maTL", maTL);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TheLoaiDTO
                                {
                                    MaTL = reader.GetInt32(0), // MaTL
                                    TenTL = reader.GetString(1) // TenTL
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin thể loại: " + ex.Message);
            }

            return null;
        }

        public bool KiemTraTenTheLoaiTonTai(string tenTL, int? excludeId = null)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM TheLoai WHERE TenTL = @tenTL";

                    if (excludeId.HasValue)
                    {
                        query += " AND MaTL != @excludeId";
                    }

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tenTL", tenTL);
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
                return false;
            }
        }
    }
}
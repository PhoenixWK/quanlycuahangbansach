using BookStoreDTO;
using MySql.Data.MySqlClient;

namespace BookStoreDAL
{
    public class TacGiaDAL
    {
        public List<TacGiaDTO> LayDanhSachTacGia()
        {
            List<TacGiaDTO> danhSach = new List<TacGiaDTO>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT MaTG, TenTG 
                                    FROM TacGia 
                                    ORDER BY TenTG";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var tacGia = new TacGiaDTO
                                {
                                    MaTG = reader.GetInt32(0), // MaTG
                                    TenTG = reader.GetString(1) // TenTG
                                };

                                danhSach.Add(tacGia);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tác giả: " + ex.Message);
            }

            return danhSach;
        }

        public List<TacGiaDTO> TimKiemTacGia(string searchTerm)
        {
            List<TacGiaDTO> danhSach = new List<TacGiaDTO>();

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT MaTG, TenTG 
                                    FROM TacGia 
                                    WHERE TenTG LIKE @search
                                    ORDER BY TenTG";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var tacGia = new TacGiaDTO
                                {
                                    MaTG = reader.GetInt32(0), // MaTG
                                    TenTG = reader.GetString(1) // TenTG
                                };

                                danhSach.Add(tacGia);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm tác giả: " + ex.Message);
            }

            return danhSach;
        }

        public bool ThemTacGia(TacGiaDTO tacGia)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO TacGia (TenTG) VALUES (@tenTG)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tenTG", tacGia.TenTG);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm tác giả: " + ex.Message);
            }
        }

        public bool CapNhatTacGia(TacGiaDTO tacGia)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"UPDATE TacGia 
                                    SET TenTG = @tenTG 
                                    WHERE MaTG = @maTG";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maTG", tacGia.MaTG);
                        command.Parameters.AddWithValue("@tenTG", tacGia.TenTG);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật tác giả: " + ex.Message);
            }
        }

        public bool XoaTacGia(int maTG)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    // Check if author has any books first
                    string checkQuery = "SELECT COUNT(*) FROM Sach WHERE MaTG = @maTG";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@maTG", maTG);
                        int bookCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (bookCount > 0)
                        {
                            throw new Exception("Không thể xóa tác giả đã có sách trong hệ thống!");
                        }
                    }

                    string deleteQuery = "DELETE FROM TacGia WHERE MaTG = @maTG";
                    using (var deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@maTG", maTG);
                        return deleteCommand.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa tác giả: " + ex.Message);
            }
        }

        public TacGiaDTO? LayTacGiaTheoMa(int maTG)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT MaTG, TenTG 
                                    FROM TacGia 
                                    WHERE MaTG = @maTG";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maTG", maTG);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TacGiaDTO
                                {
                                    MaTG = reader.GetInt32(0), // MaTG
                                    TenTG = reader.GetString(1) // TenTG
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin tác giả: " + ex.Message);
            }

            return null;
        }

        public bool KiemTraTenTacGiaTonTai(string tenTG, int? excludeId = null)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM TacGia WHERE TenTG = @tenTG";

                    if (excludeId.HasValue)
                    {
                        query += " AND MaTG != @excludeId";
                    }

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tenTG", tenTG);
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
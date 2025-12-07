using BookStoreDTO;
using MySql.Data.MySqlClient;
using System.Data;

namespace BookStoreDAL
{
    public class SachDAL
    {
        public List<SachDTO> LayDanhSachSach()
        {
            List<SachDTO> danhSach = new List<SachDTO>();
            
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.MaSach, s.TenSach, s.MaTG, tg.TenTG as TenTacGia, 
                                   s.MaTL, tl.TenTL as TenTheLoai, s.GiaBan, s.SoLuongTon, s.NhaXuatBan
                                   FROM Sach s 
                                   LEFT JOIN TacGia tg ON s.MaTG = tg.MaTG 
                                   LEFT JOIN TheLoai tl ON s.MaTL = tl.MaTL";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SachDTO sach = new SachDTO
                                {
                                    MaSach = reader["MaSach"].ToString(),
                                    TenSach = reader["TenSach"].ToString(),
                                    MaTG = reader["MaTG"].ToString(),
                                    TenTacGia = reader["TenTacGia"].ToString(),
                                    MaTL = reader["MaTL"].ToString(),
                                    TenTheLoai = reader["TenTheLoai"].ToString(),
                                    GiaBan = reader["GiaBan"] != DBNull.Value ? Convert.ToDecimal(reader["GiaBan"]) : 0,
                                    SoLuongTon = reader["SoLuongTon"] != DBNull.Value ? Convert.ToInt32(reader["SoLuongTon"]) : 0,
                                    TrangThai = true, // Default value since TrangThai doesn't exist in database
                                    // Set default values for fields that might not exist in database
                                    MoTa = "",
                                    HinhAnh = "",
                                    NgayXuatBan = null,
                                    NhaXuatBan = reader["NhaXuatBan"]?.ToString() ?? "",
                                    SoTrang = 0
                                };
                                danhSach.Add(sach);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy danh sách sách: " + ex.Message);
                }
            }
            
            return danhSach;
        }

        public List<SachDTO> TimKiemSach(string tuKhoa)
        {
            List<SachDTO> danhSach = new List<SachDTO>();
            
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.MaSach, s.TenSach, s.MaTG, tg.TenTG as TenTacGia, 
                                   s.MaTL, tl.TenTL as TenTheLoai, s.GiaBan, s.SoLuongTon, s.NhaXuatBan
                                   FROM Sach s 
                                   LEFT JOIN TacGia tg ON s.MaTG = tg.MaTG 
                                   LEFT JOIN TheLoai tl ON s.MaTL = tl.MaTL
                                   WHERE (s.TenSach LIKE @tuKhoa OR tg.TenTG LIKE @tuKhoa OR tl.TenTL LIKE @tuKhoa OR s.MaSach LIKE @tuKhoa)";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tuKhoa", "%" + tuKhoa + "%");
                        
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SachDTO sach = new SachDTO
                                {
                                    MaSach = reader["MaSach"].ToString(),
                                    TenSach = reader["TenSach"].ToString(),
                                    MaTG = reader["MaTG"].ToString(),
                                    TenTacGia = reader["TenTacGia"].ToString(),
                                    MaTL = reader["MaTL"].ToString(),
                                    TenTheLoai = reader["TenTheLoai"].ToString(),
                                    GiaBan = reader["GiaBan"] != DBNull.Value ? Convert.ToDecimal(reader["GiaBan"]) : 0,
                                    SoLuongTon = reader["SoLuongTon"] != DBNull.Value ? Convert.ToInt32(reader["SoLuongTon"]) : 0,
                                    TrangThai = true, // Default value since TrangThai doesn't exist in database
                                    // Set default values for fields that might not exist in database
                                    MoTa = "",
                                    HinhAnh = "",
                                    NgayXuatBan = null,
                                    NhaXuatBan = reader["NhaXuatBan"]?.ToString() ?? "",
                                    SoTrang = 0
                                };
                                danhSach.Add(sach);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi tìm kiếm sách: " + ex.Message);
                }
            }
            
            return danhSach;
        }

        public bool ThemSach(SachDTO sach)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO Sach (MaSach, TenSach, MaTG, MaTL, GiaBan, SoLuongTon, NhaXuatBan) 
                                   VALUES (@maSach, @tenSach, @maTG, @maTL, @giaBan, @soLuongTon, @nhaXuatBan)";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maSach", sach.MaSach);
                        command.Parameters.AddWithValue("@tenSach", sach.TenSach);
                        command.Parameters.AddWithValue("@maTG", sach.MaTG);
                        command.Parameters.AddWithValue("@maTL", sach.MaTL);
                        command.Parameters.AddWithValue("@giaBan", sach.GiaBan);
                        command.Parameters.AddWithValue("@soLuongTon", sach.SoLuongTon);
                        command.Parameters.AddWithValue("@nhaXuatBan", sach.NhaXuatBan ?? "");
                        
                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi thêm sách: " + ex.Message);
                }
            }
        }

        public bool CapNhatSach(SachDTO sach)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE Sach SET TenSach = @tenSach, MaTG = @maTG, MaTL = @maTL, GiaBan = @giaBan, 
                                   SoLuongTon = @soLuongTon, NhaXuatBan = @nhaXuatBan 
                                   WHERE MaSach = @maSach";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maSach", sach.MaSach);
                        command.Parameters.AddWithValue("@tenSach", sach.TenSach);
                        command.Parameters.AddWithValue("@maTG", sach.MaTG);
                        command.Parameters.AddWithValue("@maTL", sach.MaTL);
                        command.Parameters.AddWithValue("@giaBan", sach.GiaBan);
                        command.Parameters.AddWithValue("@soLuongTon", sach.SoLuongTon);
                        command.Parameters.AddWithValue("@nhaXuatBan", sach.NhaXuatBan ?? "");
                        
                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi cập nhật sách: " + ex.Message);
                }
            }
        }

        public bool XoaSach(string maSach)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    
                    // Check if book is used in any order (ChiTietHD table)
                    string checkQuery = "SELECT COUNT(*) FROM ChiTietHD WHERE MaSach = @maSach";
                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@maSach", maSach);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                        
                        if (count > 0)
                        {
                            throw new Exception("Không thể xóa sách này vì đã có trong hóa đơn! Sách này đã được bán trong " + count + " đơn hàng.");
                        }
                    }
                    
                    // If no references found, proceed with deletion
                    string query = "DELETE FROM Sach WHERE MaSach = @maSach";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maSach", maSach);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi xóa sách: " + ex.Message);
                }
            }
        }

        public bool KiemTraMaSachTonTai(string maSach)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Sach WHERE MaSach = @maSach";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maSach", maSach);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi kiểm tra mã sách: " + ex.Message);
                }
            }
        }

        public SachDTO? LaySachTheoMa(string maSach)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.MaSach, s.TenSach, s.MaTG, tg.TenTG as TenTacGia, 
                                   s.MaTL, tl.TenTL as TenTheLoai, s.GiaBan, s.SoLuongTon, s.NhaXuatBan
                                   FROM Sach s 
                                   LEFT JOIN TacGia tg ON s.MaTG = tg.MaTG 
                                   LEFT JOIN TheLoai tl ON s.MaTL = tl.MaTL
                                   WHERE s.MaSach = @maSach";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maSach", maSach);
                        
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new SachDTO
                                {
                                    MaSach = reader["MaSach"].ToString(),
                                    TenSach = reader["TenSach"].ToString(),
                                    MaTG = reader["MaTG"].ToString(),
                                    TenTacGia = reader["TenTacGia"].ToString(),
                                    MaTL = reader["MaTL"].ToString(),
                                    TenTheLoai = reader["TenTheLoai"].ToString(),
                                    GiaBan = reader["GiaBan"] != DBNull.Value ? Convert.ToDecimal(reader["GiaBan"]) : 0,
                                    SoLuongTon = reader["SoLuongTon"] != DBNull.Value ? Convert.ToInt32(reader["SoLuongTon"]) : 0,
                                    NhaXuatBan = reader["NhaXuatBan"]?.ToString() ?? "",
                                    TrangThai = true, // Default value since TrangThai doesn't exist in database
                                    // Set default values for fields that might not exist in database
                                    MoTa = "",
                                    HinhAnh = "",
                                    NgayXuatBan = null,
                                    SoTrang = 0
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy thông tin sách: " + ex.Message);
                }
            }
            
            return null;
        }
    }
}
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
                                   s.MaTL, tl.TenTL as TenTheLoai, s.GiaBan, s.SoLuongTon, s.NhaXuatBan,
                                   s.MoTa, s.HinhAnh, s.NgayXuatBan, s.SoTrang, s.TrangThai
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
                                    NhaXuatBan = reader["NhaXuatBan"]?.ToString() ?? "",
                                    MoTa = reader["MoTa"]?.ToString() ?? "",
                                    HinhAnh = reader["HinhAnh"]?.ToString() ?? "",
                                    NgayXuatBan = reader["NgayXuatBan"] != DBNull.Value ? Convert.ToDateTime(reader["NgayXuatBan"]) : (DateTime?)null,
                                    SoTrang = reader["SoTrang"] != DBNull.Value ? Convert.ToInt32(reader["SoTrang"]) : 0,
                                    TrangThai = reader["TrangThai"] != DBNull.Value ? Convert.ToBoolean(reader["TrangThai"]) : true
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
                                   s.MaTL, tl.TenTL as TenTheLoai, s.GiaBan, s.SoLuongTon, s.NhaXuatBan,
                                   s.MoTa, s.HinhAnh, s.NgayXuatBan, s.SoTrang, s.TrangThai
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
                                    NhaXuatBan = reader["NhaXuatBan"]?.ToString() ?? "",
                                    MoTa = reader["MoTa"]?.ToString() ?? "",
                                    HinhAnh = reader["HinhAnh"]?.ToString() ?? "",
                                    NgayXuatBan = reader["NgayXuatBan"] != DBNull.Value ? Convert.ToDateTime(reader["NgayXuatBan"]) : (DateTime?)null,
                                    SoTrang = reader["SoTrang"] != DBNull.Value ? Convert.ToInt32(reader["SoTrang"]) : 0,
                                    TrangThai = reader["TrangThai"] != DBNull.Value ? Convert.ToBoolean(reader["TrangThai"]) : true
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
                    string query = @"INSERT INTO Sach (MaSach, TenSach, MaTG, MaTL, GiaBan, SoLuongTon, 
                                   NhaXuatBan, MoTa, HinhAnh, NgayXuatBan, SoTrang, TrangThai) 
                                   VALUES (@MaSach, @TenSach, @MaTG, @MaTL, @GiaBan, @SoLuongTon, 
                                   @NhaXuatBan, @MoTa, @HinhAnh, @NgayXuatBan, @SoTrang, @TrangThai)";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaSach", sach.MaSach);
                        command.Parameters.AddWithValue("@TenSach", sach.TenSach);
                        command.Parameters.AddWithValue("@MaTG", sach.MaTG);
                        command.Parameters.AddWithValue("@MaTL", sach.MaTL);
                        command.Parameters.AddWithValue("@GiaBan", sach.GiaBan);
                        command.Parameters.AddWithValue("@SoLuongTon", sach.SoLuongTon);
                        command.Parameters.AddWithValue("@NhaXuatBan", sach.NhaXuatBan ?? "");
                        command.Parameters.AddWithValue("@MoTa", sach.MoTa ?? "");
                        command.Parameters.AddWithValue("@HinhAnh", sach.HinhAnh ?? "");
                        command.Parameters.AddWithValue("@NgayXuatBan", sach.NgayXuatBan?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SoTrang", sach.SoTrang ?? 0);
                        command.Parameters.AddWithValue("@TrangThai", sach.TrangThai ?? true);
                        
                        int result = command.ExecuteNonQuery();
                        return result > 0;
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
                    string query = @"UPDATE Sach SET TenSach = @TenSach, MaTG = @MaTG, MaTL = @MaTL, 
                                   GiaBan = @GiaBan, SoLuongTon = @SoLuongTon, NhaXuatBan = @NhaXuatBan,
                                   MoTa = @MoTa, HinhAnh = @HinhAnh, NgayXuatBan = @NgayXuatBan, 
                                   SoTrang = @SoTrang, TrangThai = @TrangThai 
                                   WHERE MaSach = @MaSach";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaSach", sach.MaSach);
                        command.Parameters.AddWithValue("@TenSach", sach.TenSach);
                        command.Parameters.AddWithValue("@MaTG", sach.MaTG);
                        command.Parameters.AddWithValue("@MaTL", sach.MaTL);
                        command.Parameters.AddWithValue("@GiaBan", sach.GiaBan);
                        command.Parameters.AddWithValue("@SoLuongTon", sach.SoLuongTon);
                        command.Parameters.AddWithValue("@NhaXuatBan", sach.NhaXuatBan ?? "");
                        command.Parameters.AddWithValue("@MoTa", sach.MoTa ?? "");
                        command.Parameters.AddWithValue("@HinhAnh", sach.HinhAnh ?? "");
                        command.Parameters.AddWithValue("@NgayXuatBan", sach.NgayXuatBan?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SoTrang", sach.SoTrang ?? 0);
                        command.Parameters.AddWithValue("@TrangThai", sach.TrangThai ?? true);
                        
                        int result = command.ExecuteNonQuery();
                        return result > 0;
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
                    string query = "DELETE FROM Sach WHERE MaSach = @maSach";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maSach", maSach);
                        
                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi xóa sách: " + ex.Message);
                }
            }
        }

        public string TaoMaSachMoi()
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT MaSach FROM Sach ORDER BY MaSach DESC LIMIT 1";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string lastCode = reader["MaSach"].ToString() ?? "";
                                if (lastCode.StartsWith("S") && lastCode.Length == 4)
                                {
                                    if (int.TryParse(lastCode.Substring(1), out int lastNumber))
                                    {
                                        return $"S{(lastNumber + 1):D3}";
                                    }
                                }
                            }
                        }
                        
                        // Default starting code
                        return "S001";
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi tạo mã sách mới: " + ex.Message);
                }
            }
        }

        public SachDTO? LayThongTinSach(string maSach)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.MaSach, s.TenSach, s.MaTG, tg.TenTG as TenTacGia, 
                                   s.MaTL, tl.TenTL as TenTheLoai, s.GiaBan, s.SoLuongTon, s.NhaXuatBan,
                                   s.MoTa, s.HinhAnh, s.NgayXuatBan, s.SoTrang, s.TrangThai
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
                                    MoTa = reader["MoTa"]?.ToString() ?? "",
                                    HinhAnh = reader["HinhAnh"]?.ToString() ?? "",
                                    NgayXuatBan = reader["NgayXuatBan"] != DBNull.Value ? Convert.ToDateTime(reader["NgayXuatBan"]) : (DateTime?)null,
                                    SoTrang = reader["SoTrang"] != DBNull.Value ? Convert.ToInt32(reader["SoTrang"]) : 0,
                                    TrangThai = reader["TrangThai"] != DBNull.Value ? Convert.ToBoolean(reader["TrangThai"]) : true
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

        public SachDTO LaySachTheoMa(string maSach)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.MaSach, s.TenSach, s.MaTG, tg.TenTG as TenTacGia, 
                                   s.MaTL, tl.TenTL as TenTheLoai, s.GiaBan, s.SoLuongTon, s.NhaXuatBan,
                                   s.MoTa, s.HinhAnh, s.NgayXuatBan, s.SoTrang, s.TrangThai
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
                                    MoTa = reader["MoTa"]?.ToString() ?? "",
                                    HinhAnh = reader["HinhAnh"]?.ToString() ?? "",
                                    NgayXuatBan = reader["NgayXuatBan"] != DBNull.Value ? Convert.ToDateTime(reader["NgayXuatBan"]) : (DateTime?)null,
                                    SoTrang = reader["SoTrang"] != DBNull.Value ? Convert.ToInt32(reader["SoTrang"]) : 0,
                                    TrangThai = reader["TrangThai"] != DBNull.Value ? Convert.ToBoolean(reader["TrangThai"]) : true
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy sách theo mã: " + ex.Message);
                }
            }
            
            return null!;
        }
    }
}
using MySql.Data.MySqlClient;
using BookStoreDTO;
using System;
using System.Collections.Generic;

namespace BookStoreDAL
{
    public class HoaDonDAL
    {
        public int ThemHoaDon(HoaDonDTO hoaDon)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO HoaDon (MaNV, MaKH, NgayBan, TongTien) 
                                   VALUES (@maNV, @maKH, @ngayBan, @tongTien);
                                   SELECT LAST_INSERT_ID();";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maNV", hoaDon.MaNV);
                        command.Parameters.AddWithValue("@maKH", hoaDon.MaKH);
                        command.Parameters.AddWithValue("@ngayBan", hoaDon.NgayBan);
                        command.Parameters.AddWithValue("@tongTien", hoaDon.TongTien);
                        
                        var result = command.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi thêm hóa đơn: " + ex.Message);
                }
            }
        }

        public List<HoaDonDTO> LayDanhSachHoaDon()
        {
            List<HoaDonDTO> danhSach = new List<HoaDonDTO>();
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT hd.MaHD, hd.NgayBan, hd.TongTien, hd.MaNV, hd.MaKH,
                                           nv.TenNV, kh.TenKH
                                   FROM HoaDon hd 
                                   LEFT JOIN NhanVien nv ON hd.MaNV = nv.MaNV 
                                   LEFT JOIN KhachHang kh ON hd.MaKH = kh.MaKH
                                   ORDER BY hd.NgayBan DESC";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                HoaDonDTO hoaDon = new HoaDonDTO
                                {
                                    MaHD = reader["MaHD"] != DBNull.Value ? Convert.ToInt32(reader["MaHD"]) : 0,
                                    NgayBan = reader["NgayBan"] != DBNull.Value ? Convert.ToDateTime(reader["NgayBan"]) : DateTime.Now,
                                    TongTien = reader["TongTien"] != DBNull.Value ? Convert.ToDecimal(reader["TongTien"]) : 0,
                                    MaNV = reader["MaNV"] != DBNull.Value ? Convert.ToInt32(reader["MaNV"]) : 0,
                                    MaKH = reader["MaKH"] != DBNull.Value ? Convert.ToInt32(reader["MaKH"]) : 0,
                                    TenNV = reader["TenNV"]?.ToString(),
                                    TenKH = reader["TenKH"]?.ToString()
                                };
                                danhSach.Add(hoaDon);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy danh sách hóa đơn: " + ex.Message);
                }
            }
            return danhSach;
        }

        public HoaDonDTO? LayHoaDonTheoMa(int maHD)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT hd.MaHD, hd.NgayBan, hd.TongTien, hd.MaNV, hd.MaKH,
                                           nv.TenNV, kh.TenKH
                                   FROM HoaDon hd 
                                   LEFT JOIN NhanVien nv ON hd.MaNV = nv.MaNV 
                                   LEFT JOIN KhachHang kh ON hd.MaKH = kh.MaKH
                                   WHERE hd.MaHD = @maHD";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maHD", maHD);
                        
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new HoaDonDTO
                                {
                                    MaHD = reader["MaHD"] != DBNull.Value ? Convert.ToInt32(reader["MaHD"]) : 0,
                                    NgayBan = reader["NgayBan"] != DBNull.Value ? Convert.ToDateTime(reader["NgayBan"]) : DateTime.Now,
                                    TongTien = reader["TongTien"] != DBNull.Value ? Convert.ToDecimal(reader["TongTien"]) : 0,
                                    MaNV = reader["MaNV"] != DBNull.Value ? Convert.ToInt32(reader["MaNV"]) : 0,
                                    MaKH = reader["MaKH"] != DBNull.Value ? Convert.ToInt32(reader["MaKH"]) : 0,
                                    TenNV = reader["TenNV"]?.ToString(),
                                    TenKH = reader["TenKH"]?.ToString()
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy thông tin hóa đơn: " + ex.Message);
                }
            }
            return null;
        }

        public bool CapNhatHoaDon(HoaDonDTO hoaDon)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE HoaDon 
                                   SET MaNV = @maNV, MaKH = @maKH, NgayBan = @ngayBan, TongTien = @tongTien
                                   WHERE MaHD = @maHD";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maNV", hoaDon.MaNV);
                        command.Parameters.AddWithValue("@maKH", hoaDon.MaKH);
                        command.Parameters.AddWithValue("@ngayBan", hoaDon.NgayBan);
                        command.Parameters.AddWithValue("@tongTien", hoaDon.TongTien);
                        command.Parameters.AddWithValue("@maHD", hoaDon.MaHD);
                        
                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi cập nhật hóa đơn: " + ex.Message);
                }
            }
        }

        public bool XoaHoaDon(int maHD)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM HoaDon WHERE MaHD = @maHD";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maHD", maHD);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi xóa hóa đơn: " + ex.Message);
                }
            }
        }
    }
}
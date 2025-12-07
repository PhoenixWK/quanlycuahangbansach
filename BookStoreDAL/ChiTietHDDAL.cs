using MySql.Data.MySqlClient;
using BookStoreDTO;
using System;
using System.Collections.Generic;

namespace BookStoreDAL
{
    public class ChiTietHDDAL
    {
        public bool ThemChiTietHD(ChiTietHDDTO chiTiet)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO ChiTietHD (MaHD, MaSach, SoLuong, DonGia) 
                                   VALUES (@maHD, @maSach, @soLuong, @donGia)";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maHD", chiTiet.MaHD);
                        command.Parameters.AddWithValue("@maSach", chiTiet.MaSach);
                        command.Parameters.AddWithValue("@soLuong", chiTiet.SoLuong);
                        command.Parameters.AddWithValue("@donGia", chiTiet.DonGia);
                        
                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi thêm chi tiết hóa đơn: " + ex.Message);
                }
            }
        }

        public List<ChiTietHDDTO> LayChiTietHoaDon(int maHD)
        {
            List<ChiTietHDDTO> danhSach = new List<ChiTietHDDTO>();
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT ct.MaCTHD, ct.SoLuong, ct.DonGia, ct.MaHD, ct.MaSach,
                                           s.TenSach
                                   FROM ChiTietHD ct 
                                   LEFT JOIN Sach s ON ct.MaSach = s.MaSach
                                   WHERE ct.MaHD = @maHD";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maHD", maHD);
                        
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ChiTietHDDTO chiTiet = new ChiTietHDDTO
                                {
                                    MaCTHD = reader["MaCTHD"] != DBNull.Value ? Convert.ToInt32(reader["MaCTHD"]) : 0,
                                    SoLuong = reader["SoLuong"] != DBNull.Value ? Convert.ToInt32(reader["SoLuong"]) : 0,
                                    DonGia = reader["DonGia"] != DBNull.Value ? Convert.ToDecimal(reader["DonGia"]) : 0,
                                    MaHD = reader["MaHD"] != DBNull.Value ? Convert.ToInt32(reader["MaHD"]) : 0,
                                    MaSach = reader["MaSach"]?.ToString(),
                                    TenSach = reader["TenSach"]?.ToString()
                                };
                                danhSach.Add(chiTiet);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy chi tiết hóa đơn: " + ex.Message);
                }
            }
            return danhSach;
        }

        public bool CapNhatChiTietHD(ChiTietHDDTO chiTiet)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE ChiTietHD 
                                   SET SoLuong = @soLuong, DonGia = @donGia
                                   WHERE MaCTHD = @maCTHD";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@soLuong", chiTiet.SoLuong);
                        command.Parameters.AddWithValue("@donGia", chiTiet.DonGia);
                        command.Parameters.AddWithValue("@maCTHD", chiTiet.MaCTHD);
                        
                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi cập nhật chi tiết hóa đơn: " + ex.Message);
                }
            }
        }

        public bool XoaChiTietHD(int maCTHD)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM ChiTietHD WHERE MaCTHD = @maCTHD";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maCTHD", maCTHD);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi xóa chi tiết hóa đơn: " + ex.Message);
                }
            }
        }
    }
}
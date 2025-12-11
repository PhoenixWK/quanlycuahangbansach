-- 1. Tạo Database (Tùy chọn, bạn có thể bỏ qua nếu đã có sẵn)
DROP DATABASE IF EXISTS QuanLyCuaHangBanSach;
CREATE DATABASE QuanLyCuaHangBanSach;
USE QuanLyCuaHangBanSach;

-- Thiết lập Charset và Collation cho tiếng Việt
SET NAMES 'utf8mb4';
SET CHARACTER SET utf8mb4;

-- #######################################################
-- I. Bảng Danh mục (Look-up Tables)
-- #######################################################

-- 2. Bảng Thể loại
CREATE TABLE TheLoai (
    MaTL INT PRIMARY KEY AUTO_INCREMENT,
    TenTL NVARCHAR(100) NOT NULL
);

-- 3. Bảng Tác giả
CREATE TABLE TacGia (
    MaTG INT PRIMARY KEY AUTO_INCREMENT,
    TenTG NVARCHAR(100) NOT NULL
);

-- 4. Bảng Khách hàng
CREATE TABLE KhachHang (
    MaKH INT PRIMARY KEY AUTO_INCREMENT,
    TenKH NVARCHAR(100) NOT NULL,
    Email VARCHAR(255) NULL,
    SoDienThoai VARCHAR(15) NULL,
    DiaChi NVARCHAR(255),
    NgayTao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    NgayCapNhat DATETIME NULL,
    
    -- Indexes for better performance
    INDEX idx_khachhang_email (Email),
    INDEX idx_khachhang_ngaytao (NgayTao),
    INDEX idx_khachhang_ten (TenKH),
    
    -- Unique constraint for email (if not null)
    UNIQUE KEY unique_email (Email)
);

-- #######################################################
-- II. Bảng Nhân sự & Sách
-- #######################################################

-- 5. Bảng Nhân viên
CREATE TABLE NhanVien (
    MaNV INT PRIMARY KEY AUTO_INCREMENT,
    TenNV NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15),
    DiaChi NVARCHAR(255),
    NgayVaoLam DATE NOT NULL
);

-- 6. Bảng Tài khoản (Phân quyền)
CREATE TABLE TaiKhoan (
    MaTK INT PRIMARY KEY AUTO_INCREMENT,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL, -- Dùng VARCHAR(255) để lưu hash mật khẩu
    VaiTro ENUM('Admin', 'NhanVien') NOT NULL, -- Chỉ chấp nhận 2 giá trị này
    MaNV INT NOT NULL,
    
    -- Ràng buộc Khóa ngoại liên kết với Nhân viên
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

-- 7. Bảng Sách (Danh mục sản phẩm)
CREATE TABLE Sach (
    MaSach VARCHAR(20) PRIMARY KEY,  -- Changed from INT AUTO_INCREMENT to VARCHAR(20) for custom book codes like 'S001'
    TenSach NVARCHAR(255) NOT NULL,
    GiaBan DECIMAL(18, 2) NOT NULL CHECK (GiaBan >= 0),
    SoLuongTon INT NOT NULL CHECK (SoLuongTon >= 0),
    NhaXuatBan NVARCHAR(255) NULL,
    MoTa TEXT NULL,
    HinhAnh VARCHAR(500) NULL,
    NgayXuatBan DATE NULL,
    SoTrang INT NULL,
    TrangThai BOOLEAN DEFAULT TRUE,
    
    MaTG INT,
    MaTL INT,
    
    -- Ràng buộc Khóa ngoại
    FOREIGN KEY (MaTG) REFERENCES TacGia(MaTG),
    FOREIGN KEY (MaTL) REFERENCES TheLoai(MaTL)
);


-- #######################################################
-- III. Bảng Giao dịch (Hóa đơn)
-- #######################################################

-- 8. Bảng Hóa đơn
CREATE TABLE HoaDon (
    MaHD INT PRIMARY KEY AUTO_INCREMENT,
    NgayBan DATETIME NOT NULL,
    TongTien DECIMAL(18, 2) NOT NULL CHECK (TongTien >= 0),

    MaNV INT,
    MaKH INT,
    
    -- Ràng buộc Khóa ngoại
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);

-- 9. Bảng Chi tiết Hóa đơn (Sản phẩm trong từng hóa đơn)
CREATE TABLE ChiTietHD (
    MaCTHD INT PRIMARY KEY AUTO_INCREMENT,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18, 2) NOT NULL CHECK (DonGia >= 0),

    MaHD INT,
    MaSach VARCHAR(20),
    
    -- Ràng buộc Khóa ngoại
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

-- #######################################################
-- IV. Dữ liệu Mẫu (Sample Data)
-- #######################################################

-- Thêm Tác giả
INSERT INTO TacGia (TenTG) VALUES (N'Nguyễn Nhật Ánh'), (N'Stephen King'), (N'Eric Evans');

-- Thêm Thể loại
INSERT INTO TheLoai (TenTL) VALUES (N'Tiểu thuyết'), (N'Sách Kỹ năng'), (N'Lập trình');

-- Thêm Nhân viên (Admin và Nhân viên thường)
INSERT INTO NhanVien (TenNV, SDT, DiaChi, NgayVaoLam) VALUES 
(N'Nguyễn Văn A (Admin)', '0901234567', N'Quận 1, TPHCM', '2020-01-01'),
(N'Trần Thị B (Nhân viên)', '0902345678', N'Quận 3, TPHCM', '2023-05-15');

-- Thêm Tài khoản (Admin và Nhân viên)
-- Lưu ý: Trong thực tế, 'matkhau123' phải được băm (hashed) trước khi lưu.
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, MaNV) VALUES 
('admin', 'matkhau123', 'Admin', 1), -- Liên kết với Nhân viên có MaNV = 1
('nhanvien01', 'matkhau123', 'NhanVien', 2); -- Liên kết với Nhân viên có MaNV = 2

-- Thêm Sách
INSERT INTO Sach (MaSach, TenSach, MaTG, MaTL, GiaBan, SoLuongTon, NhaXuatBan, MoTa, NgayXuatBan, SoTrang, TrangThai) VALUES
('S001', N'Tôi thấy hoa vàng trên cỏ xanh', 1, 1, 100000.00, 50, N'Nhà xuất bản Trẻ', N'Một tác phẩm văn học hay về tuổi thơ', '2018-01-01', 320, TRUE),
('S002', N'Domain-Driven Design', 3, 3, 550000.00, 15, N'Addison-Wesley', N'Sách về thiết kế phần mềm theo domain', '2003-08-30', 560, TRUE),
('S003', N'The Shining', 2, 1, 250000.00, 30, N'Doubleday', N'Tiểu thuyết kinh dị kinh điển', '1977-01-28', 447, TRUE);

-- Thêm Khách hàng
INSERT INTO KhachHang (TenKH, SoDienThoai, Email, NgayTao) VALUES 
(N'Lê Văn C', '0903456789', 'levanc@email.com', NOW()), 
(N'Phạm Thị D', '0904567890', 'phamthid@email.com', NOW());

-- Thêm Hóa đơn
INSERT INTO HoaDon (MaNV, MaKH, NgayBan, TongTien) VALUES 
(2, 1, NOW(), 350000.00); -- Nhân viên 2 bán cho Khách hàng 1

-- Thêm Chi tiết Hóa đơn
INSERT INTO ChiTietHD (MaHD, MaSach, SoLuong, DonGia) VALUES
(1, 'S001', 2, 100000.00), -- 2 cuốn 'Tôi thấy hoa vàng...'
(1, 'S003', 1, 150000.00); -- 1 cuốn 'The Shining' (giả sử có khuyến mãi còn 150k)

-- Verify the changes
SELECT 'Database Schema Updated Successfully' as Status;
SELECT * FROM Sach;
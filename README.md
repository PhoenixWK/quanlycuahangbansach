# Hệ Thống Quản Lý Cửa Hàng Bán Sách

Hệ thống quản lý cửa hàng bán sách được phát triển bằng C# Windows Forms với kiến trúc 3-layer, sử dụng MySQL làm cơ sở dữ liệu.

## Tính Năng Chính

### Hệ Thống Đăng Nhập & Phân Quyền
- **Admin**: Toàn quyền quản lý hệ thống
- **Nhân viên**: Quyền hạn hạn chế theo chức năng

### Quản Lý Sách
- Thêm, sửa, xóa thông tin sách
- Tìm kiếm sách theo tên, tác giả, thể loại
- Quản lý hình ảnh sách
- Theo dõi số lượng tồn kho

### Quản Lý Khách Hàng
- Thêm mới khách hàng
- Cập nhật thông tin khách hàng
- Xem lịch sử mua hàng của khách hàng
- Thống kê khách hàng

### Quản Lý Nhân Viên (Dành cho Admin)
- Thêm, sửa, xóa nhân viên
- Quản lý tài khoản đăng nhập
- Phân quyền người dùng

### Quản Lý Hóa Đơn
- **Admin**: 
  - Xem tất cả hóa đơn
  - Chỉnh sửa hóa đơn
  - Xóa hóa đơn
  - Xuất PDF hóa đơn
- **Nhân viên**: 
  - Tạo hóa đơn mới
  - Xem hóa đơn
  - Xuất PDF hóa đơn

### Thống Kê & Báo Cáo
- **Thống kê tổng quan**:
  - Tổng số sách
  - Số lượng khách hàng  
  - Số lượng nhân viên
  - Số hóa đơn theo thời gian
  - Doanh thu theo thời gian
  - Giá trị đơn hàng trung bình

- **Biểu đồ doanh thu**: Hiển thị doanh thu 6 tháng gần nhất
- **Lọc thời gian**: Tháng này, 3 tháng, 6 tháng, năm nay, tất cả

### Quản Lý Danh Mục
- Quản lý tác giả
- Quản lý thể loại sách

## Kiến Trúc Hệ Thống

### Three-Layer Architecture
```
BookStoreGUI (Presentation Layer)
├── Forms & Panels
├── User Controls
└── UI Logic

BookStoreBLL (Business Logic Layer)  
├── Business Rules
├── Data Processing
└── Validation Logic

BookStoreDAL (Data Access Layer)
├── Database Operations
├── SQL Queries
└── Connection Management

BookStoreDTO (Data Transfer Objects)
├── Entity Models
└── Data Structures
```

## Công Nghệ Sử Dụng

- **Framework**: .NET 10.0 Windows Forms
- **Database**: MySQL
- **PDF Generation**: iTextSharp
- **Architecture**: 3-Layer Architecture (GUI - BLL - DAL - DTO)

## Các Package Dependencies

```xml
<PackageReference Include="MySql.Data" Version="9.1.0" />
<PackageReference Include="iTextSharp" Version="5.5.13.3" />
```

## Cài Đặt & Chạy Ứng Dụng

### Yêu Cầu Hệ Thống
- Windows 10/11
- .NET 10.0 Runtime
- MySQL Server 8.0+

### Bước 1: Clone Repository
```bash
git clone https://github.com/PhoenixWK/quanlycuahangbansach.git
cd quan-ly-cua-hang-ban-sach
```

### Bước 2: Cài Đặt Cơ Sở Dữ Liệu
1. Khởi tạo MySQL server
2. Chạy script `db.sql` để tạo database và tables
3. Cập nhật connection string trong `BookStoreDAL/DatabaseConnection.cs`

### Bước 3: Build & Run
```bash
dotnet restore
dotnet build
dotnet run --project BookStoreGUI
```

## Tài Khoản Mặc Định

### Admin
- **Username**: `admin`
- **Password**: `admin123`

### Nhân Viên
- **Username**: `employee`  
- **Password**: `emp123`

## Giao Diện Ứng Dụng

### Màn Hình Chính
- Sidebar navigation với các module
- Header hiển thị thông tin user và thời gian
- Content area động theo module được chọn

### Các Module Chính
1. **Quản Lý Bán Hàng** - Tạo hóa đơn, quản lý giỏ hàng
2. **Quản Lý Sách** - CRUD operations cho sách
3. **Quản Lý Khách Hàng** - Thông tin khách hàng
4. **Quản Lý Nhân Viên** - Chỉ dành cho Admin
5. **Quản Lý Hóa Đơn** - Xem, sửa, xuất PDF
6. **Thống Kê** - Dashboard với charts và metrics

## Database Schema

### Các Bảng Chính
- `sach` - Thông tin sách
- `tacgia` - Tác giả  
- `theloai` - Thể loại
- `khachhang` - Khách hàng
- `nhanvien` - Nhân viên
- `taikhoan` - Tài khoản đăng nhập
- `hoadon` - Hóa đơn
- `chitiethd` - Chi tiết hóa đơn

## Troubleshooting

### Lỗi Kết Nối Database
```
Kiểm tra:
1. MySQL server đang chạy
2. Connection string đúng
3. Firewall không block port 3306
4. User có quyền truy cập database
```

### Lỗi Build
```bash
# Clean và rebuild
dotnet clean
dotnet restore
dotnet build
```

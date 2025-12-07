using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BookStoreBLL;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class InvoiceManagementPanel : UserControl
    {
        private readonly SachBLL sachBLL;
        private readonly KhachHangBLL khachHangBLL;
        private readonly HoaDonBLL hoaDonBLL;
        private readonly ChiTietHDBLL chiTietHDBLL;
        private readonly NhanVienBLL nhanVienBLL;

        private Panel headerPanel = null!;
        private TextBox searchBookTextBox = null!;
        private ComboBox customerComboBox = null!;
        private DataGridView booksDataGrid = null!;
        private DataGridView cartDataGrid = null!;
        private Label totalLabel = null!;
        private Button addToCartButton = null!;
        private Button removeFromCartButton = null!;
        private Button saveInvoiceButton = null!;
        private Button clearCartButton = null!;
        private NumericUpDown quantityNumeric = null!;

        private List<SachDTO> availableBooks = new List<SachDTO>();
        private List<CartItemDTO> cartItems = new List<CartItemDTO>();
        private decimal totalAmount = 0;

        public InvoiceManagementPanel()
        {
            sachBLL = new SachBLL();
            khachHangBLL = new KhachHangBLL();
            hoaDonBLL = new HoaDonBLL();
            chiTietHDBLL = new ChiTietHDBLL();
            nhanVienBLL = new NhanVienBLL();
            
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main container
            this.Size = new Size(1100, 700);
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Padding = new Padding(20);

            CreateHeaderPanel();
            CreateCustomerSelection();
            CreateBookSearchSection();
            CreateCartSection();
            CreateTotalSection();

            this.ResumeLayout(false);
        }

        private void CreateHeaderPanel()
        {
            headerPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1060, 60),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label titleLabel = new Label
            {
                Text = "🛒 Lập Hóa Đơn Bán Sách",
                Location = new Point(20, 15),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255)
            };

            headerPanel.Controls.Add(titleLabel);
            this.Controls.Add(headerPanel);
        }

        private void CreateCustomerSelection()
        {
            Panel customerPanel = new Panel
            {
                Location = new Point(20, 90),
                Size = new Size(520, 80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label customerLabel = new Label
            {
                Text = "Khách hàng:",
                Location = new Point(15, 15),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            customerComboBox = new ComboBox
            {
                Location = new Point(15, 45),
                Size = new Size(480, 30),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            customerPanel.Controls.AddRange(new Control[] { customerLabel, customerComboBox });
            this.Controls.Add(customerPanel);
        }

        private void CreateBookSearchSection()
        {
            // Search panel
            Panel searchPanel = new Panel
            {
                Location = new Point(20, 180),
                Size = new Size(520, 80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label searchLabel = new Label
            {
                Text = "Tìm kiếm sách:",
                Location = new Point(15, 15),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            searchBookTextBox = new TextBox
            {
                Location = new Point(15, 45),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 11),
                PlaceholderText = "Nhập tên sách hoặc tác giả..."
            };
            searchBookTextBox.TextChanged += SearchBookTextBox_TextChanged;

            quantityNumeric = new NumericUpDown
            {
                Location = new Point(325, 45),
                Size = new Size(80, 30),
                Font = new Font("Segoe UI", 11),
                Minimum = 1,
                Maximum = 1000,
                Value = 1
            };

            addToCartButton = new Button
            {
                Text = "➕ Thêm",
                Location = new Point(415, 45),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            addToCartButton.FlatAppearance.BorderSize = 0;
            addToCartButton.Click += AddToCartButton_Click;

            searchPanel.Controls.AddRange(new Control[] { searchLabel, searchBookTextBox, quantityNumeric, addToCartButton });

            // Books grid
            booksDataGrid = new DataGridView
            {
                Location = new Point(20, 270),
                Size = new Size(520, 200),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 10),
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            SetupBooksGrid();

            this.Controls.AddRange(new Control[] { searchPanel, booksDataGrid });
        }

        private void CreateCartSection()
        {
            // Cart panel header
            Panel cartHeaderPanel = new Panel
            {
                Location = new Point(560, 180),
                Size = new Size(520, 40),
                BackColor = Color.FromArgb(0, 123, 255),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label cartLabel = new Label
            {
                Text = "🛒 Giỏ hàng",
                Location = new Point(15, 8),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White
            };

            removeFromCartButton = new Button
            {
                Text = "➖ Xóa",
                Location = new Point(350, 5),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            removeFromCartButton.FlatAppearance.BorderSize = 0;
            removeFromCartButton.Click += RemoveFromCartButton_Click;

            clearCartButton = new Button
            {
                Text = "🗑️ Xóa tất cả",
                Location = new Point(435, 5),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            clearCartButton.FlatAppearance.BorderSize = 0;
            clearCartButton.Click += ClearCartButton_Click;

            cartHeaderPanel.Controls.AddRange(new Control[] { cartLabel, removeFromCartButton, clearCartButton });

            // Cart grid
            cartDataGrid = new DataGridView
            {
                Location = new Point(560, 220),
                Size = new Size(520, 250),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 10),
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            SetupCartGrid();

            this.Controls.AddRange(new Control[] { cartHeaderPanel, cartDataGrid });
        }

        private void CreateTotalSection()
        {
            Panel totalPanel = new Panel
            {
                Location = new Point(560, 480),
                Size = new Size(520, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            totalLabel = new Label
            {
                Text = "Tổng tiền: 0 VNĐ",
                Location = new Point(15, 15),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 53, 69)
            };

            saveInvoiceButton = new Button
            {
                Text = "💾 Lưu Hóa Đơn",
                Location = new Point(350, 50),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            saveInvoiceButton.FlatAppearance.BorderSize = 0;
            saveInvoiceButton.Click += SaveInvoiceButton_Click;

            totalPanel.Controls.AddRange(new Control[] { totalLabel, saveInvoiceButton });
            this.Controls.Add(totalPanel);
        }

        private void SetupBooksGrid()
        {
            booksDataGrid.Columns.Clear();
            booksDataGrid.Columns.Add("MaSach", "Mã sách");
            booksDataGrid.Columns.Add("TenSach", "Tên sách");
            booksDataGrid.Columns.Add("TenTacGia", "Tác giả");
            booksDataGrid.Columns.Add("GiaBan", "Giá bán");
            booksDataGrid.Columns.Add("SoLuongTon", "Tồn kho");

            booksDataGrid.Columns["MaSach"].Width = 80;
            booksDataGrid.Columns["TenSach"].Width = 200;
            booksDataGrid.Columns["TenTacGia"].Width = 120;
            booksDataGrid.Columns["GiaBan"].Width = 80;
            booksDataGrid.Columns["SoLuongTon"].Width = 70;
        }

        private void SetupCartGrid()
        {
            cartDataGrid.Columns.Clear();
            cartDataGrid.Columns.Add("MaSach", "Mã sách");
            cartDataGrid.Columns.Add("TenSach", "Tên sách");
            cartDataGrid.Columns.Add("SoLuong", "Số lượng");
            cartDataGrid.Columns.Add("GiaBan", "Giá bán");
            cartDataGrid.Columns.Add("ThanhTien", "Thành tiền");

            cartDataGrid.Columns["MaSach"].Width = 80;
            cartDataGrid.Columns["TenSach"].Width = 200;
            cartDataGrid.Columns["SoLuong"].Width = 80;
            cartDataGrid.Columns["GiaBan"].Width = 80;
            cartDataGrid.Columns["ThanhTien"].Width = 100;
        }

        private void LoadData()
        {
            try
            {
                LoadBooks();
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBooks()
        {
            availableBooks = sachBLL.LayDanhSachSach();
            DisplayBooks(availableBooks);
        }

        private void LoadCustomers()
        {
            var customers = khachHangBLL.LayDanhSachKhachHang();
            customerComboBox.DataSource = customers;
            customerComboBox.DisplayMember = "TenKH";
            customerComboBox.ValueMember = "MaKH";
        }

        private void DisplayBooks(List<SachDTO> books)
        {
            booksDataGrid.Rows.Clear();
            foreach (var book in books.Where(b => b.SoLuongTon > 0))
            {
                booksDataGrid.Rows.Add(
                    book.MaSach,
                    book.TenSach,
                    book.TenTacGia,
                    $"{book.GiaBan ?? 0:N0} VNĐ",
                    book.SoLuongTon
                );
            }
        }

        private void DisplayCart()
        {
            cartDataGrid.Rows.Clear();
            foreach (var item in cartItems)
            {
                cartDataGrid.Rows.Add(
                    item.MaSach,
                    item.TenSach,
                    item.SoLuong.ToString(),
                    $"{item.GiaBan:N0} VNĐ",
                    $"{item.ThanhTien:N0} VNĐ"
                );
            }
            
            totalAmount = cartItems.Sum(item => item.ThanhTien);
            totalLabel.Text = $"Tổng tiền: {totalAmount:N0} VNĐ";
        }

        private void SearchBookTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = searchBookTextBox.Text.Trim().ToLower();
                
                if (string.IsNullOrEmpty(searchText))
                {
                    DisplayBooks(availableBooks);
                }
                else
                {
                    var filteredBooks = availableBooks.Where(book =>
                        book.TenSach?.ToLower().Contains(searchText) == true ||
                        book.TenTacGia?.ToLower().Contains(searchText) == true
                    ).ToList();
                    
                    DisplayBooks(filteredBooks);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddToCartButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (booksDataGrid.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn sách để thêm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = booksDataGrid.SelectedRows[0];
                string maSach = selectedRow.Cells["MaSach"].Value.ToString()!;
                string tenSach = selectedRow.Cells["TenSach"].Value.ToString()!;
                int soLuongTon = int.Parse(selectedRow.Cells["SoLuongTon"].Value.ToString()!);
                int soLuongMua = (int)quantityNumeric.Value;

                if (soLuongMua > soLuongTon)
                {
                    MessageBox.Show($"Số lượng không đủ! Chỉ còn {soLuongTon} cuốn trong kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var book = availableBooks.First(b => b.MaSach == maSach);
                decimal giaBan = book.GiaBan ?? 0;
                var existingItem = cartItems.FirstOrDefault(item => item.MaSach == maSach);

                if (existingItem != null)
                {
                    existingItem.SoLuong += soLuongMua;
                    existingItem.ThanhTien = existingItem.SoLuong * existingItem.GiaBan;
                }
                else
                {
                    var cartItem = new CartItemDTO
                    {
                        MaSach = maSach,
                        TenSach = tenSach,
                        SoLuong = soLuongMua,
                        GiaBan = giaBan,
                        ThanhTien = soLuongMua * giaBan
                    };
                    cartItems.Add(cartItem);
                }

                DisplayCart();
                quantityNumeric.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm vào giỏ hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveFromCartButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (cartDataGrid.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = cartDataGrid.SelectedRows[0];
                string maSach = selectedRow.Cells["MaSach"].Value.ToString()!;
                
                cartItems.RemoveAll(item => item.MaSach == maSach);
                DisplayCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearCartButton_Click(object sender, EventArgs e)
        {
            cartItems.Clear();
            DisplayCart();
        }

        private void SaveInvoiceButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (cartItems.Count == 0)
                {
                    MessageBox.Show("Giỏ hàng trống! Vui lòng thêm sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (customerComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create invoice
                var hoaDon = new HoaDonDTO
                {
                    MaNV = SessionManager.CurrentUser?.MaNV ?? 1, // Default to admin
                    MaKH = (int)customerComboBox.SelectedValue,
                    NgayBan = DateTime.Now,
                    TongTien = totalAmount
                };

                int maHD = hoaDonBLL.ThemHoaDon(hoaDon);
                
                if (maHD > 0)
                {
                    // Add invoice details
                    foreach (var item in cartItems)
                    {
                        var chiTiet = new ChiTietHDDTO
                        {
                            MaHD = maHD,
                            MaSach = item.MaSach,
                            SoLuong = item.SoLuong,
                            DonGia = item.GiaBan
                        };
                        chiTietHDBLL.ThemChiTietHD(chiTiet);

                        // Update stock
                        var book = availableBooks.First(b => b.MaSach == item.MaSach);
                        book.SoLuongTon -= item.SoLuong;
                        sachBLL.CapNhatSach(book);
                    }

                    MessageBox.Show($"Tạo hóa đơn thành công!\nMã hóa đơn: {maHD}\nTổng tiền: {totalAmount:N0} VNĐ", 
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear cart and reload data
                    cartItems.Clear();
                    DisplayCart();
                    LoadBooks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Helper DTO for cart items
    public class CartItemDTO
    {
        public string MaSach { get; set; } = "";
        public string TenSach { get; set; } = "";
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
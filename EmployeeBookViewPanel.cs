using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BookStoreBLL;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class EmployeeBookViewPanel : UserControl
    {
        private readonly SachBLL sachBLL;
        private readonly TacGiaBLL tacGiaBLL;
        private readonly TheLoaiBLL theLoaiBLL;

        private Panel headerPanel = null!;
        private TextBox searchTextBox = null!;
        private ComboBox authorFilterComboBox = null!;
        private ComboBox categoryFilterComboBox = null!;
        private Button searchButton = null!;
        private Button refreshButton = null!;
        private Panel booksCardsPanel = null!;
        private Panel selectedBookDetailPanel = null!;

        private List<SachDTO> allBooks = new List<SachDTO>();
        private List<SachDTO> filteredBooks = new List<SachDTO>();

        public EmployeeBookViewPanel()
        {
            sachBLL = new SachBLL();
            tacGiaBLL = new TacGiaBLL();
            theLoaiBLL = new TheLoaiBLL();
            
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
            CreateSearchAndFilterPanel();
            CreateBooksDisplayArea();
            CreateBookDetailPanel();

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
                Text = "📚 Danh Mục Sách - Tư Vấn Khách Hàng",
                Location = new Point(20, 15),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255)
            };

            Label infoLabel = new Label
            {
                Text = "Tra cứu thông tin sách để tư vấn khách hàng",
                Location = new Point(550, 20),
                Size = new Size(400, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleRight
            };

            headerPanel.Controls.AddRange(new Control[] { titleLabel, infoLabel });
            this.Controls.Add(headerPanel);
        }

        private void CreateSearchAndFilterPanel()
        {
            Panel searchPanel = new Panel
            {
                Location = new Point(20, 90),
                Size = new Size(1060, 80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Search box
            Label searchLabel = new Label
            {
                Text = "Tìm kiếm:",
                Location = new Point(15, 15),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            searchTextBox = new TextBox
            {
                Location = new Point(15, 45),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Nhập tên sách, tác giả..."
            };

            // Author filter
            Label authorLabel = new Label
            {
                Text = "Tác giả:",
                Location = new Point(280, 15),
                Size = new Size(60, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            authorFilterComboBox = new ComboBox
            {
                Location = new Point(280, 45),
                Size = new Size(180, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Category filter
            Label categoryLabel = new Label
            {
                Text = "Thể loại:",
                Location = new Point(480, 15),
                Size = new Size(70, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            categoryFilterComboBox = new ComboBox
            {
                Location = new Point(480, 45),
                Size = new Size(180, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Buttons
            searchButton = new Button
            {
                Text = "🔍 Tìm kiếm",
                Location = new Point(680, 45),
                Size = new Size(100, 25),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.Click += SearchButton_Click;

            refreshButton = new Button
            {
                Text = "🔄 Làm mới",
                Location = new Point(790, 45),
                Size = new Size(100, 25),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.Click += RefreshButton_Click;

            searchPanel.Controls.AddRange(new Control[] { 
                searchLabel, searchTextBox, authorLabel, authorFilterComboBox, 
                categoryLabel, categoryFilterComboBox, searchButton, refreshButton 
            });

            this.Controls.Add(searchPanel);
        }

        private void CreateBooksDisplayArea()
        {
            Panel containerPanel = new Panel
            {
                Location = new Point(20, 180),
                Size = new Size(720, 500),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            Label booksLabel = new Label
            {
                Text = "Danh sách sách",
                Location = new Point(15, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            booksCardsPanel = new Panel
            {
                Location = new Point(15, 40),
                Size = new Size(680, 440),
                AutoScroll = true,
                BackColor = Color.Transparent
            };

            containerPanel.Controls.AddRange(new Control[] { booksLabel, booksCardsPanel });
            this.Controls.Add(containerPanel);
        }

        private void CreateBookDetailPanel()
        {
            selectedBookDetailPanel = new Panel
            {
                Location = new Point(760, 180),
                Size = new Size(320, 500),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            Label detailLabel = new Label
            {
                Text = "Chi tiết sách",
                Location = new Point(15, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            Label selectPromptLabel = new Label
            {
                Text = "Chọn một cuốn sách để xem chi tiết",
                Location = new Point(15, 50),
                Size = new Size(290, 100),
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleCenter
            };

            selectedBookDetailPanel.Controls.AddRange(new Control[] { detailLabel, selectPromptLabel });
            this.Controls.Add(selectedBookDetailPanel);
        }

        private void LoadData()
        {
            try
            {
                LoadBooks();
                LoadFilters();
                DisplayBooks(allBooks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBooks()
        {
            allBooks = sachBLL.LayDanhSachSach();
            filteredBooks = allBooks.ToList();
        }

        private void LoadFilters()
        {
            // Load authors
            var authors = tacGiaBLL.LayDanhSachTacGia();
            var allAuthorsItem = new TacGiaDTO { MaTG = 0, TenTG = "-- Tất cả tác giả --" };
            authors.Insert(0, allAuthorsItem);
            
            authorFilterComboBox.DataSource = authors;
            authorFilterComboBox.DisplayMember = "TenTG";
            authorFilterComboBox.ValueMember = "MaTG";

            // Load categories
            var categories = theLoaiBLL.LayDanhSachTheLoai();
            var allCategoriesItem = new TheLoaiDTO { MaTL = 0, TenTL = "-- Tất cả thể loại --" };
            categories.Insert(0, allCategoriesItem);
            
            categoryFilterComboBox.DataSource = categories;
            categoryFilterComboBox.DisplayMember = "TenTL";
            categoryFilterComboBox.ValueMember = "MaTL";
        }

        private void DisplayBooks(List<SachDTO> books)
        {
            booksCardsPanel.Controls.Clear();
            booksCardsPanel.SuspendLayout();

            int cardWidth = 320;
            int cardHeight = 120;
            int margin = 10;
            int x = 0, y = 0;
            int cardsPerRow = 2;

            for (int i = 0; i < books.Count; i++)
            {
                var book = books[i];
                var bookCard = CreateBookCard(book, cardWidth, cardHeight);
                
                bookCard.Location = new Point(x, y);
                bookCard.Click += (sender, e) => ShowBookDetail(book);
                
                // Make all child controls clickable too
                foreach (Control child in bookCard.Controls)
                {
                    child.Click += (sender, e) => ShowBookDetail(book);
                }

                booksCardsPanel.Controls.Add(bookCard);

                x += cardWidth + margin;
                if ((i + 1) % cardsPerRow == 0)
                {
                    x = 0;
                    y += cardHeight + margin;
                }
            }

            booksCardsPanel.ResumeLayout(false);
        }

        private Panel CreateBookCard(SachDTO book, int width, int height)
        {
            Panel card = new Panel
            {
                Size = new Size(width, height),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            Label titleLabel = new Label
            {
                Text = book.TenSach ?? "N/A",
                Location = new Point(10, 10),
                Size = new Size(width - 20, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255)
            };

            Label authorLabel = new Label
            {
                Text = $"Tác giả: {book.TenTacGia ?? "N/A"}",
                Location = new Point(10, 35),
                Size = new Size(width - 20, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            Label categoryLabel = new Label
            {
                Text = $"Thể loại: {book.TenTheLoai ?? "N/A"}",
                Location = new Point(10, 55),
                Size = new Size(width - 20, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            Label priceLabel = new Label
            {
                Text = $"Giá: {book.GiaBan?.ToString("N0") ?? "N/A"} VNĐ",
                Location = new Point(10, 75),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 167, 69)
            };

            Label stockLabel = new Label
            {
                Text = $"Tồn kho: {book.SoLuongTon ?? 0}",
                Location = new Point(170, 75),
                Size = new Size(140, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = book.SoLuongTon > 0 ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69)
            };

            card.Controls.AddRange(new Control[] { titleLabel, authorLabel, categoryLabel, priceLabel, stockLabel });
            return card;
        }

        private void ShowBookDetail(SachDTO book)
        {
            selectedBookDetailPanel.Controls.Clear();
            selectedBookDetailPanel.SuspendLayout();

            Label titleLabel = new Label
            {
                Text = "Chi tiết sách",
                Location = new Point(15, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            // Book details
            int yPosition = 50;
            int spacing = 30;

            var details = new[]
            {
                ("Mã sách:", book.MaSach ?? "N/A"),
                ("Tên sách:", book.TenSach ?? "N/A"),
                ("Tác giả:", book.TenTacGia ?? "N/A"),
                ("Thể loại:", book.TenTheLoai ?? "N/A"),
                ("Nhà xuất bản:", book.NhaXuatBan ?? "N/A"),
                ("Giá bán:", $"{book.GiaBan?.ToString("N0") ?? "N/A"} VNĐ"),
                ("Số lượng tồn:", $"{book.SoLuongTon ?? 0} cuốn"),
                ("Số trang:", $"{book.SoTrang ?? 0} trang"),
                ("Ngày xuất bản:", book.NgayXuatBan?.ToString("dd/MM/yyyy") ?? "N/A")
            };

            selectedBookDetailPanel.Controls.Add(titleLabel);

            foreach (var (label, value) in details)
            {
                Label fieldLabel = new Label
                {
                    Text = label,
                    Location = new Point(15, yPosition),
                    Size = new Size(100, 20),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                };

                Label valueLabel = new Label
                {
                    Text = value,
                    Location = new Point(115, yPosition),
                    Size = new Size(190, 20),
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(80, 80, 80)
                };

                selectedBookDetailPanel.Controls.AddRange(new Control[] { fieldLabel, valueLabel });
                yPosition += spacing;
            }

            // Description
            if (!string.IsNullOrEmpty(book.MoTa))
            {
                Label descLabel = new Label
                {
                    Text = "Mô tả:",
                    Location = new Point(15, yPosition),
                    Size = new Size(100, 20),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                };

                TextBox descTextBox = new TextBox
                {
                    Text = book.MoTa,
                    Location = new Point(15, yPosition + 25),
                    Size = new Size(280, 80),
                    Font = new Font("Segoe UI", 9),
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    BackColor = Color.FromArgb(248, 249, 250)
                };

                selectedBookDetailPanel.Controls.AddRange(new Control[] { descLabel, descTextBox });
            }

            selectedBookDetailPanel.ResumeLayout(false);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                searchTextBox.Text = "";
                authorFilterComboBox.SelectedIndex = 0;
                categoryFilterComboBox.SelectedIndex = 0;
                LoadBooks();
                DisplayBooks(allBooks);
                
                // Clear detail panel
                selectedBookDetailPanel.Controls.Clear();
                selectedBookDetailPanel.Controls.Add(new Label
                {
                    Text = "Chi tiết sách",
                    Location = new Point(15, 10),
                    Size = new Size(200, 25),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                });
                selectedBookDetailPanel.Controls.Add(new Label
                {
                    Text = "Chọn một cuốn sách để xem chi tiết",
                    Location = new Point(15, 50),
                    Size = new Size(290, 100),
                    Font = new Font("Segoe UI", 10, FontStyle.Italic),
                    ForeColor = Color.FromArgb(108, 117, 125),
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            string searchText = searchTextBox.Text.Trim().ToLower();
            int selectedAuthor = (int)authorFilterComboBox.SelectedValue;
            int selectedCategory = (int)categoryFilterComboBox.SelectedValue;

            filteredBooks = allBooks.Where(book =>
            {
                bool matchesSearch = string.IsNullOrEmpty(searchText) ||
                    book.TenSach?.ToLower().Contains(searchText) == true ||
                    book.TenTacGia?.ToLower().Contains(searchText) == true;

                bool matchesAuthor = selectedAuthor == 0 || 
                    int.TryParse(book.MaTG, out int maTG) && maTG == selectedAuthor;

                bool matchesCategory = selectedCategory == 0 || 
                    int.TryParse(book.MaTL, out int maTL) && maTL == selectedCategory;

                return matchesSearch && matchesAuthor && matchesCategory;
            }).ToList();

            DisplayBooks(filteredBooks);
        }
    }
}
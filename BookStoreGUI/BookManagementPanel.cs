using BookStoreBLL;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class BookManagementPanel : UserControl
    {
        private SachBLL sachBLL;
        private List<SachDTO> books;

        private Panel bookCardsPanel = null!;
        private TextBox searchTextBox = null!;
        private Button addButton = null!;

        public BookManagementPanel()
        {
            sachBLL = new SachBLL();
            books = new List<SachDTO>();
            InitializeComponent();
            LoadBooks();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main panel setup
            this.Size = new Size(1200, 800);
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Header panel
            Panel headerPanel = new Panel
            {
                Height = 120,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Padding = new Padding(40, 20, 40, 20)
            };

            Label titleLabel = new Label
            {
                Text = "📚 QUẢN LÝ SÁCH",
                Location = new Point(0, 20),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            Label subtitleLabel = new Label
            {
                Text = "Quản lý toàn bộ kho sách trong hệ thống",
                Location = new Point(0, 60),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(120, 120, 120)
            };

            headerPanel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel });

            // Search and action panel
            Panel searchPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Padding = new Padding(40, 10, 40, 10)
            };

            searchTextBox = new TextBox
            {
                Width = 350,
                Height = 32,
                Location = new Point(0, 14),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = Color.FromArgb(120, 120, 120),
                Text = "Tìm kiếm sách..."
            };

            searchTextBox.GotFocus += (s, e) =>
            {
                if (searchTextBox.Text == "Tìm kiếm sách...")
                {
                    searchTextBox.Text = "";
                    searchTextBox.ForeColor = Color.FromArgb(60, 60, 60);
                }
            };

            searchTextBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchTextBox.Text))
                {
                    searchTextBox.Text = "Tìm kiếm sách...";
                    searchTextBox.ForeColor = Color.FromArgb(120, 120, 120);
                }
            };
            
            searchTextBox.TextChanged += SearchTextBox_TextChanged;

            Button searchButton = new Button
            {
                Text = "🔍 Search",
                Width = 120,
                Height = 32,
                Location = new Point(360, 14),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 103, 235);
            searchButton.Click += SearchButton_Click;

            Button refreshButton = new Button
            {
                Text = "🔄 Làm mới",
                Width = 120,
                Height = 32,
                Location = new Point(490, 14),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(90, 98, 104);
            refreshButton.Click += RefreshButton_Click;

            // Admin-only add button
            addButton = new Button
            {
                Text = "+ Thêm sách",
                Width = 140,
                Height = 32,
                Location = new Point(650, 14),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = SessionManager.IsAdmin // Only visible for admin
            };
            addButton.FlatAppearance.BorderSize = 0;
            addButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(34, 142, 58);
            addButton.Click += AddButton_Click;

            searchPanel.Controls.AddRange(new Control[] { searchTextBox, searchButton, refreshButton, addButton });

            // Books cards container
            bookCardsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(40, 20, 40, 40),
                AutoScroll = true
            };

            this.Controls.Add(bookCardsPanel);
            this.Controls.Add(searchPanel);
            this.Controls.Add(headerPanel);

            this.ResumeLayout(false);
        }

        private void LoadBooks()
        {
            try
            {
                books = sachBLL.LayDanhSachSach();
                DisplayBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayBooks()
        {
            bookCardsPanel.Controls.Clear();
            
            if (books == null || books.Count == 0)
            {
                Label emptyLabel = new Label
                {
                    Text = "Không có sách nào trong hệ thống",
                    Font = new Font("Segoe UI", 14),
                    ForeColor = Color.FromArgb(120, 120, 120),
                    AutoSize = true,
                    Location = new Point(50, 50)
                };
                bookCardsPanel.Controls.Add(emptyLabel);
                return;
            }

            int x = 0, y = 0;
            int cardWidth = 350;
            int cardHeight = 150;
            int margin = 20;
            int cardsPerRow = (bookCardsPanel.Width - 80) / (cardWidth + margin);

            for (int i = 0; i < books.Count; i++)
            {
                Panel bookCard = CreateBookCard(books[i]);
                bookCard.Location = new Point(x * (cardWidth + margin), y * (cardHeight + margin));
                bookCardsPanel.Controls.Add(bookCard);

                x++;
                if (x >= cardsPerRow)
                {
                    x = 0;
                    y++;
                }
            }
        }

        private Panel CreateBookCard(SachDTO book)
        {
            Panel cardPanel = new Panel
            {
                Size = new Size(350, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            // Add shadow effect
            cardPanel.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, cardPanel.ClientRectangle, Color.FromArgb(200, 200, 200), ButtonBorderStyle.Solid);
            };

            // Book ID Badge
            Label idBadge = new Label
            {
                Text = book.MaSach,
                Location = new Point(15, 15),
                Size = new Size(60, 25),
                BackColor = Color.FromArgb(96, 91, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Book name
            Label nameLabel = new Label
            {
                Text = book.TenSach?.Length > 30 ? book.TenSach.Substring(0, 30) + "..." : book.TenSach,
                Location = new Point(85, 12),
                Size = new Size(220, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Author and category info
            Label infoLabel = new Label
            {
                Text = $"Tác giả: {book.TenTacGia} | Thể loại: {book.TenTheLoai}",
                Location = new Point(85, 35),
                Size = new Size(250, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Price and stock info
            Label priceStockLabel = new Label
            {
                Text = $"Giá: {book.GiaBan:N0} VND | Tồn kho: {book.SoLuongTon}",
                Location = new Point(85, 55),
                Size = new Size(250, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Stock status indicator
            Label stockStatusLabel = new Label
            {
                Text = book.SoLuongTon > 10 ? "• Còn hàng" : book.SoLuongTon > 0 ? "• Sắp hết" : "• Hết hàng",
                Location = new Point(85, 75),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = book.SoLuongTon > 10 ? Color.FromArgb(40, 167, 69) : 
                           book.SoLuongTon > 0 ? Color.FromArgb(255, 193, 7) : Color.FromArgb(220, 53, 69),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Action Buttons Panel (Admin only)
            if (SessionManager.IsAdmin)
            {
                Panel buttonPanel = new Panel
                {
                    Location = new Point(200, 100),
                    Size = new Size(140, 40),
                    BackColor = Color.Transparent
                };

                Button editButton = new Button
                {
                    Text = "✏ Edit",
                    Location = new Point(0, 5),
                    Size = new Size(65, 30),
                    BackColor = Color.FromArgb(255, 193, 7),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = book
                };
                editButton.FlatAppearance.BorderSize = 0;
                editButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 173, 7);
                editButton.Click += EditButton_Click;

                Button deleteButton = new Button
                {
                    Text = "🗑 Delete",
                    Location = new Point(70, 5),
                    Size = new Size(65, 30),
                    BackColor = Color.FromArgb(220, 53, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = book
                };
                deleteButton.FlatAppearance.BorderSize = 0;
                deleteButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 35, 51);
                deleteButton.Click += DeleteButton_Click;

                buttonPanel.Controls.AddRange(new Control[] { editButton, deleteButton });
                cardPanel.Controls.Add(buttonPanel);
            }

            // Add all controls to card
            cardPanel.Controls.AddRange(new Control[] {
                idBadge, nameLabel, infoLabel, priceStockLabel, stockStatusLabel
            });

            // Add hover effect
            cardPanel.MouseEnter += (s, e) => {
                cardPanel.BackColor = Color.FromArgb(248, 249, 250);
                cardPanel.Cursor = Cursors.Default;
            };
            cardPanel.MouseLeave += (s, e) => cardPanel.BackColor = Color.White;

            return cardPanel;
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (searchTextBox.Text == "Tìm kiếm sách..." || string.IsNullOrWhiteSpace(searchTextBox.Text))
                return;

            // Auto-search after typing
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 500; // 500ms delay
            timer.Tick += (s, ev) =>
            {
                timer.Stop();
                PerformSearch();
            };
            timer.Start();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            try
            {
                string searchTerm = searchTextBox.Text == "Tìm kiếm sách..." ? "" : searchTextBox.Text.Trim();
                books = sachBLL.TimKiemSach(searchTerm);
                DisplayBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "Tìm kiếm sách...";
            searchTextBox.ForeColor = Color.FromArgb(120, 120, 120);
            LoadBooks();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var bookForm = new BookForm();
            if (bookForm.ShowDialog() == DialogResult.OK)
            {
                LoadBooks(); // Refresh the list
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sender is Button button && button.Tag is SachDTO book)
            {
                var bookForm = new BookForm(book);
                if (bookForm.ShowDialog() == DialogResult.OK)
                {
                    LoadBooks(); // Refresh the list
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sender is Button button && button.Tag is SachDTO book)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sách \"{book.TenSach}\"?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        bool deleteResult = sachBLL.XoaSach(book.MaSach!);
                        if (deleteResult)
                        {
                            MessageBox.Show("Xóa sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadBooks(); // Refresh the list
                        }
                        else
                        {
                            MessageBox.Show("Xóa sách thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
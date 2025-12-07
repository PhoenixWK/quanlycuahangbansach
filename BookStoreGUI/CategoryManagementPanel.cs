using BookStoreBLL;
using BookStoreDTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BookStoreGUI
{
    public partial class CategoryManagementPanel : UserControl
    {
        private readonly TheLoaiBLL theLoaiBLL = new TheLoaiBLL();
        private List<TheLoaiDTO> categories = new List<TheLoaiDTO>();
        private Panel categoryCardsPanel;
        private TextBox searchTextBox;
        private Button addButton;

        public CategoryManagementPanel()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main panel setup
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(20);

            // Header panel
            Panel headerPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent
            };

            // Title label
            Label titleLabel = new Label
            {
                Text = "QUẢN LÝ THỂ LOẠI",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = true,
                Location = new Point(0, 15)
            };

            // Search controls
            Panel searchPanel = new Panel
            {
                Height = 50,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 10)
            };

            searchTextBox = new TextBox
            {
                Width = 350,
                Height = 32,
                Location = new Point(0, 9),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Text = "🔍 Tìm kiếm thể loại...",
                ForeColor = Color.FromArgb(150, 150, 150)
            };
            
            // Add placeholder functionality
            searchTextBox.Enter += (s, e) => {
                if (searchTextBox.Text == "🔍 Tìm kiếm thể loại...")
                {
                    searchTextBox.Text = "";
                    searchTextBox.ForeColor = Color.FromArgb(51, 51, 51);
                }
            };
            
            searchTextBox.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(searchTextBox.Text))
                {
                    searchTextBox.Text = "🔍 Tìm kiếm thể loại...";
                    searchTextBox.ForeColor = Color.FromArgb(150, 150, 150);
                }
            };
            
            searchTextBox.TextChanged += SearchTextBox_TextChanged;

            Button searchButton = new Button
            {
                Text = "🔍 Search",
                Width = 120,
                Height = 32,
                Location = new Point(360, 9),
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
                Location = new Point(490, 9),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(88, 97, 105);
            refreshButton.Click += RefreshButton_Click;

            addButton = new Button
            {
                Text = "➕ Thêm thể loại",
                Width = 160,
                Height = 32,
                Location = new Point(620, 9),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            addButton.FlatAppearance.BorderSize = 0;
            addButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(33, 136, 56);
            addButton.Click += AddButton_Click;

            // Cards container
            categoryCardsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true,
                Padding = new Padding(0, 10, 0, 0)
            };

            // Add controls to panels
            searchPanel.Controls.AddRange(new Control[] { searchTextBox, searchButton, refreshButton, addButton });
            headerPanel.Controls.Add(titleLabel);

            // Add panels to main control
            this.Controls.Add(categoryCardsPanel);
            this.Controls.Add(searchPanel);
            this.Controls.Add(headerPanel);

            this.ResumeLayout(false);

            // Load data
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                categories = theLoaiBLL.LayDanhSachTheLoai();
                DisplayCategories(categories);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách thể loại: {ex.Message}", "Lỗi", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayCategories(List<TheLoaiDTO> categoryList)
        {
            categoryCardsPanel.Controls.Clear();

            int cardWidth = 320; // Card width + margin
            int cardHeight = 135; // Card height + margin
            int cardsPerRow = Math.Max(1, categoryCardsPanel.Width / cardWidth);
            
            int currentRow = 0;
            int currentCol = 0;
            
            foreach (var category in categoryList)
            {
                Panel cardPanel = CreateCategoryCard(category);
                
                int xPosition = currentCol * cardWidth + 10;
                int yPosition = currentRow * cardHeight + 10;
                
                cardPanel.Location = new Point(xPosition, yPosition);
                categoryCardsPanel.Controls.Add(cardPanel);
                
                currentCol++;
                if (currentCol >= cardsPerRow)
                {
                    currentCol = 0;
                    currentRow++;
                }
            }

            // Update scroll size
            if (categoryCardsPanel.Controls.Count > 0)
            {
                int totalHeight = (currentRow + (currentCol > 0 ? 1 : 0)) * cardHeight + 20;
                categoryCardsPanel.AutoScrollMinSize = new Size(0, totalHeight);
            }
        }

        private Panel CreateCategoryCard(TheLoaiDTO category)
        {
            Panel cardPanel = new Panel
            {
                Width = 300,
                Height = 135,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10, 0, 10, 15)
            };

            // Add card shadow effect
            cardPanel.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, cardPanel.Width - 1, cardPanel.Height - 1);
                using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            // Category ID Badge
            Label idBadge = new Label
            {
                Text = $"#{category.MaTL:D3}",
                Location = new Point(15, 15),
                Size = new Size(60, 25),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(40, 167, 69),
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.None
            };

            // Category Name Label
            Label nameLabel = new Label
            {
                Text = category.TenTL,
                Location = new Point(85, 12),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Category info subtitle
            Label infoLabel = new Label
            {
                Text = "Thể loại sách",
                Location = new Point(85, 40),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Status indicator
            Label statusLabel = new Label
            {
                Text = "• Hoạt động",
                Location = new Point(85, 62),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(40, 167, 69),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Action Buttons Panel
            Panel buttonPanel = new Panel
            {
                Location = new Point(80, 80),
                Size = new Size(200, 40),
                BackColor = Color.Transparent
            };

            Button editButton = new Button
            {
                Text = "✏ Sửa",
                Location = new Point(0, 5),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Tag = category
            };
            editButton.FlatAppearance.BorderSize = 0;
            editButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 173, 7);
            editButton.Click += EditButton_Click;

            Button deleteButton = new Button
            {
                Text = "🗑 Delete",
                Location = new Point(85, 5),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Tag = category
            };
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 35, 51);
            deleteButton.Click += DeleteButton_Click;

            // Add all controls to button panel
            buttonPanel.Controls.AddRange(new Control[] { editButton, deleteButton });

            // Add all controls to card
            cardPanel.Controls.AddRange(new Control[] {
                idBadge, nameLabel, infoLabel, statusLabel, buttonPanel
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
            if (searchTextBox.Text == "🔍 Tìm kiếm thể loại..." || string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                DisplayCategories(categories);
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm == "🔍 Tìm kiếm thể loại...")
            {
                DisplayCategories(categories);
                return;
            }

            try
            {
                var searchResults = theLoaiBLL.TimKiemTheLoai(searchTerm);
                DisplayCategories(searchResults);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "🔍 Tìm kiếm thể loại...";
            searchTextBox.ForeColor = Color.FromArgb(150, 150, 150);
            LoadCategories();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var addForm = new CategoryForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadCategories();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is TheLoaiDTO category)
            {
                var editForm = new CategoryForm(category);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadCategories();
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is TheLoaiDTO category)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa thể loại '{category.TenTL}'?\n\nLưu ý: Không thể xóa thể loại đã có sách trong hệ thống!",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (theLoaiBLL.XoaTheLoai(category.MaTL))
                        {
                            MessageBox.Show("Xóa thể loại thành công!", "Thông báo", 
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCategories();
                        }
                        else
                        {
                            MessageBox.Show("Không thể xóa thể loại!", "Lỗi", 
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa thể loại: {ex.Message}", "Lỗi", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void RefreshData()
        {
            LoadCategories();
        }
    }
}
using BookStoreBLL;
using BookStoreDTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BookStoreGUI
{
    public partial class CustomerManagementPanel : UserControl
    {
        private readonly KhachHangBLL khachHangBLL = new KhachHangBLL();
        private List<KhachHangDTO> customers = new List<KhachHangDTO>();
        private Panel customerCardsPanel;
        private TextBox searchTextBox;
        private Button addButton;

        public CustomerManagementPanel()
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
                Text = "QUẢN LÝ KHÁCH HÀNG",
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
                Width = 300,
                Height = 30,
                Location = new Point(0, 10),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(51, 51, 51),
                BorderStyle = BorderStyle.FixedSingle
            };
            searchTextBox.TextChanged += SearchTextBox_TextChanged;

            Button searchButton = new Button
            {
                Text = "Tìm kiếm",
                Width = 100,
                Height = 30,
                Location = new Point(310, 10),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand
            };
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.Click += SearchButton_Click;

            Button refreshButton = new Button
            {
                Text = "Làm mới",
                Width = 100,
                Height = 30,
                Location = new Point(420, 10),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand
            };
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.Click += RefreshButton_Click;

            addButton = new Button
            {
                Text = "Thêm khách hàng",
                Width = 150,
                Height = 30,
                Location = new Point(530, 10),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand
            };
            addButton.FlatAppearance.BorderSize = 0;
            addButton.Click += AddButton_Click;

            // Cards container
            customerCardsPanel = new Panel
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
            this.Controls.Add(customerCardsPanel);
            this.Controls.Add(searchPanel);
            this.Controls.Add(headerPanel);

            this.ResumeLayout(false);

            // Load data
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                customers = khachHangBLL.LayDanhSachKhachHang();
                DisplayCustomers(customers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khách hàng: {ex.Message}", "Lỗi", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayCustomers(List<KhachHangDTO> customerList)
        {
            customerCardsPanel.Controls.Clear();

            int cardWidth = 440;
            int cardHeight = 150;
            int margin = 15;
            int cardsPerRow = Math.Max(3, (customerCardsPanel.Width - margin) / (cardWidth + margin));
            
            int currentRow = 0;
            int currentCol = 0;
            
            foreach (var customer in customerList)
            {
                Panel cardPanel = CreateCustomerCard(customer);
                
                int x = margin + currentCol * (cardWidth + margin);
                int y = margin + currentRow * (cardHeight + margin);
                
                cardPanel.Location = new Point(x, y);
                customerCardsPanel.Controls.Add(cardPanel);
                
                currentCol++;
                if (currentCol >= cardsPerRow)
                {
                    currentCol = 0;
                    currentRow++;
                }
            }

            // Update scroll size
            if (customerCardsPanel.Controls.Count > 0)
            {
                int totalHeight = (currentRow + (currentCol > 0 ? 1 : 0)) * (cardHeight + margin) + margin;
                customerCardsPanel.AutoScrollMinSize = new Size(0, totalHeight);
            }
        }

        private Panel CreateCustomerCard(KhachHangDTO customer)
        {
            Panel cardPanel = new Panel
            {
                Width = 440,
                Height = 150,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5)
            };

            // Customer ID Label
            Label idLabel = new Label
            {
                Text = $"ID: {customer.MaKH}",
                Location = new Point(15, 10),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255)
            };

            // Customer Name Label
            Label nameLabel = new Label
            {
                Text = customer.TenKH,
                Location = new Point(15, 35),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51)
            };

            // Email Info
            Label emailLabel = new Label
            {
                Text = $"Email: {customer.Email ?? "Chưa có"}",
                Location = new Point(15, 65),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125),
                AutoEllipsis = true
            };

            // Date Info
            Label dateLabel = new Label
            {
                Text = $"Ngày tạo: {customer.NgayTao:dd/MM/yyyy}",
                Location = new Point(15, 90),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            // Action Buttons with proper sizing and positioning
            Button editButton = new Button
            {
                Text = "Sửa",
                Location = new Point(330, 20),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand,
                Tag = customer,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            editButton.FlatAppearance.BorderSize = 0;
            editButton.Click += EditButton_Click;

            Button deleteButton = new Button
            {
                Text = "Xóa",
                Location = new Point(330, 55),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand,
                Tag = customer,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.Click += DeleteButton_Click;

            Button viewOrdersButton = new Button
            {
                Text = "Đơn hàng",
                Location = new Point(330, 90),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand,
                Tag = customer,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            viewOrdersButton.FlatAppearance.BorderSize = 0;
            viewOrdersButton.Click += ViewOrdersButton_Click;

            // Add all controls to card
            cardPanel.Controls.AddRange(new Control[] {
                idLabel, nameLabel, emailLabel,
                dateLabel, editButton, deleteButton, viewOrdersButton
            });

            // Add hover effect
            cardPanel.MouseEnter += (s, e) => cardPanel.BackColor = Color.FromArgb(248, 249, 250);
            cardPanel.MouseLeave += (s, e) => cardPanel.BackColor = Color.White;

            return cardPanel;
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                DisplayCustomers(customers);
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                DisplayCustomers(customers);
                return;
            }

            try
            {
                var searchResults = khachHangBLL.TimKiemKhachHang(searchTerm);
                DisplayCustomers(searchResults);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Clear();
            LoadCustomers();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var addForm = new CustomerForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadCustomers();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is KhachHangDTO customer)
            {
                var editForm = new CustomerForm(customer);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadCustomers();
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is KhachHangDTO customer)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa khách hàng '{customer.TenKH}'?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (khachHangBLL.XoaKhachHang(customer.MaKH))
                        {
                            MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", 
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCustomers();
                        }
                        else
                        {
                            MessageBox.Show("Không thể xóa khách hàng!", "Lỗi", 
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa khách hàng: {ex.Message}", "Lỗi", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ViewOrdersButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is KhachHangDTO customer)
            {
                // TODO: Implement order history view for customer
                MessageBox.Show($"Xem đơn hàng của khách hàng: {customer.TenKH}", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void RefreshData()
        {
            LoadCustomers();
        }
    }
}
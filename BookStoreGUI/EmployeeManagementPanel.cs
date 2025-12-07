using BookStoreBLL;
using BookStoreDTO;

namespace BookStoreGUI
{
    public class EmployeeManagementPanel : UserControl
    {
        private FlowLayoutPanel employeeCardsPanel;
        private TextBox searchBox;
        private Button btnAddEmployee;
        private Button btnRefresh;

        public EmployeeManagementPanel()
        {
            InitializeComponent();
            LoadEmployeeCards();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Header Panel
            var headerPanel = new Panel
            {
                Height = 70,
                Dock = DockStyle.Top,
                BackColor = Color.White
            };

            var titleLabel = new Label
            {
                Text = "Quản lý Nhân viên",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 20),
                AutoSize = true
            };

            // Action Bar Panel
            var actionBarPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Padding = new Padding(30, 10, 30, 10)
            };

            var searchContainer = new Panel
            {
                Width = 350,
                Height = 40,
                Location = new Point(30, 10),
                BackColor = Color.FromArgb(245, 245, 245),
                Padding = new Padding(10, 8, 10, 8)
            };

            searchBox = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 11),
                Text = "Tìm kiếm nhân viên...",
                ForeColor = Color.FromArgb(149, 165, 166),
                BackColor = Color.FromArgb(245, 245, 245)
            };

            btnAddEmployee = new Button
            {
                Text = "➕ Thêm Nhân viên",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Width = 180,
                Height = 40,
                Location = new Point(400, 10),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddEmployee.FlatAppearance.BorderSize = 0;

            btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Font = new Font("Segoe UI", 10),
                Width = 130,
                Height = 40,
                Location = new Point(600, 10),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;

            // Main Content Area
            var mainContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(30, 20, 30, 30)
            };

            // Employee Cards Container with grid layout
            employeeCardsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 249, 250),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(20, 20, 20, 20)
            };

            // Search functionality
            searchBox.GotFocus += SearchBox_GotFocus;
            searchBox.LostFocus += SearchBox_LostFocus;
            searchBox.TextChanged += SearchBox_TextChanged;

            // Button events
            btnAddEmployee.Click += BtnAddEmployee_Click;
            btnRefresh.Click += BtnRefresh_Click;

            // Assembly
            searchContainer.Controls.Add(searchBox);
            actionBarPanel.Controls.Add(searchContainer);
            actionBarPanel.Controls.Add(btnAddEmployee);
            actionBarPanel.Controls.Add(btnRefresh);
            mainContentPanel.Controls.Add(employeeCardsPanel);
            headerPanel.Controls.Add(titleLabel);

            this.Controls.Add(mainContentPanel);
            this.Controls.Add(actionBarPanel);
            this.Controls.Add(headerPanel);
        }

        private void SearchBox_GotFocus(object? sender, EventArgs e)
        {
            if (searchBox.Text == "Tìm kiếm nhân viên...")
            {
                searchBox.Text = "";
                searchBox.ForeColor = Color.FromArgb(44, 62, 80);
            }
        }

        private void SearchBox_LostFocus(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchBox.Text))
            {
                searchBox.Text = "Tìm kiếm nhân viên...";
                searchBox.ForeColor = Color.FromArgb(149, 165, 166);
            }
        }

        private void SearchBox_TextChanged(object? sender, EventArgs e)
        {
            if (searchBox.Text != "Tìm kiếm nhân viên..." && !string.IsNullOrWhiteSpace(searchBox.Text))
            {
                FilterEmployeeCards(searchBox.Text);
            }
            else if (searchBox.Text == "")
            {
                LoadEmployeeCards();
            }
        }

        private void BtnAddEmployee_Click(object? sender, EventArgs e)
        {
            ShowAddEditEmployeeForm();
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadEmployeeCards();
        }

        private void LoadEmployeeCards()
        {
            employeeCardsPanel.Controls.Clear();

            try
            {
                var nhanVienBLL = new NhanVienBLL();
                var employees = nhanVienBLL.LayDanhSachNhanVien();

                foreach (var employee in employees)
                {
                    var card = CreateEmployeeCard(employee);
                    employeeCardsPanel.Controls.Add(card);
                }

                if (employees.Count == 0)
                {
                    ShowEmptyState();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowEmptyState()
        {
            var emptyPanel = new Panel
            {
                Width = 1200,
                Height = 250,
                BackColor = Color.White,
                Margin = new Padding(0, 50, 0, 0)
            };

            var emptyLabel = new Label
            {
                Text = "Chưa có nhân viên nào",
                Font = new Font("Segoe UI", 16),
                ForeColor = Color.FromArgb(149, 165, 166),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            emptyPanel.Controls.Add(emptyLabel);
            employeeCardsPanel.Controls.Add(emptyPanel);
        }

        private Panel CreateEmployeeCard(NhanVienDTO employee)
        {
            var card = new Panel
            {
                Width = 380,
                Height = 200,
                BackColor = Color.White,
                Margin = new Padding(10, 10, 10, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Employee ID Badge
            Label idBadge = new Label
            {
                Text = $"#{employee.MaNV:D3}",
                Location = new Point(15, 15),
                Size = new Size(60, 25),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = employee.VaiTro == "Admin" ? Color.FromArgb(231, 76, 60) : Color.FromArgb(52, 152, 219),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Employee Name
            var nameLabel = new Label
            {
                Text = employee.TenNV,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(85, 15),
                Size = new Size(280, 30),
                AutoEllipsis = true
            };

            // Role
            var roleLabel = new Label
            {
                Text = !string.IsNullOrEmpty(employee.VaiTro) ? employee.VaiTro : "Chưa có tài khoản",
                Font = new Font("Segoe UI", 10),
                ForeColor = !string.IsNullOrEmpty(employee.VaiTro) ?
                    (employee.VaiTro == "Admin" ? Color.FromArgb(231, 76, 60) : Color.FromArgb(39, 174, 96)) :
                    Color.FromArgb(230, 126, 34),
                Location = new Point(85, 50),
                Size = new Size(280, 20),
                AutoEllipsis = true
            };

            // Phone Number
            var phoneLabel = new Label
            {
                Text = employee.SDT ?? "Chưa cập nhật",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(15, 75),
                Size = new Size(350, 20),
                AutoEllipsis = true
            };

            // Address
            var addressLabel = new Label
            {
                Text = employee.DiaChi ?? "Chưa cập nhật",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(15, 95),
                Size = new Size(350, 20),
                AutoEllipsis = true
            };

            // Join Date
            var joinDateLabel = new Label
            {
                Text = $"Vào làm: {employee.NgayVaoLam:dd/MM/yyyy}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(15, 115),
                Size = new Size(350, 20),
                AutoEllipsis = true
            };

            // Action buttons
            var btnEdit = CreateActionButton("Sửa", Color.FromArgb(241, 196, 15), new Point(15, 165), 80);
            var btnAccount = CreateActionButton(
                !string.IsNullOrEmpty(employee.TenDangNhap) ? "Tài khoản" : "Tạo TK",
                !string.IsNullOrEmpty(employee.TenDangNhap) ? Color.FromArgb(155, 89, 182) : Color.FromArgb(52, 152, 219),
                new Point(105, 165), 100);
            var btnDelete = CreateActionButton("Xóa", Color.FromArgb(231, 76, 60), new Point(215, 165), 80);

            // Button events
            btnEdit.Click += (s, e) => ShowAddEditEmployeeForm(employee);
            btnAccount.Click += (s, e) => ShowAccountManagementForm(employee);
            btnDelete.Click += (s, e) => DeleteEmployee(employee);

            // Assembly
            card.Controls.AddRange(new Control[] {
                idBadge, nameLabel, roleLabel, phoneLabel,
                addressLabel, joinDateLabel, btnEdit, btnAccount, btnDelete
            });

            // Add hover effect
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(248, 249, 250);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            return card;
        }

        private Button CreateActionButton(string text, Color color, Point location, int width = 90)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Width = width,
                Height = 30,
                Location = location,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            button.FlatAppearance.BorderSize = 0;

            // Add hover effect
            var originalColor = color;
            button.MouseEnter += (s, e) => button.BackColor = ControlPaint.Dark(originalColor, 0.1f);
            button.MouseLeave += (s, e) => button.BackColor = originalColor;

            return button;
        }

        private void FilterEmployeeCards(string searchTerm)
        {
            try
            {
                var nhanVienBLL = new NhanVienBLL();
                var filteredEmployees = nhanVienBLL.TimKiemNhanVien(searchTerm);

                employeeCardsPanel.Controls.Clear();
                foreach (var employee in filteredEmployees)
                {
                    var card = CreateEmployeeCard(employee);
                    employeeCardsPanel.Controls.Add(card);
                }

                if (filteredEmployees.Count == 0)
                {
                    ShowNoResultsFound(searchTerm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowNoResultsFound(string searchTerm)
        {
            var noResultsPanel = new Panel
            {
                Width = 1200,
                Height = 250,
                BackColor = Color.White,
                Margin = new Padding(0, 50, 0, 0)
            };

            var noResultsLabel = new Label
            {
                Text = $"Không tìm thấy nhân viên nào với từ khóa '{searchTerm}'",
                Font = new Font("Segoe UI", 14),
                ForeColor = Color.FromArgb(149, 165, 166),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            noResultsPanel.Controls.Add(noResultsLabel);
            employeeCardsPanel.Controls.Add(noResultsPanel);
        }

        private void ShowAddEditEmployeeForm(NhanVienDTO? employee = null)
        {
            using var form = new EmployeeForm(employee);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadEmployeeCards();
            }
        }

        private void ShowAccountManagementForm(NhanVienDTO employee)
        {
            using var form = new AccountManagementForm(employee);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadEmployeeCards();
            }
        }

        private void DeleteEmployee(NhanVienDTO employee)
        {
            var result = MessageBox.Show(
                $"Xác nhận xóa nhân viên '{employee.TenNV}'?\n\nLưu ý: Không thể xóa nhân viên có tài khoản.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var nhanVienBLL = new NhanVienBLL();
                    bool success = nhanVienBLL.XoaNhanVien(employee.MaNV);

                    if (success)
                    {
                        MessageBox.Show("Xóa nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEmployeeCards();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void RefreshData()
        {
            LoadEmployeeCards();
        }
    }
}
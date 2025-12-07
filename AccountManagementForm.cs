using BookStoreBLL;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class AccountManagementForm : Form
    {
        private NhanVienDTO _employee;
        private bool _hasAccount;
        private Label lblInfo, lblCurrentAccount, lblCurrentRole;
        private Label lblUsername, lblPassword, lblRole, lblRoleChange;
        private TextBox txtUsername, txtPassword;
        private ComboBox cmbRole, cmbRoleChange;
        private Button btnCreateAccount, btnUpdateRole, btnResetPassword, btnDeleteAccount, btnClose;

        public AccountManagementForm(NhanVienDTO employee)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _hasAccount = !string.IsNullOrEmpty(employee.TenDangNhap);
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            this.Text = $"Quản lý Tài khoản - {_employee.TenNV}";
            this.Size = new Size(700, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Header Panel
            var headerPanel = new Panel
            {
                Height = 70,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(155, 89, 182)
            };

            lblInfo = new Label
            {
                Text = $"🔐 Quản lý tài khoản\n{_employee.TenNV}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            headerPanel.Controls.Add(lblInfo);

            // Content Panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30, 20, 30, 70),
                BackColor = Color.White
            };

            if (_hasAccount)
            {
                SetupExistingAccountUI(contentPanel);
            }
            else
            {
                SetupNewAccountUI(contentPanel);
            }

            // Buttons Panel
            var buttonsPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(30, 15, 30, 15)
            };

            btnClose = new Button
            {
                Text = "❌ Đóng",
                Location = new Point(550, 15),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                DialogResult = DialogResult.Cancel
            };
            btnClose.FlatAppearance.BorderSize = 0;

            buttonsPanel.Controls.Add(btnClose);

            this.Controls.AddRange(new Control[] { contentPanel, buttonsPanel, headerPanel });
            this.CancelButton = btnClose;
        }

        private void SetupExistingAccountUI(Panel contentPanel)
        {
            // Account info
            lblCurrentAccount = new Label
            {
                Text = $"👤 Tài khoản hiện tại: {_employee.TenDangNhap}",
                Location = new Point(0, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            lblCurrentRole = new Label
            {
                Text = $"🔑 Vai trò: {_employee.VaiTro}",
                Location = new Point(0, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = GetRoleColor(_employee.VaiTro)
            };

            // Role change section
            var roleChangeGroup = new GroupBox
            {
                Text = "Thay đổi vai trò",
                Location = new Point(0, 70),
                Size = new Size(620, 80),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblRoleChange = new Label
            {
                Text = "Vai trò mới:",
                Location = new Point(15, 25),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            cmbRoleChange = new ComboBox
            {
                Location = new Point(15, 45),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbRoleChange.Items.AddRange(new[] { "Admin", "NhanVien" });
            cmbRoleChange.SelectedItem = _employee.VaiTro;

            btnUpdateRole = new Button
            {
                Text = "🔄 Cập nhật",
                Location = new Point(180, 43),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnUpdateRole.FlatAppearance.BorderSize = 0;
            btnUpdateRole.Click += BtnUpdateRole_Click;

            roleChangeGroup.Controls.AddRange(new Control[] { lblRoleChange, cmbRoleChange, btnUpdateRole });

            // Action buttons
            btnResetPassword = new Button
            {
                Text = "🔐 Đặt lại Mật khẩu",
                Location = new Point(0, 170),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnResetPassword.FlatAppearance.BorderSize = 0;
            btnResetPassword.Click += BtnResetPassword_Click;

            btnDeleteAccount = new Button
            {
                Text = "🗑️ Xóa Tài khoản",
                Location = new Point(170, 170),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnDeleteAccount.FlatAppearance.BorderSize = 0;
            btnDeleteAccount.Click += BtnDeleteAccount_Click;

            // Add hover effects
            AddButtonHoverEffect(btnUpdateRole, Color.FromArgb(52, 152, 219));
            AddButtonHoverEffect(btnResetPassword, Color.FromArgb(230, 126, 34));
            AddButtonHoverEffect(btnDeleteAccount, Color.FromArgb(231, 76, 60));

            contentPanel.Controls.AddRange(new Control[] {
                lblCurrentAccount, lblCurrentRole, roleChangeGroup,
                btnResetPassword, btnDeleteAccount
            });
        }

        private void SetupNewAccountUI(Panel contentPanel)
        {
            var noAccountLabel = new Label
            {
                Text = "⚠️ Nhân viên chưa có tài khoản hệ thống",
                Location = new Point(0, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(230, 126, 34)
            };

            // Create account form
            var createAccountGroup = new GroupBox
            {
                Text = "Tạo tài khoản mới",
                Location = new Point(0, 50),
                Size = new Size(620, 230),
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            lblUsername = new Label
            {
                Text = "Tên đăng nhập: *",
                Location = new Point(15, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            txtUsername = new TextBox
            {
                Location = new Point(15, 55),
                Width = 250,
                Height = 25,
                Font = new Font("Segoe UI", 10)
            };

            lblPassword = new Label
            {
                Text = "Mật khẩu: *",
                Location = new Point(280, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            txtPassword = new TextBox
            {
                Location = new Point(280, 55),
                Width = 250,
                Height = 25,
                Font = new Font("Segoe UI", 10),
                PasswordChar = '*'
            };

            lblRole = new Label
            {
                Text = "Vai trò: *",
                Location = new Point(15, 90),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            cmbRole = new ComboBox
            {
                Location = new Point(15, 115),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbRole.Items.AddRange(new[] { "Admin", "NhanVien" });
            cmbRole.SelectedIndex = 1; // Default to NhanVien

            btnCreateAccount = new Button
            {
                Text = "➕ Tạo Tài khoản",
                Location = new Point(250, 113),
                Size = new Size(160, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCreateAccount.FlatAppearance.BorderSize = 0;
            btnCreateAccount.Click += BtnCreateAccount_Click;

            AddButtonHoverEffect(btnCreateAccount, Color.FromArgb(46, 204, 113));

            createAccountGroup.Controls.AddRange(new Control[] {
                lblUsername, txtUsername, lblPassword, txtPassword,
                lblRole, cmbRole, btnCreateAccount
            });

            contentPanel.Controls.AddRange(new Control[] { noAccountLabel, createAccountGroup });
        }

        private void SetupForm()
        {
            if (!_hasAccount)
            {
                txtUsername.Focus();
            }
        }

        private Color GetRoleColor(string? role)
        {
            return role switch
            {
                "Admin" => Color.FromArgb(231, 76, 60),
                "NhanVien" => Color.FromArgb(39, 174, 96),
                _ => Color.FromArgb(149, 165, 166)
            };
        }

        private void AddButtonHoverEffect(Button button, Color originalColor)
        {
            button.MouseEnter += (s, e) => button.BackColor = ControlPaint.Dark(originalColor, 0.1f);
            button.MouseLeave += (s, e) => button.BackColor = originalColor;
        }

        private void BtnCreateAccount_Click(object? sender, EventArgs e)
        {
            if (!ValidateCreateAccountForm())
                return;

            try
            {
                var taiKhoanBLL = new TaiKhoanBLL();
                bool success = taiKhoanBLL.TaoTaiKhoan(
                    _employee.MaNV,
                    txtUsername.Text.Trim(),
                    txtPassword.Text.Trim(),
                    cmbRole.SelectedItem?.ToString() ?? "NhanVien");

                if (success)
                {
                    MessageBox.Show("Tạo tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể tạo tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdateRole_Click(object? sender, EventArgs e)
        {
            var newRole = cmbRoleChange.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(newRole))
            {
                MessageBox.Show("Vui lòng chọn vai trò!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newRole == _employee.VaiTro)
            {
                MessageBox.Show("Vai trò mới phải khác vai trò hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Xác nhận thay đổi vai trò từ '{_employee.VaiTro}' thành '{newRole}'?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var taiKhoanBLL = new TaiKhoanBLL();
                    bool success = taiKhoanBLL.CapNhatVaiTro(_employee.MaNV, newRole);

                    if (success)
                    {
                        MessageBox.Show("Cập nhật vai trò thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật vai trò!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnResetPassword_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Mật khẩu sẽ được đặt lại thành 'password123'.\n\nXác nhận thực hiện?",
                "Xác nhận đặt lại mật khẩu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var taiKhoanBLL = new TaiKhoanBLL();
                    bool success = taiKhoanBLL.DatLaiMatKhau(_employee.MaNV, "password123");

                    if (success)
                    {
                        MessageBox.Show("Đặt lại mật khẩu thành công!\n\nMật khẩu mới: password123", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể đặt lại mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteAccount_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"⚠️ CẢNH BÁO ⚠️\n\nXác nhận xóa tài khoản '{_employee.TenDangNhap}'?\n\n" +
                "Nhân viên sẽ không thể đăng nhập vào hệ thống sau khi xóa.\n" +
                "Thao tác này không thể hoàn tác!",
                "Xác nhận xóa tài khoản",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var taiKhoanBLL = new TaiKhoanBLL();
                    bool success = taiKhoanBLL.XoaTaiKhoan(_employee.MaNV);

                    if (success)
                    {
                        MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateCreateAccountForm()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (txtUsername.Text.Trim().Length < 3)
            {
                MessageBox.Show("Tên đăng nhập phải có ít nhất 3 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbRole.Focus();
                return false;
            }

            return true;
        }
    }
}
using BookStoreBLL;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class EmployeeForm : Form
    {
        private NhanVienDTO? _employee;
        private Label lblName, lblPhone, lblAddress, lblJoinDate;
        private TextBox txtName, txtPhone, txtAddress;
        private DateTimePicker dtpJoinDate;
        private Button btnSave, btnCancel;

        public EmployeeForm(NhanVienDTO? employee = null)
        {
            _employee = employee;
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            this.Text = _employee == null ? "Thêm Nhân viên mới" : "Chỉnh sửa Nhân viên";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Header Panel
            var headerPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(52, 152, 219)
            };

            var headerLabel = new Label
            {
                Text = _employee == null ? "➕ Thêm Nhân viên mới" : "✏️ Chỉnh sửa Nhân viên",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            headerPanel.Controls.Add(headerLabel);

            // Form Fields Panel
            var fieldsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30, 30, 30, 80)
            };

            // Name Field
            lblName = new Label
            {
                Text = "Họ và tên: *",
                Location = new Point(0, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            txtName = new TextBox
            {
                Location = new Point(0, 35),
                Width = 420,
                Height = 30,
                Font = new Font("Segoe UI", 11),
                Text = _employee?.TenNV ?? ""
            };

            // Phone Field
            lblPhone = new Label
            {
                Text = "Số điện thoại:",
                Location = new Point(0, 85),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            txtPhone = new TextBox
            {
                Location = new Point(0, 110),
                Width = 200,
                Height = 30,
                Font = new Font("Segoe UI", 11),
                Text = _employee?.SDT ?? ""
            };

            // Join Date Field
            lblJoinDate = new Label
            {
                Text = "Ngày vào làm: *",
                Location = new Point(220, 85),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            dtpJoinDate = new DateTimePicker
            {
                Location = new Point(220, 110),
                Width = 200,
                Font = new Font("Segoe UI", 11),
                Value = _employee?.NgayVaoLam ?? DateTime.Now,
                Format = DateTimePickerFormat.Short
            };

            // Address Field
            lblAddress = new Label
            {
                Text = "Địa chỉ:",
                Location = new Point(0, 160),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            txtAddress = new TextBox
            {
                Location = new Point(0, 185),
                Width = 420,
                Height = 30,
                Font = new Font("Segoe UI", 11),
                Text = _employee?.DiaChi ?? ""
            };

            // Buttons Panel
            var buttonsPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(30, 15, 30, 15)
            };

            btnCancel = new Button
            {
                Text = "❌ Hủy",
                Location = new Point(250, 15),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            btnSave = new Button
            {
                Text = "💾 Lưu",
                Location = new Point(340, 15),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.FlatAppearance.BorderSize = 0;

            // Add hover effects
            AddButtonHoverEffect(btnSave, Color.FromArgb(46, 204, 113));
            AddButtonHoverEffect(btnCancel, Color.FromArgb(149, 165, 166));

            // Event handlers
            btnSave.Click += BtnSave_Click;
            txtName.KeyPress += TxtName_KeyPress;

            // Assembly
            buttonsPanel.Controls.AddRange(new Control[] { btnCancel, btnSave });
            fieldsPanel.Controls.AddRange(new Control[] {
                lblName, txtName, lblPhone, txtPhone,
                lblJoinDate, dtpJoinDate, lblAddress, txtAddress
            });

            this.Controls.AddRange(new Control[] { fieldsPanel, buttonsPanel, headerPanel });
            this.CancelButton = btnCancel;
            this.AcceptButton = btnSave;
        }

        private void SetupForm()
        {
            // Focus on name field
            txtName.Focus();

            // Validation styling
            SetValidationStyling();
        }

        private void SetValidationStyling()
        {
            txtName.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    txtName.BackColor = Color.FromArgb(254, 242, 242);
                    txtName.BorderStyle = BorderStyle.FixedSingle;
                }
                else
                {
                    txtName.BackColor = Color.White;
                }
            };
        }

        private void AddButtonHoverEffect(Button button, Color originalColor)
        {
            button.MouseEnter += (s, e) => button.BackColor = ControlPaint.Dark(originalColor, 0.1f);
            button.MouseLeave += (s, e) => button.BackColor = originalColor;
        }

        private void TxtName_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Allow only letters, spaces, and Vietnamese characters
            if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var nhanVienBLL = new NhanVienBLL();
                var emp = new NhanVienDTO
                {
                    MaNV = _employee?.MaNV ?? 0,
                    TenNV = txtName.Text.Trim(),
                    SDT = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                    DiaChi = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim(),
                    NgayVaoLam = dtpJoinDate.Value
                };

                bool success = _employee == null
                    ? nhanVienBLL.ThemNhanVien(emp)
                    : nhanVienBLL.CapNhatNhanVien(emp);

                if (success)
                {
                    var message = _employee == null
                        ? "Thêm nhân viên thành công!"
                        : "Cập nhật thông tin nhân viên thành công!";

                    MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi lưu dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            // Reset validation styling
            txtName.BackColor = Color.White;

            // Validate name
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                txtName.BackColor = Color.FromArgb(254, 242, 242);
                MessageBox.Show("Vui lòng nhập họ tên nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            // Validate name length
            if (txtName.Text.Trim().Length < 2)
            {
                txtName.BackColor = Color.FromArgb(254, 242, 242);
                MessageBox.Show("Họ tên phải có ít nhất 2 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            // Validate phone if provided
            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                if (!IsValidPhoneNumber(txtPhone.Text.Trim()))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!\nVui lòng nhập số điện thoại có 10-11 chữ số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhone.Focus();
                    return false;
                }
            }

            // Validate join date
            if (dtpJoinDate.Value > DateTime.Now.Date)
            {
                MessageBox.Show("Ngày vào làm không thể lớn hơn ngày hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpJoinDate.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Remove spaces, dashes, dots
            phoneNumber = phoneNumber.Replace(" ", "").Replace("-", "").Replace(".", "");

            // Check if all characters are digits
            if (!phoneNumber.All(char.IsDigit))
                return false;

            // Check length (Vietnamese phone numbers: 10-11 digits)
            return phoneNumber.Length >= 10 && phoneNumber.Length <= 11;
        }
    }
}
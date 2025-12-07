using BookStoreBLL;
using BookStoreDTO;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BookStoreGUI
{
    public partial class CustomerForm : Form
    {
        private readonly KhachHangBLL khachHangBLL = new KhachHangBLL();
        private KhachHangDTO? customer;
        private bool isEditMode = false;

        // Controls
        private TextBox txtTenKH;
        private TextBox txtEmail;
        private TextBox txtSoDienThoai;
        private TextBox txtDiaChi;
        private Button btnSave;
        private Button btnCancel;

        public CustomerForm()
        {
            InitializeComponent();
            this.Text = "Thêm khách hàng mới";
        }

        public CustomerForm(KhachHangDTO customer)
        {
            this.customer = customer;
            this.isEditMode = true;
            InitializeComponent();
            this.Text = "Sửa thông tin khách hàng";
            LoadCustomerData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Title
            Label titleLabel = new Label
            {
                Text = isEditMode ? "SỬA THÔNG TIN KHÁCH HÀNG" : "THÊM KHÁCH HÀNG MỚI",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            // Customer Name
            Label lblTenKH = new Label
            {
                Text = "Tên khách hàng (*)",
                Location = new Point(0, 50),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(51, 51, 51)
            };

            txtTenKH = new TextBox
            {
                Location = new Point(0, 75),
                Size = new Size(440, 25),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Email
            Label lblEmail = new Label
            {
                Text = "Email",
                Location = new Point(0, 115),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(51, 51, 51)
            };

            txtEmail = new TextBox
            {
                Location = new Point(0, 140),
                Size = new Size(440, 25),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Phone Number
            Label lblSoDienThoai = new Label
            {
                Text = "Số điện thoại",
                Location = new Point(0, 180),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(51, 51, 51)
            };

            txtSoDienThoai = new TextBox
            {
                Location = new Point(0, 205),
                Size = new Size(440, 25),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Address
            Label lblDiaChi = new Label
            {
                Text = "Địa chỉ",
                Location = new Point(0, 245),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(51, 51, 51)
            };

            txtDiaChi = new TextBox
            {
                Location = new Point(0, 270),
                Size = new Size(440, 25),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Buttons panel
            Panel buttonPanel = new Panel
            {
                Height = 50,
                Dock = DockStyle.Bottom,
                BackColor = Color.Transparent
            };

            btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(100, 35),
                Location = new Point(220, 10),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            btnSave = new Button
            {
                Text = isEditMode ? "Cập nhật" : "Thêm",
                Size = new Size(100, 35),
                Location = new Point(330, 10),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            // Required field note
            Label noteLabel = new Label
            {
                Text = "(*) Trường bắt buộc",
                Location = new Point(0, 310),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(220, 53, 69)
            };

            // Add controls
            buttonPanel.Controls.AddRange(new Control[] { btnCancel, btnSave });
            
            mainPanel.Controls.AddRange(new Control[] {
                titleLabel, lblTenKH, txtTenKH, lblEmail, txtEmail,
                lblSoDienThoai, txtSoDienThoai, lblDiaChi, txtDiaChi, noteLabel
            });

            this.Controls.AddRange(new Control[] { mainPanel, buttonPanel });

            // Event handlers
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;

            this.ResumeLayout(false);
        }

        private void LoadCustomerData()
        {
            if (customer != null)
            {
                txtTenKH.Text = customer.TenKH;
                txtEmail.Text = customer.Email ?? "";
                txtSoDienThoai.Text = customer.SoDienThoai ?? "";
                txtDiaChi.Text = customer.DiaChi ?? "";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    var customerData = new KhachHangDTO
                    {
                        TenKH = txtTenKH.Text.Trim(),
                        Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                        SoDienThoai = string.IsNullOrWhiteSpace(txtSoDienThoai.Text) ? null : txtSoDienThoai.Text.Trim(),
                        DiaChi = string.IsNullOrWhiteSpace(txtDiaChi.Text) ? null : txtDiaChi.Text.Trim()
                    };

                    bool success;
                    string message;

                    if (isEditMode && customer != null)
                    {
                        customerData.MaKH = customer.MaKH;
                        customerData.NgayTao = customer.NgayTao;
                        success = khachHangBLL.CapNhatKhachHang(customerData);
                        message = success ? "Cập nhật khách hàng thành công!" : "Không thể cập nhật khách hàng!";
                    }
                    else
                    {
                        customerData.NgayTao = DateTime.Now;
                        success = khachHangBLL.ThemKhachHang(customerData);
                        message = success ? "Thêm khách hàng thành công!" : "Không thể thêm khách hàng!";
                    }

                    if (success)
                    {
                        MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            // Check required fields
            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Lỗi nhập liệu", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKH.Focus();
                return false;
            }

            // Validate customer name length
            if (txtTenKH.Text.Trim().Length < 2)
            {
                MessageBox.Show("Tên khách hàng phải có ít nhất 2 ký tự!", "Lỗi nhập liệu", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKH.Focus();
                return false;
            }

            // Validate email if provided
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (!IsValidEmail(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("Email không hợp lệ!", "Lỗi nhập liệu", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }

                // Check email uniqueness
                try
                {
                    int? excludeId = isEditMode && customer != null ? customer.MaKH : null;
                    if (khachHangBLL.KiemTraEmailTonTai(txtEmail.Text.Trim(), excludeId))
                    {
                        MessageBox.Show("Email đã tồn tại trong hệ thống!", "Lỗi nhập liệu", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEmail.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi kiểm tra email: {ex.Message}", "Lỗi", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validate phone number if provided
            if (!string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                if (!IsValidPhoneNumber(txtSoDienThoai.Text.Trim()))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!\nSố điện thoại phải có 10-11 chữ số.", 
                                  "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoDienThoai.Focus();
                    return false;
                }
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            try
            {
                // Remove spaces and common separators
                string cleanPhone = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
                
                // Check if all characters are digits
                if (!Regex.IsMatch(cleanPhone, @"^\d+$"))
                    return false;

                // Check length (10-11 digits for Vietnamese phone numbers)
                return cleanPhone.Length >= 10 && cleanPhone.Length <= 11;
            }
            catch
            {
                return false;
            }
        }
    }
}
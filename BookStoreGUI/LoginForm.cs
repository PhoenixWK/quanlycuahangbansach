using BookStoreBLL;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class LoginForm : Form
    {
        private TaiKhoanBLL taiKhoanBLL;
        public TaiKhoan? DangNhapThanhCong { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            taiKhoanBLL = new TaiKhoanBLL();
            this.KeyPreview = true;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Center the login panel
            CenterLoginPanel();
            
            // Check database connection
            if (!taiKhoanBLL.KiemTraKetNoiDatabase())
            {
                ShowStatus("Không thể kết nối tới cơ sở dữ liệu!", Color.Red);
                btnLogin.Enabled = false;
            }
            
            // Set focus to username textbox
            txtUsername.Focus();
        }

        private void LoginForm_Resize(object sender, EventArgs e)
        {
            CenterLoginPanel();
        }

        private void CenterLoginPanel()
        {
            if (panelLogin != null && panelMain != null)
            {
                panelLogin.Location = new Point(
                    (panelMain.Width - panelLogin.Width) / 2,
                    (panelMain.Height - panelLogin.Height) / 2
                );
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            DangNhap();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DangNhap();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnLogin_MouseEnter(object sender, EventArgs e)
        {
            btnLogin.BackColor = Color.FromArgb(35, 45, 65);
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            btnLogin.BackColor = Color.FromArgb(23, 28, 45);
        }

        private void panelIcon_Paint(object sender, PaintEventArgs e)
        {
            // Draw rounded corners for the icon panel (perfect circle)
            Panel panel = sender as Panel;
            if (panel != null)
            {
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, panel.Width, panel.Height);
                panel.Region = new Region(path);
            }
        }

        private void panelLogin_Paint(object sender, PaintEventArgs e)
        {
            // Draw rounded corners and shadow for login panel
            Panel panel = sender as Panel;
            if (panel != null)
            {
                // Draw rounded rectangle with larger radius for bigger form
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                int radius = 20;
                path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
                path.AddArc(panel.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
                path.AddArc(panel.Width - radius * 2, panel.Height - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(0, panel.Height - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseAllFigures();
                panel.Region = new Region(path);
            }
        }

        private void DangNhap()
        {
            try
            {
                HideStatus();
                
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (string.IsNullOrWhiteSpace(username))
                {
                    ShowStatus("Vui lòng nhập tên đăng nhập!", Color.Red);
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    ShowStatus("Vui lòng nhập mật khẩu!", Color.Red);
                    txtPassword.Focus();
                    return;
                }

                // Disable login button during authentication
                btnLogin.Enabled = false;
                btnLogin.Text = "Đang đăng nhập...";

                var taiKhoan = taiKhoanBLL.DangNhap(username, password);

                if (taiKhoan != null)
                {
                    DangNhapThanhCong = taiKhoan;
                    ShowStatus($"Đăng nhập thành công! Chào mừng {taiKhoan.TenNV}", Color.Green);
                    
                    // Close login form after successful login
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowStatus("Tên đăng nhập hoặc mật khẩu không đúng!", Color.Red);
                    txtPassword.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"Lỗi: {ex.Message}", Color.Red);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập";
            }
        }

        private void ShowStatus(string message, Color color)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;
            lblStatus.Visible = true;
        }

        private void HideStatus()
        {
            lblStatus.Visible = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
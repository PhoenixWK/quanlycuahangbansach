using System;
using System.Drawing;
using System.Windows.Forms;

namespace BookStoreGUI
{
    public partial class MainForm : Form
    {
        private System.Windows.Forms.Timer? timeTimer;
        private CustomerManagementPanel? customerManagementPanel;
        private AuthorManagementPanel? authorManagementPanel;
        private CategoryManagementPanel? categoryManagementPanel;
        private UserControl? currentPanel;

        public MainForm()
        {
            InitializeComponent();
            SetupTimer();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupUI();
            UpdateUserInfo();
        }

        private void SetupUI()
        {
            // Show/hide admin menu based on user role
            if (SessionManager.IsAdmin)
            {
                adminToolStripMenuItem.Visible = true;
            }
            else
            {
                adminToolStripMenuItem.Visible = false;
            }

            // Update welcome message
            lblWelcome.Text = $"Chào mừng {SessionManager.GetUserDisplayName()}\nđến với Hệ thống Quản lý Cửa hàng Sách";
            
            // Center the welcome label
            CenterWelcomeLabel();
        }

        private void SetupTimer()
        {
            timeTimer = new System.Windows.Forms.Timer();
            timeTimer.Interval = 1000; // Update every second
            timeTimer.Tick += TimeTimer_Tick;
            timeTimer.Start();
        }

        private void TimeTimer_Tick(object? sender, EventArgs e)
        {
            lblTime.Text = $"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
        }

        private void UpdateUserInfo()
        {
            lblUserInfo.Text = $"Người dùng: {SessionManager.GetUserDisplayName()} ({SessionManager.GetUserRole()})";
        }

        private void CenterWelcomeLabel()
        {
            lblWelcome.Location = new Point(
                (panelMain.Width - lblWelcome.Width) / 2,
                (panelMain.Height - lblWelcome.Height) / 2
            );
        }

        private void customerManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCustomerManagement();
        }

        private void authorManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAuthorManagement();
        }

        private void categoryManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCategoryManagement();
        }

        private void ShowCustomerManagement()
        {
            HideCurrentPanel();
            
            if (customerManagementPanel == null)
            {
                customerManagementPanel = new CustomerManagementPanel();
            }
            
            ShowPanel(customerManagementPanel);
        }

        private void ShowAuthorManagement()
        {
            HideCurrentPanel();
            
            if (authorManagementPanel == null)
            {
                authorManagementPanel = new AuthorManagementPanel();
            }
            else
            {
                authorManagementPanel.RefreshData();
            }
            
            ShowPanel(authorManagementPanel);
        }

        private void ShowCategoryManagement()
        {
            HideCurrentPanel();
            
            if (categoryManagementPanel == null)
            {
                categoryManagementPanel = new CategoryManagementPanel();
            }
            else
            {
                categoryManagementPanel.RefreshData();
            }
            
            ShowPanel(categoryManagementPanel);
        }

        private void ShowPanel(UserControl panel)
        {
            lblWelcome.Visible = false;
            panel.Dock = DockStyle.Fill;
            panelMain.Controls.Add(panel);
            panel.BringToFront();
            currentPanel = panel;
        }

        private void HideCurrentPanel()
        {
            if (currentPanel != null)
            {
                panelMain.Controls.Remove(currentPanel);
                currentPanel = null;
            }
            
            lblWelcome.Visible = true;
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                SessionManager.Logout();
                this.Hide();

                // Show login form again
                var loginForm = new LoginForm();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    SessionManager.Login(loginForm.DangNhapThanhCong!);
                    SetupUI();
                    UpdateUserInfo();
                    this.Show();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát ứng dụng?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterWelcomeLabel();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                exitToolStripMenuItem_Click(this, EventArgs.Empty);
            }
            else
            {
                base.OnFormClosing(e);
            }
        }
    }
}
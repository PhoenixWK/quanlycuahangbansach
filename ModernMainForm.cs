using BookStoreBLL;
using BookStoreDTO;
using System.Drawing;
using System.Windows.Forms;

namespace BookStoreGUI
{
    public partial class ModernMainForm : Form
    {
        private System.Windows.Forms.Timer? timeTimer;

        public ModernMainForm()
        {
            InitializeComponent();
            SetupTimer();
        }

        private void ModernMainForm_Load(object sender, EventArgs e)
        {
            SetupUI();
            LoadSalesInvoice(); // Load sales/invoice by default for both admin and employees
        }

        private void SetupUI()
        {
            // Update user info in header
            lblUserInfo.Text = SessionManager.GetUserDisplayName();
            lblUserRole.Text = SessionManager.GetUserRole();
            
            // Configure interface based on user role
            if (SessionManager.IsAdmin)
            {
                // Admin: Full access with invoice management
                btnSalesInvoice.Text = "📋  Xem hóa đơn";
                btnCreateInvoice.Text = "🛒  Lập hóa đơn";
                btnCreateInvoice.Visible = false; // Admin không được tạo hóa đơn theo yêu cầu
                btnBooksManagement.Text = "📖  Quản lý Sách";
                btnCustomers.Text = "👥  Quản lý Khách hàng";
                btnEmployees.Visible = true;
                btnStatistics.Visible = true;
                btnAuthors.Visible = true;
                btnGenres.Visible = true;
            }
            else
            {
                // Employee: Limited access - invoice viewing/creation, view books/customers
                btnSalesInvoice.Text = "📋  Xem hóa đơn";
                btnCreateInvoice.Text = "🛒  Lập hóa đơn";
                btnCreateInvoice.Visible = true; // Nhân viên có thể tạo hóa đơn
                btnBooksManagement.Text = "📚  Xem danh mục Sách";
                btnCustomers.Text = "👥  Xem Khách hàng";
                btnEmployees.Visible = false;
                btnStatistics.Visible = false;
                btnAuthors.Visible = false;
                btnGenres.Visible = false;
            }

            // Set default active button
            SetActiveButton(btnSalesInvoice);
        }

        private void SetupTimer()
        {
            timeTimer = new System.Windows.Forms.Timer();
            timeTimer.Interval = 1000;
            timeTimer.Tick += (s, e) => {
                // Update time if needed
            };
            timeTimer.Start();
        }

        private void SetActiveButton(Button activeButton)
        {
            // Reset all buttons
            var buttons = new[] { btnSalesInvoice, btnCreateInvoice, btnBooksManagement, btnAuthors, btnGenres, btnEmployees, btnCustomers, btnStatistics };
            foreach (var btn in buttons)
            {
                btn.BackColor = Color.Transparent;
                btn.ForeColor = Color.FromArgb(100, 100, 100);
            }

            // Set active button
            activeButton.BackColor = Color.FromArgb(240, 240, 255);
            activeButton.ForeColor = Color.FromArgb(96, 91, 255);
        }

        private void LoadBooksManagement()
        {
            panelMainContent.Controls.Clear();

            if (SessionManager.IsAdmin)
            {
                // Admin: Full book management
                var bookManagementPanel = new BookManagementPanel
                {
                    Dock = DockStyle.Fill
                };
                panelMainContent.Controls.Add(bookManagementPanel);
            }
            else
            {
                // Employee: Read-only book viewing
                var employeeBookViewPanel = new EmployeeBookViewPanel
                {
                    Dock = DockStyle.Fill
                };
                panelMainContent.Controls.Add(employeeBookViewPanel);
            }
        }

        private void LoadAuthorsManagement()
        {
            panelMainContent.Controls.Clear();

            // Create our modern AuthorManagementPanel
            var authorManagementPanel = new AuthorManagementPanel
            {
                Dock = DockStyle.Fill
            };

            panelMainContent.Controls.Add(authorManagementPanel);
        }

        private void LoadCustomersManagement()
        {
            panelMainContent.Controls.Clear();

            if (SessionManager.IsAdmin)
            {
                // Admin: Full customer management
                var customerManagementPanel = new CustomerManagementPanel
                {
                    Dock = DockStyle.Fill
                };
                panelMainContent.Controls.Add(customerManagementPanel);
            }
            else
            {
                // Employee: Read-only customer viewing
                var employeeCustomerViewPanel = new EmployeeCustomerViewPanel
                {
                    Dock = DockStyle.Fill
                };
                panelMainContent.Controls.Add(employeeCustomerViewPanel);
            }
        }

        private void LoadEmployeesManagement()
        {
            panelMainContent.Controls.Clear();
            
            var employeePanel = new EmployeeManagementPanel()
            {
                Dock = DockStyle.Fill
            };
            
            panelMainContent.Controls.Add(employeePanel);
        }

        private void LoadStatistics()
        {
            panelMainContent.Controls.Clear();
            
            var statisticsPanel = new StatisticsPanel()
            {
                Dock = DockStyle.Fill
            };
            
            panelMainContent.Controls.Add(statisticsPanel);
        }

        private void LoadSalesInvoice()
        {
            panelMainContent.Controls.Clear();

            if (SessionManager.IsAdmin)
            {
                // Admin: Invoice management with full CRUD capabilities
                var adminInvoicePanel = new AdminInvoiceManagementPanel
                {
                    Dock = DockStyle.Fill
                };
                panelMainContent.Controls.Add(adminInvoicePanel);
            }
            else
            {
                // Employee: Invoice viewing with PDF export but no edit/delete
                var employeeInvoicePanel = new EmployeeInvoiceViewPanel
                {
                    Dock = DockStyle.Fill
                };
                panelMainContent.Controls.Add(employeeInvoicePanel);
            }
        }

        private void btnSalesInvoice_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnSalesInvoice);
            LoadSalesInvoice();
        }

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnCreateInvoice);
            LoadCreateInvoice();
        }

        private void LoadCreateInvoice()
        {
            panelMainContent.Controls.Clear();

            // Only employees can create invoices
            if (!SessionManager.IsAdmin)
            {
                var invoiceManagementPanel = new InvoiceManagementPanel
                {
                    Dock = DockStyle.Fill
                };
                panelMainContent.Controls.Add(invoiceManagementPanel);
            }
            else
            {
                // Admin should not access invoice creation
                MessageBox.Show("Admin không được phép tạo hóa đơn. Chỉ nhân viên mới được tạo hóa đơn.", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBooksManagement_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnBooksManagement);
            LoadBooksManagement();
        }

        private void btnGenres_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnGenres);
            LoadCategoriesManagement();
        }

        private void LoadCategoriesManagement()
        {
            panelMainContent.Controls.Clear();

            // Create our modern CategoryManagementPanel
            var categoryManagementPanel = new CategoryManagementPanel
            {
                Dock = DockStyle.Fill
            };

            panelMainContent.Controls.Add(categoryManagementPanel);
        }

        private void btnAuthors_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnAuthors);
            LoadAuthorsManagement();
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnEmployees);
            LoadEmployeesManagement();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnCustomers);
            LoadCustomersManagement();
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnStatistics);
            LoadStatistics();
        }

        private void btnLogout_Click(object sender, EventArgs e)
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

                var loginForm = new LoginForm();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    SessionManager.Login(loginForm.DangNhapThanhCong!);
                    SetupUI();
                    this.Show();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timeTimer?.Stop();
                timeTimer?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
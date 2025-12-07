namespace BookStoreGUI
{
    partial class ModernMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.btnCustomers = new System.Windows.Forms.Button();
            this.btnEmployees = new System.Windows.Forms.Button();
            this.btnGenres = new System.Windows.Forms.Button();
            this.btnAuthors = new System.Windows.Forms.Button();
            this.btnBooksManagement = new System.Windows.Forms.Button();
            this.btnCreateInvoice = new System.Windows.Forms.Button();
            this.btnSalesInvoice = new System.Windows.Forms.Button();
            this.panelMainContent = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.panelSidebar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.btnLogout);
            this.panelHeader.Controls.Add(this.lblUserRole);
            this.panelHeader.Controls.Add(this.lblUserInfo);
            this.panelHeader.Controls.Add(this.lblHeaderTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1400, 70);
            this.panelHeader.TabIndex = 0;
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.White;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnLogout.Location = new System.Drawing.Point(1300, 20);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(80, 30);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "🚪 Đăng xuất";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // lblUserRole
            // 
            this.lblUserRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserRole.AutoSize = true;
            this.lblUserRole.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUserRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.lblUserRole.Location = new System.Drawing.Point(1050, 35);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(42, 20);
            this.lblUserRole.TabIndex = 2;
            this.lblUserRole.Text = "Admin";
            this.lblUserRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblUserInfo.Location = new System.Drawing.Point(1000, 15);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(98, 25);
            this.lblUserInfo.TabIndex = 1;
            this.lblUserInfo.Text = "Admin User";
            this.lblUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(91)))), ((int)(((byte)(255)))));
            this.lblHeaderTitle.Location = new System.Drawing.Point(30, 20);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(277, 37);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "📚 Quản lý Cửa hàng Sách";
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panelSidebar.Controls.Add(this.btnStatistics);
            this.panelSidebar.Controls.Add(this.btnCustomers);
            this.panelSidebar.Controls.Add(this.btnEmployees);
            this.panelSidebar.Controls.Add(this.btnGenres);
            this.panelSidebar.Controls.Add(this.btnAuthors);
            this.panelSidebar.Controls.Add(this.btnBooksManagement);
            this.panelSidebar.Controls.Add(this.btnCreateInvoice);
            this.panelSidebar.Controls.Add(this.btnSalesInvoice);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 70);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(280, 730);
            this.panelSidebar.TabIndex = 1;
            // 
            // btnStatistics
            // 
            this.btnStatistics.BackColor = System.Drawing.Color.Transparent;
            this.btnStatistics.FlatAppearance.BorderSize = 0;
            this.btnStatistics.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatistics.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnStatistics.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnStatistics.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStatistics.Location = new System.Drawing.Point(20, 450);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnStatistics.Size = new System.Drawing.Size(240, 50);
            this.btnStatistics.TabIndex = 6;
            this.btnStatistics.Text = "📊  Thống kê & Báo cáo";
            this.btnStatistics.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStatistics.UseVisualStyleBackColor = false;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // btnCustomers
            // 
            this.btnCustomers.BackColor = System.Drawing.Color.Transparent;
            this.btnCustomers.FlatAppearance.BorderSize = 0;
            this.btnCustomers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomers.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCustomers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnCustomers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCustomers.Location = new System.Drawing.Point(20, 390);
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnCustomers.Size = new System.Drawing.Size(240, 50);
            this.btnCustomers.TabIndex = 5;
            this.btnCustomers.Text = "👥  Khách hàng";
            this.btnCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCustomers.UseVisualStyleBackColor = false;
            this.btnCustomers.Click += new System.EventHandler(this.btnCustomers_Click);
            // 
            // btnEmployees
            // 
            this.btnEmployees.BackColor = System.Drawing.Color.Transparent;
            this.btnEmployees.FlatAppearance.BorderSize = 0;
            this.btnEmployees.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmployees.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnEmployees.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnEmployees.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmployees.Location = new System.Drawing.Point(20, 330);
            this.btnEmployees.Name = "btnEmployees";
            this.btnEmployees.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnEmployees.Size = new System.Drawing.Size(240, 50);
            this.btnEmployees.TabIndex = 4;
            this.btnEmployees.Text = "👨‍💼  Nhân viên";
            this.btnEmployees.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmployees.UseVisualStyleBackColor = false;
            this.btnEmployees.Click += new System.EventHandler(this.btnEmployees_Click);
            // 
            // btnGenres
            // 
            this.btnGenres.BackColor = System.Drawing.Color.Transparent;
            this.btnGenres.FlatAppearance.BorderSize = 0;
            this.btnGenres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenres.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnGenres.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnGenres.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenres.Location = new System.Drawing.Point(20, 270);
            this.btnGenres.Name = "btnGenres";
            this.btnGenres.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnGenres.Size = new System.Drawing.Size(240, 50);
            this.btnGenres.TabIndex = 3;
            this.btnGenres.Text = "📚  Thể loại";
            this.btnGenres.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenres.UseVisualStyleBackColor = false;
            this.btnGenres.Click += new System.EventHandler(this.btnGenres_Click);
            // 
            // btnAuthors
            // 
            this.btnAuthors.BackColor = System.Drawing.Color.Transparent;
            this.btnAuthors.FlatAppearance.BorderSize = 0;
            this.btnAuthors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAuthors.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnAuthors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnAuthors.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAuthors.Location = new System.Drawing.Point(20, 210);
            this.btnAuthors.Name = "btnAuthors";
            this.btnAuthors.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnAuthors.Size = new System.Drawing.Size(240, 50);
            this.btnAuthors.TabIndex = 2;
            this.btnAuthors.Text = "✍️  Tác giả";
            this.btnAuthors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAuthors.UseVisualStyleBackColor = false;
            this.btnAuthors.Click += new System.EventHandler(this.btnAuthors_Click);
            // 
            // btnBooksManagement
            // 
            this.btnBooksManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.btnBooksManagement.FlatAppearance.BorderSize = 0;
            this.btnBooksManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBooksManagement.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnBooksManagement.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(91)))), ((int)(((byte)(255)))));
            this.btnBooksManagement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBooksManagement.Location = new System.Drawing.Point(20, 150);
            this.btnBooksManagement.Name = "btnBooksManagement";
            this.btnBooksManagement.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnBooksManagement.Size = new System.Drawing.Size(240, 50);
            this.btnBooksManagement.TabIndex = 1;
            this.btnBooksManagement.Text = "📖  Quản lý Sách";
            this.btnBooksManagement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBooksManagement.UseVisualStyleBackColor = false;
            this.btnBooksManagement.Click += new System.EventHandler(this.btnBooksManagement_Click);
            // 
            // btnSalesInvoice
            // 
            this.btnSalesInvoice.BackColor = System.Drawing.Color.Transparent;
            this.btnSalesInvoice.FlatAppearance.BorderSize = 0;
            this.btnSalesInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalesInvoice.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnSalesInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnSalesInvoice.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesInvoice.Location = new System.Drawing.Point(20, 30);
            this.btnSalesInvoice.Name = "btnSalesInvoice";
            this.btnSalesInvoice.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnSalesInvoice.Size = new System.Drawing.Size(240, 50);
            this.btnSalesInvoice.TabIndex = 0;
            this.btnSalesInvoice.Text = "📋  Xem hóa đơn";
            this.btnSalesInvoice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesInvoice.UseVisualStyleBackColor = false;
            this.btnSalesInvoice.Click += new System.EventHandler(this.btnSalesInvoice_Click);
            // 
            // btnCreateInvoice
            // 
            this.btnCreateInvoice.BackColor = System.Drawing.Color.Transparent;
            this.btnCreateInvoice.FlatAppearance.BorderSize = 0;
            this.btnCreateInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateInvoice.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCreateInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnCreateInvoice.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateInvoice.Location = new System.Drawing.Point(20, 90);
            this.btnCreateInvoice.Name = "btnCreateInvoice";
            this.btnCreateInvoice.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnCreateInvoice.Size = new System.Drawing.Size(240, 50);
            this.btnCreateInvoice.TabIndex = 7;
            this.btnCreateInvoice.Text = "🛒  Lập hóa đơn";
            this.btnCreateInvoice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateInvoice.UseVisualStyleBackColor = false;
            this.btnCreateInvoice.Click += new System.EventHandler(this.btnCreateInvoice_Click);
            // 
            // panelMainContent
            // 
            this.panelMainContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainContent.Location = new System.Drawing.Point(280, 70);
            this.panelMainContent.Name = "panelMainContent";
            this.panelMainContent.Size = new System.Drawing.Size(1120, 730);
            this.panelMainContent.TabIndex = 2;
            // 
            // ModernMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.panelMainContent);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "ModernMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ thống Quản lý Cửa hàng Sách";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ModernMainForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelSidebar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panelHeader;
        private Panel panelSidebar;
        private Panel panelMainContent;
        private Label lblHeaderTitle;
        private Label lblUserInfo;
        private Label lblUserRole;
        private Button btnLogout;
        private Button btnSalesInvoice;
        private Button btnCreateInvoice;
        private Button btnBooksManagement;
        private Button btnGenres;
        private Button btnEmployees;
        private Button btnCustomers;
        private Button btnStatistics;
        private Button btnAuthors;
    }
}
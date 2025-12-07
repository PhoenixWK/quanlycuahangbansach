using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class CustomerInvoiceHistoryForm : Form
    {
        private string customerName;
        private List<HoaDonDTO> customerInvoices;

        private DataGridView invoicesGridView = null!;
        private Label statsLabel = null!;

        public CustomerInvoiceHistoryForm(string customerName, List<HoaDonDTO> invoices)
        {
            this.customerName = customerName;
            this.customerInvoices = invoices;

            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = $"Lịch sử mua hàng - {customerName}";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.BackColor = Color.FromArgb(248, 249, 250);

            CreateControls();

            this.ResumeLayout(false);
        }

        private void CreateControls()
        {
            // Header
            Label titleLabel = new Label
            {
                Text = $"📋 Lịch sử mua hàng",
                Location = new Point(20, 20),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255)
            };

            Label customerLabel = new Label
            {
                Text = $"Khách hàng: {customerName}",
                Location = new Point(20, 50),
                Size = new Size(400, 20),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            // Statistics
            statsLabel = new Label
            {
                Location = new Point(20, 80),
                Size = new Size(750, 40),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            // Close button
            Button closeButton = new Button
            {
                Text = "❌ Đóng",
                Location = new Point(700, 20),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => this.Close();

            // Invoices grid
            invoicesGridView = new DataGridView
            {
                Location = new Point(20, 130),
                Size = new Size(750, 320),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 10),
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            SetupGrid();

            this.Controls.AddRange(new Control[] 
            {
                titleLabel, customerLabel, statsLabel, closeButton, invoicesGridView
            });
        }

        private void SetupGrid()
        {
            invoicesGridView.Columns.Clear();

            invoicesGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaHD",
                HeaderText = "Mã HĐ",
                DataPropertyName = "MaHD",
                Width = 80
            });

            invoicesGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgayBan",
                HeaderText = "Ngày mua",
                DataPropertyName = "NgayBan",
                Width = 130,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });

            invoicesGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenNV",
                HeaderText = "Nhân viên bán",
                DataPropertyName = "TenNV",
                Width = 150
            });

            invoicesGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TongTien",
                HeaderText = "Tổng tiền",
                DataPropertyName = "TongTien",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight 
                }
            });

            invoicesGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "KhoangCach",
                HeaderText = "Thời gian",
                Width = 100
            });

            // Style the grid
            invoicesGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255);
            invoicesGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            invoicesGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            invoicesGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }

        private void LoadData()
        {
            try
            {
                // Calculate statistics
                int totalInvoices = customerInvoices.Count;
                decimal totalSpent = customerInvoices.Sum(i => i.TongTien);
                decimal averageSpent = totalInvoices > 0 ? totalSpent / totalInvoices : 0;
                var firstPurchase = customerInvoices.OrderBy(i => i.NgayBan).FirstOrDefault();
                var lastPurchase = customerInvoices.OrderByDescending(i => i.NgayBan).FirstOrDefault();

                statsLabel.Text = $"Tổng số hóa đơn: {totalInvoices} | " +
                                $"Tổng chi tiêu: {totalSpent:N0} VNĐ | " +
                                $"Trung bình/hóa đơn: {averageSpent:N0} VNĐ\n" +
                                $"Lần mua đầu: {firstPurchase?.NgayBan.ToString("dd/MM/yyyy") ?? "N/A"} | " +
                                $"Lần mua cuối: {lastPurchase?.NgayBan.ToString("dd/MM/yyyy") ?? "N/A"}";

                // Load invoices data
                invoicesGridView.Rows.Clear();
                foreach (var invoice in customerInvoices)
                {
                    string timeAgo = GetTimeAgo(invoice.NgayBan);
                    
                    int rowIndex = invoicesGridView.Rows.Add(
                        invoice.MaHD,
                        invoice.NgayBan,
                        invoice.TenNV,
                        invoice.TongTien,
                        timeAgo
                    );

                    // Color coding based on time
                    var row = invoicesGridView.Rows[rowIndex];
                    if ((DateTime.Now - invoice.NgayBan).Days <= 7)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255); // Light blue for recent
                    }
                    else if ((DateTime.Now - invoice.NgayBan).Days <= 30)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(248, 255, 240); // Light green for this month
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;
            
            if (timeSpan.Days > 365)
                return $"{timeSpan.Days / 365} năm trước";
            if (timeSpan.Days > 30)
                return $"{timeSpan.Days / 30} tháng trước";
            if (timeSpan.Days > 0)
                return $"{timeSpan.Days} ngày trước";
            if (timeSpan.Hours > 0)
                return $"{timeSpan.Hours} giờ trước";
            if (timeSpan.Minutes > 0)
                return $"{timeSpan.Minutes} phút trước";
            
            return "Vừa mua";
        }
    }
}
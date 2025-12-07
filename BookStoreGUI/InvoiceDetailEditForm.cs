using System;
using System.Drawing;
using System.Windows.Forms;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class InvoiceDetailEditForm : Form
    {
        private ChiTietHDDTO currentDetail;
        public ChiTietHDDTO UpdatedDetail { get; private set; }

        private Label bookLabel = null!;
        private NumericUpDown quantityNumeric = null!;
        private NumericUpDown priceNumeric = null!;
        private Label totalLabel = null!;
        private Button saveButton = null!;
        private Button cancelButton = null!;

        public InvoiceDetailEditForm(ChiTietHDDTO detail)
        {
            currentDetail = detail;
            UpdatedDetail = new ChiTietHDDTO
            {
                MaHD = detail.MaHD,
                MaSach = detail.MaSach,
                TenSach = detail.TenSach,
                SoLuong = detail.SoLuong,
                DonGia = detail.DonGia
            };

            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Chỉnh sửa Chi tiết hóa đơn";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 250);

            CreateControls();

            this.ResumeLayout(false);
        }

        private void CreateControls()
        {
            // Header
            Label titleLabel = new Label
            {
                Text = "Chỉnh sửa Chi tiết hóa đơn",
                Location = new Point(20, 20),
                Size = new Size(350, 25),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            // Book info (read-only)
            Label bookTitleLabel = new Label
            {
                Text = "Sách:",
                Location = new Point(20, 60),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            bookLabel = new Label
            {
                Location = new Point(100, 60),
                Size = new Size(260, 40),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(80, 80, 80),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(240, 240, 240),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Quantity
            Label quantityLabel = new Label
            {
                Text = "Số lượng:",
                Location = new Point(20, 120),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            quantityNumeric = new NumericUpDown
            {
                Location = new Point(100, 120),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10),
                Minimum = 1,
                Maximum = 1000,
                Value = 1
            };
            quantityNumeric.ValueChanged += CalculateTotal;

            // Price
            Label priceLabel = new Label
            {
                Text = "Đơn giá:",
                Location = new Point(20, 160),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            priceNumeric = new NumericUpDown
            {
                Location = new Point(100, 160),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10),
                Minimum = 0,
                Maximum = 10000000,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };
            priceNumeric.ValueChanged += CalculateTotal;

            Label priceUnitLabel = new Label
            {
                Text = "VNĐ",
                Location = new Point(230, 160),
                Size = new Size(40, 25),
                Font = new Font("Segoe UI", 10)
            };

            // Total (read-only)
            Label totalTitleLabel = new Label
            {
                Text = "Thành tiền:",
                Location = new Point(20, 200),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            totalLabel = new Label
            {
                Location = new Point(100, 200),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 167, 69),
                Text = "0 VNĐ"
            };

            // Buttons
            saveButton = new Button
            {
                Text = "💾 Lưu",
                Location = new Point(200, 230),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Click += SaveButton_Click;

            cancelButton = new Button
            {
                Text = "❌ Hủy",
                Location = new Point(290, 230),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.Click += CancelButton_Click;

            this.Controls.AddRange(new Control[] 
            {
                titleLabel, bookTitleLabel, bookLabel, quantityLabel, quantityNumeric,
                priceLabel, priceNumeric, priceUnitLabel, totalTitleLabel, totalLabel,
                saveButton, cancelButton
            });
        }

        private void LoadData()
        {
            bookLabel.Text = $"{currentDetail.MaSach} - {currentDetail.TenSach}";
            quantityNumeric.Value = currentDetail.SoLuong;
            priceNumeric.Value = currentDetail.DonGia;
            CalculateTotal(null!, EventArgs.Empty);
        }

        private void CalculateTotal(object sender, EventArgs e)
        {
            decimal total = quantityNumeric.Value * priceNumeric.Value;
            totalLabel.Text = $"{total:N0} VNĐ";
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (quantityNumeric.Value <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (priceNumeric.Value <= 0)
                {
                    MessageBox.Show("Đơn giá phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UpdatedDetail.SoLuong = (int)quantityNumeric.Value;
                UpdatedDetail.DonGia = priceNumeric.Value;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
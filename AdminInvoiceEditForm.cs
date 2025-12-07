using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BookStoreBLL;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class AdminInvoiceEditForm : Form
    {
        private readonly HoaDonBLL hoaDonBLL;
        private readonly ChiTietHDBLL chiTietHDBLL;
        private readonly KhachHangBLL khachHangBLL;
        private readonly NhanVienBLL nhanVienBLL;

        private HoaDonDTO currentInvoice;
        private List<ChiTietHDDTO> invoiceDetails = new List<ChiTietHDDTO>();

        private ComboBox customerComboBox = null!;
        private ComboBox employeeComboBox = null!;
        private DateTimePicker datePicker = null!;
        private DataGridView detailsGridView = null!;
        private Label totalLabel = null!;
        private Button saveButton = null!;
        private Button cancelButton = null!;
        private Button editDetailButton = null!;
        private Button deleteDetailButton = null!;

        public AdminInvoiceEditForm(HoaDonDTO invoice)
        {
            hoaDonBLL = new HoaDonBLL();
            chiTietHDBLL = new ChiTietHDBLL();
            khachHangBLL = new KhachHangBLL();
            nhanVienBLL = new NhanVienBLL();
            
            currentInvoice = invoice;
            
            InitializeComponent();
            LoadData();
            LoadInvoiceData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Chỉnh sửa Hóa đơn";
            this.Size = new Size(800, 600);
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
                Text = $"Chỉnh sửa Hóa đơn #{currentInvoice.MaHD}",
                Location = new Point(20, 20),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 53, 69)
            };

            // Customer selection
            Label customerLabel = new Label
            {
                Text = "Khách hàng:",
                Location = new Point(20, 70),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            customerComboBox = new ComboBox
            {
                Location = new Point(130, 70),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Employee selection
            Label employeeLabel = new Label
            {
                Text = "Nhân viên:",
                Location = new Point(400, 70),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            employeeComboBox = new ComboBox
            {
                Location = new Point(510, 70),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Date selection
            Label dateLabel = new Label
            {
                Text = "Ngày bán:",
                Location = new Point(20, 110),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            datePicker = new DateTimePicker
            {
                Location = new Point(130, 110),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm"
            };

            // Details grid
            Label detailsLabel = new Label
            {
                Text = "Chi tiết hóa đơn:",
                Location = new Point(20, 150),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            editDetailButton = new Button
            {
                Text = "✏️ Sửa",
                Location = new Point(600, 150),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            editDetailButton.FlatAppearance.BorderSize = 0;
            editDetailButton.Click += EditDetailButton_Click;

            deleteDetailButton = new Button
            {
                Text = "🗑️ Xóa",
                Location = new Point(690, 150),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            deleteDetailButton.FlatAppearance.BorderSize = 0;
            deleteDetailButton.Click += DeleteDetailButton_Click;

            detailsGridView = new DataGridView
            {
                Location = new Point(20, 185),
                Size = new Size(750, 250),
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

            SetupDetailsGrid();

            // Total
            totalLabel = new Label
            {
                Text = "Tổng tiền: 0 VNĐ",
                Location = new Point(20, 450),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 53, 69)
            };

            // Buttons
            saveButton = new Button
            {
                Text = "💾 Lưu",
                Location = new Point(600, 480),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Click += SaveButton_Click;

            cancelButton = new Button
            {
                Text = "❌ Hủy",
                Location = new Point(690, 480),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.Click += CancelButton_Click;

            this.Controls.AddRange(new Control[] 
            {
                titleLabel, customerLabel, customerComboBox, employeeLabel, employeeComboBox,
                dateLabel, datePicker, detailsLabel, editDetailButton, deleteDetailButton,
                detailsGridView, totalLabel, saveButton, cancelButton
            });
        }

        private void SetupDetailsGrid()
        {
            detailsGridView.Columns.Clear();
            detailsGridView.Columns.Add("MaSach", "Mã sách");
            detailsGridView.Columns.Add("TenSach", "Tên sách");
            detailsGridView.Columns.Add("SoLuong", "Số lượng");
            detailsGridView.Columns.Add("DonGia", "Đơn giá");
            detailsGridView.Columns.Add("ThanhTien", "Thành tiền");

            detailsGridView.Columns["MaSach"].Width = 100;
            detailsGridView.Columns["TenSach"].Width = 250;
            detailsGridView.Columns["SoLuong"].Width = 100;
            detailsGridView.Columns["DonGia"].Width = 120;
            detailsGridView.Columns["ThanhTien"].Width = 120;
        }

        private void LoadData()
        {
            try
            {
                // Load customers
                var customers = khachHangBLL.LayDanhSachKhachHang();
                customerComboBox.DataSource = customers;
                customerComboBox.DisplayMember = "TenKH";
                customerComboBox.ValueMember = "MaKH";

                // Load employees
                var employees = nhanVienBLL.LayDanhSachNhanVien();
                employeeComboBox.DataSource = employees;
                employeeComboBox.DisplayMember = "TenNV";
                employeeComboBox.ValueMember = "MaNV";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadInvoiceData()
        {
            try
            {
                // Set invoice info
                customerComboBox.SelectedValue = currentInvoice.MaKH;
                employeeComboBox.SelectedValue = currentInvoice.MaNV;
                datePicker.Value = currentInvoice.NgayBan;

                // Load invoice details
                invoiceDetails = chiTietHDBLL.LayChiTietTheoMaHD(currentInvoice.MaHD);
                DisplayInvoiceDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải chi tiết hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayInvoiceDetails()
        {
            detailsGridView.Rows.Clear();
            decimal total = 0;

            foreach (var detail in invoiceDetails)
            {
                decimal thanhTien = detail.SoLuong * detail.DonGia;
                detailsGridView.Rows.Add(
                    detail.MaSach,
                    detail.TenSach,
                    detail.SoLuong,
                    detail.DonGia.ToString("N0") + " VNĐ",
                    thanhTien.ToString("N0") + " VNĐ"
                );
                total += thanhTien;
            }

            totalLabel.Text = $"Tổng tiền: {total:N0} VNĐ";
        }

        private void EditDetailButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (detailsGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn chi tiết để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedIndex = detailsGridView.SelectedRows[0].Index;
                var selectedDetail = invoiceDetails[selectedIndex];

                var editForm = new InvoiceDetailEditForm(selectedDetail);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    invoiceDetails[selectedIndex] = editForm.UpdatedDetail;
                    DisplayInvoiceDetails();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteDetailButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (detailsGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn chi tiết để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (invoiceDetails.Count <= 1)
                {
                    MessageBox.Show("Không thể xóa chi tiết cuối cùng của hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedIndex = detailsGridView.SelectedRows[0].Index;
                var selectedDetail = invoiceDetails[selectedIndex];

                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa chi tiết này?\nSách: {selectedDetail.TenSach}\nSố lượng: {selectedDetail.SoLuong}",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    invoiceDetails.RemoveAt(selectedIndex);
                    DisplayInvoiceDetails();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (customerComboBox.SelectedValue == null || employeeComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng và nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (invoiceDetails.Count == 0)
                {
                    MessageBox.Show("Hóa đơn phải có ít nhất một chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Update invoice
                currentInvoice.MaKH = (int)customerComboBox.SelectedValue;
                currentInvoice.MaNV = (int)employeeComboBox.SelectedValue;
                currentInvoice.NgayBan = datePicker.Value;
                currentInvoice.TongTien = invoiceDetails.Sum(d => d.SoLuong * d.DonGia);

                if (hoaDonBLL.CapNhatHoaDon(currentInvoice))
                {
                    // Delete old details
                    var oldDetails = chiTietHDBLL.LayChiTietTheoMaHD(currentInvoice.MaHD);
                    foreach (var detail in oldDetails)
                    {
                        chiTietHDBLL.XoaChiTietHD(detail.MaCTHD);
                    }

                    // Add new details
                    foreach (var detail in invoiceDetails)
                    {
                        detail.MaHD = currentInvoice.MaHD;
                        chiTietHDBLL.ThemChiTietHD(detail);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
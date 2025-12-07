using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BookStoreBLL;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class AdminInvoiceManagementPanel : UserControl
    {
        private readonly HoaDonBLL hoaDonBLL;
        private readonly ChiTietHDBLL chiTietHDBLL;
        private readonly KhachHangBLL khachHangBLL;

        private Panel headerPanel = null!;
        private DateTimePicker fromDatePicker = null!;
        private DateTimePicker toDatePicker = null!;
        private TextBox searchTextBox = null!;
        private ComboBox searchCriteriaComboBox = null!;
        private ComboBox customerFilterComboBox = null!;
        private Button searchButton = null!;
        private Button refreshButton = null!;
        private Button editButton = null!;
        private Button deleteButton = null!;
        private Button viewCustomerInvoicesButton = null!;
        private DataGridView invoicesGridView = null!;
        private Panel invoiceDetailPanel = null!;

        private List<HoaDonDTO> allInvoices = new List<HoaDonDTO>();
        private List<HoaDonDTO> filteredInvoices = new List<HoaDonDTO>();
        private HoaDonDTO? selectedInvoice = null;

        public AdminInvoiceManagementPanel()
        {
            hoaDonBLL = new HoaDonBLL();
            chiTietHDBLL = new ChiTietHDBLL();
            khachHangBLL = new KhachHangBLL();
            
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main container
            this.Size = new Size(1100, 700);
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Padding = new Padding(20);

            CreateHeaderPanel();
            CreateSearchPanel();
            CreateInvoicesGrid();
            CreateInvoiceDetailPanel();

            this.ResumeLayout(false);
        }

        private void CreateHeaderPanel()
        {
            headerPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1060, 60),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label titleLabel = new Label
            {
                Text = "🧾 Quản Lý Hóa Đơn - Admin",
                Location = new Point(20, 15),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 53, 69)
            };

            Label infoLabel = new Label
            {
                Text = "Xem, chỉnh sửa và xóa hóa đơn",
                Location = new Point(550, 20),
                Size = new Size(400, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleRight
            };

            headerPanel.Controls.AddRange(new Control[] { titleLabel, infoLabel });
            this.Controls.Add(headerPanel);
        }

        private void CreateSearchPanel()
        {
            Panel searchPanel = new Panel
            {
                Location = new Point(20, 90),
                Size = new Size(1060, 120),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Date range
            Label fromDateLabel = new Label
            {
                Text = "Từ ngày:",
                Location = new Point(15, 15),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            fromDatePicker = new DateTimePicker
            {
                Location = new Point(15, 45),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 9),
                Format = DateTimePickerFormat.Short
            };
            fromDatePicker.Value = DateTime.Now.AddMonths(-1);

            Label toDateLabel = new Label
            {
                Text = "Đến ngày:",
                Location = new Point(150, 15),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            toDatePicker = new DateTimePicker
            {
                Location = new Point(150, 45),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 9),
                Format = DateTimePickerFormat.Short
            };

            // Customer filter
            Label customerLabel = new Label
            {
                Text = "Khách hàng:",
                Location = new Point(285, 15),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            customerFilterComboBox = new ComboBox
            {
                Location = new Point(285, 45),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Search criteria
            Label criteriaLabel = new Label
            {
                Text = "Tìm theo:",
                Location = new Point(450, 15),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            searchCriteriaComboBox = new ComboBox
            {
                Location = new Point(450, 45),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            searchCriteriaComboBox.Items.AddRange(new string[] 
            { 
                "Mã hóa đơn", 
                "Tên khách hàng", 
                "Tên nhân viên"
            });
            searchCriteriaComboBox.SelectedIndex = 0;

            // Search box
            searchTextBox = new TextBox
            {
                Location = new Point(560, 45),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 9),
                PlaceholderText = "Nội dung tìm kiếm..."
            };

            // Action buttons row 1
            searchButton = new Button
            {
                Text = "🔍 Tìm kiếm",
                Location = new Point(15, 80),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.Click += SearchButton_Click;

            refreshButton = new Button
            {
                Text = "🔄 Làm mới",
                Location = new Point(115, 80),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.Click += RefreshButton_Click;

            editButton = new Button
            {
                Text = "✏️ Sửa",
                Location = new Point(215, 80),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            editButton.FlatAppearance.BorderSize = 0;
            editButton.Click += EditButton_Click;

            deleteButton = new Button
            {
                Text = "🗑️ Xóa",
                Location = new Point(305, 80),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.Click += DeleteButton_Click;

            viewCustomerInvoicesButton = new Button
            {
                Text = "👤 HĐ Khách hàng",
                Location = new Point(395, 80),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(23, 162, 184),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            viewCustomerInvoicesButton.FlatAppearance.BorderSize = 0;
            viewCustomerInvoicesButton.Click += ViewCustomerInvoicesButton_Click;

            Label countLabel = new Label
            {
                Name = "countLabel",
                Text = "Tổng: 0 hóa đơn",
                Location = new Point(750, 85),
                Size = new Size(250, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleRight
            };

            searchPanel.Controls.AddRange(new Control[] { 
                fromDateLabel, fromDatePicker, toDateLabel, toDatePicker,
                customerLabel, customerFilterComboBox,
                criteriaLabel, searchCriteriaComboBox, searchTextBox, 
                searchButton, refreshButton, editButton, deleteButton, 
                viewCustomerInvoicesButton, countLabel 
            });

            this.Controls.Add(searchPanel);
        }

        private void CreateInvoicesGrid()
        {
            Panel gridPanel = new Panel
            {
                Location = new Point(20, 220),
                Size = new Size(720, 460),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label gridLabel = new Label
            {
                Text = "Danh sách hóa đơn",
                Location = new Point(15, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            invoicesGridView = new DataGridView
            {
                Location = new Point(15, 40),
                Size = new Size(690, 405),
                Font = new Font("Segoe UI", 9),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                RowHeadersVisible = false
            };

            invoicesGridView.SelectionChanged += InvoicesGridView_SelectionChanged;

            // Style the grid
            invoicesGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
            invoicesGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            invoicesGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            invoicesGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            gridPanel.Controls.AddRange(new Control[] { gridLabel, invoicesGridView });
            this.Controls.Add(gridPanel);
        }

        private void CreateInvoiceDetailPanel()
        {
            invoiceDetailPanel = new Panel
            {
                Location = new Point(760, 220),
                Size = new Size(320, 460),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            Label detailLabel = new Label
            {
                Text = "Chi tiết hóa đơn",
                Location = new Point(15, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            Label selectPromptLabel = new Label
            {
                Text = "Chọn một hóa đơn để xem chi tiết",
                Location = new Point(15, 50),
                Size = new Size(290, 100),
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleCenter
            };

            invoiceDetailPanel.Controls.AddRange(new Control[] { detailLabel, selectPromptLabel });
            this.Controls.Add(invoiceDetailPanel);
        }

        private void LoadData()
        {
            try
            {
                LoadInvoices();
                LoadCustomersFilter();
                SetupGridColumns();
                DisplayInvoices(allInvoices);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadInvoices()
        {
            allInvoices = hoaDonBLL.LayDanhSachHoaDon();
            filteredInvoices = allInvoices.ToList();
        }

        private void LoadCustomersFilter()
        {
            var customers = khachHangBLL.LayDanhSachKhachHang();
            var allCustomersItem = new KhachHangDTO { MaKH = 0, TenKH = "-- Tất cả khách hàng --" };
            customers.Insert(0, allCustomersItem);
            
            customerFilterComboBox.DataSource = customers;
            customerFilterComboBox.DisplayMember = "TenKH";
            customerFilterComboBox.ValueMember = "MaKH";
        }

        private void SetupGridColumns()
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
                HeaderText = "Ngày bán",
                DataPropertyName = "NgayBan",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            invoicesGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenKH",
                HeaderText = "Khách hàng",
                DataPropertyName = "TenKH",
                Width = 150
            });

            invoicesGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenNV",
                HeaderText = "Nhân viên",
                DataPropertyName = "TenNV",
                Width = 120
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
        }

        private void DisplayInvoices(List<HoaDonDTO> invoices)
        {
            invoicesGridView.DataSource = null;
            invoicesGridView.DataSource = invoices;

            // Update count
            var countLabel = this.Controls.OfType<Panel>()
                .Where(p => p.Location.Y == 90)
                .SelectMany(p => p.Controls.OfType<Label>())
                .FirstOrDefault(l => l.Name == "countLabel");
            
            if (countLabel != null)
                countLabel.Text = $"Tổng: {invoices.Count} hóa đơn - Tổng giá trị: {invoices.Sum(i => i.TongTien):N0} VNĐ";
        }

        private void InvoicesGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (invoicesGridView.SelectedRows.Count > 0)
            {
                selectedInvoice = (HoaDonDTO)invoicesGridView.SelectedRows[0].DataBoundItem;
                ShowInvoiceDetail(selectedInvoice);
            }
        }

        private void ShowInvoiceDetail(HoaDonDTO invoice)
        {
            invoiceDetailPanel.Controls.Clear();
            invoiceDetailPanel.SuspendLayout();

            Label titleLabel = new Label
            {
                Text = "Chi tiết hóa đơn",
                Location = new Point(15, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            // Invoice header info
            int yPosition = 50;
            int spacing = 25;

            var headerInfo = new[]
            {
                ("Mã hóa đơn:", invoice.MaHD.ToString()),
                ("Ngày bán:", invoice.NgayBan.ToString("dd/MM/yyyy HH:mm")),
                ("Khách hàng:", invoice.TenKH ?? "N/A"),
                ("Nhân viên:", invoice.TenNV ?? "N/A"),
                ("Tổng tiền:", $"{invoice.TongTien:N0} VNĐ")
            };

            invoiceDetailPanel.Controls.Add(titleLabel);

            foreach (var (label, value) in headerInfo)
            {
                Label fieldLabel = new Label
                {
                    Text = label,
                    Location = new Point(15, yPosition),
                    Size = new Size(100, 20),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                };

                Label valueLabel = new Label
                {
                    Text = value,
                    Location = new Point(115, yPosition),
                    Size = new Size(190, 20),
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(80, 80, 80)
                };

                invoiceDetailPanel.Controls.AddRange(new Control[] { fieldLabel, valueLabel });
                yPosition += spacing;
            }

            // Invoice details
            try
            {
                var chiTietList = chiTietHDBLL.LayChiTietTheoMaHD(invoice.MaHD);
                
                Label detailsLabel = new Label
                {
                    Text = "Chi tiết sản phẩm:",
                    Location = new Point(15, yPosition),
                    Size = new Size(200, 20),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                };
                invoiceDetailPanel.Controls.Add(detailsLabel);
                yPosition += 30;

                foreach (var chiTiet in chiTietList)
                {
                    Panel itemPanel = new Panel
                    {
                        Location = new Point(15, yPosition),
                        Size = new Size(280, 90),
                        BackColor = Color.FromArgb(248, 249, 250),
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    Label bookLabel = new Label
                    {
                        Text = chiTiet.TenSach ?? "N/A",
                        Location = new Point(10, 5),
                        Size = new Size(260, 20),
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        ForeColor = Color.FromArgb(60, 60, 60)
                    };

                    Label quantityLabel = new Label
                    {
                        Text = $"Số lượng: {chiTiet.SoLuong}",
                        Location = new Point(10, 25),
                        Size = new Size(120, 18),
                        Font = new Font("Segoe UI", 8),
                        ForeColor = Color.FromArgb(80, 80, 80)
                    };

                    Label priceLabel = new Label
                    {
                        Text = $"Đơn giá: {chiTiet.DonGia:N0} VNĐ",
                        Location = new Point(10, 43),
                        Size = new Size(120, 18),
                        Font = new Font("Segoe UI", 8),
                        ForeColor = Color.FromArgb(80, 80, 80)
                    };

                    Label totalLabel = new Label
                    {
                        Text = $"Thành tiền:",
                        Location = new Point(10, 61),
                        Size = new Size(90, 18),
                        Font = new Font("Segoe UI", 8, FontStyle.Bold),
                        ForeColor = Color.FromArgb(40, 167, 69)
                    };

                    Label totalAmountLabel = new Label
                    {
                        Text = $"{chiTiet.SoLuong * chiTiet.DonGia:N0} VNĐ",
                        Location = new Point(95, 61),
                        Size = new Size(175, 18),
                        Font = new Font("Segoe UI", 8, FontStyle.Bold),
                        ForeColor = Color.FromArgb(40, 167, 69)
                    };

                    itemPanel.Controls.AddRange(new Control[] { bookLabel, quantityLabel, priceLabel, totalLabel, totalAmountLabel });
                    invoiceDetailPanel.Controls.Add(itemPanel);
                    yPosition += 100;
                }

                // Customer invoice history button
                Button customerHistoryButton = new Button
                {
                    Text = "📋 Lịch sử mua hàng",
                    Location = new Point(15, yPosition + 10),
                    Size = new Size(280, 30),
                    BackColor = Color.FromArgb(23, 162, 184),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = invoice.MaKH
                };
                customerHistoryButton.FlatAppearance.BorderSize = 0;
                customerHistoryButton.Click += CustomerHistoryButton_Click;

                invoiceDetailPanel.Controls.Add(customerHistoryButton);
            }
            catch (Exception ex)
            {
                Label errorLabel = new Label
                {
                    Text = "Không thể tải chi tiết hóa đơn",
                    Location = new Point(15, yPosition),
                    Size = new Size(280, 20),
                    Font = new Font("Segoe UI", 9, FontStyle.Italic),
                    ForeColor = Color.FromArgb(220, 53, 69)
                };

                invoiceDetailPanel.Controls.Add(errorLabel);
            }

            invoiceDetailPanel.ResumeLayout(false);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                searchTextBox.Text = "";
                searchCriteriaComboBox.SelectedIndex = 0;
                customerFilterComboBox.SelectedIndex = 0;
                fromDatePicker.Value = DateTime.Now.AddMonths(-1);
                toDatePicker.Value = DateTime.Now;
                LoadInvoices();
                DisplayInvoices(allInvoices);
                
                // Clear detail panel
                invoiceDetailPanel.Controls.Clear();
                invoiceDetailPanel.Controls.Add(new Label
                {
                    Text = "Chi tiết hóa đơn",
                    Location = new Point(15, 10),
                    Size = new Size(200, 25),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                });
                invoiceDetailPanel.Controls.Add(new Label
                {
                    Text = "Chọn một hóa đơn để xem chi tiết",
                    Location = new Point(15, 50),
                    Size = new Size(290, 100),
                    Font = new Font("Segoe UI", 10, FontStyle.Italic),
                    ForeColor = Color.FromArgb(108, 117, 125),
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedInvoice == null)
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var editForm = new AdminInvoiceEditForm(selectedInvoice);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadInvoices();
                    DisplayInvoices(filteredInvoices);
                    MessageBox.Show("Cập nhật hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedInvoice == null)
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa hóa đơn {selectedInvoice.MaHD}?\n" +
                    $"Khách hàng: {selectedInvoice.TenKH}\n" +
                    $"Tổng tiền: {selectedInvoice.TongTien:N0} VNĐ\n\n" +
                    "Thao tác này không thể hoàn tác!",
                    "Xác nhận xóa hóa đơn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    // Delete invoice details first
                    var chiTietList = chiTietHDBLL.LayChiTietTheoMaHD(selectedInvoice.MaHD);
                    foreach (var chiTiet in chiTietList)
                    {
                        chiTietHDBLL.XoaChiTietHD(chiTiet.MaCTHD);
                    }

                    // Delete invoice
                    if (hoaDonBLL.XoaHoaDon(selectedInvoice.MaHD))
                    {
                        LoadInvoices();
                        DisplayInvoices(filteredInvoices);
                        MessageBox.Show("Xóa hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Clear detail panel
                        invoiceDetailPanel.Controls.Clear();
                        invoiceDetailPanel.Controls.Add(new Label
                        {
                            Text = "Chi tiết hóa đơn",
                            Location = new Point(15, 10),
                            Size = new Size(200, 25),
                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                            ForeColor = Color.FromArgb(60, 60, 60)
                        });
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewCustomerInvoicesButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedInvoice == null)
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn để xem lịch sử khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var customerInvoices = allInvoices.Where(h => h.MaKH == selectedInvoice.MaKH).OrderByDescending(h => h.NgayBan).ToList();
                
                var historyForm = new CustomerInvoiceHistoryForm(selectedInvoice.TenKH ?? "N/A", customerInvoices);
                historyForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xem lịch sử: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CustomerHistoryButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag is int customerId)
                {
                    var customerInvoices = allInvoices.Where(h => h.MaKH == customerId).OrderByDescending(h => h.NgayBan).ToList();
                    var customer = customerInvoices.FirstOrDefault();
                    
                    var historyForm = new CustomerInvoiceHistoryForm(customer?.TenKH ?? "N/A", customerInvoices);
                    historyForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xem lịch sử: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            string searchText = searchTextBox.Text.Trim().ToLower();
            string searchCriteria = searchCriteriaComboBox.SelectedItem?.ToString() ?? "";
            int selectedCustomerId = (int)(customerFilterComboBox.SelectedValue ?? 0);
            DateTime fromDate = fromDatePicker.Value.Date;
            DateTime toDate = toDatePicker.Value.Date.AddDays(1).AddTicks(-1);

            filteredInvoices = allInvoices.Where(invoice =>
            {
                // Date filter
                bool dateMatch = invoice.NgayBan >= fromDate && invoice.NgayBan <= toDate;

                // Customer filter
                bool customerMatch = selectedCustomerId == 0 || invoice.MaKH == selectedCustomerId;

                // Search filter
                bool searchMatch = string.IsNullOrEmpty(searchText);
                if (!searchMatch)
                {
                    switch (searchCriteria)
                    {
                        case "Mã hóa đơn":
                            searchMatch = invoice.MaHD.ToString().Contains(searchText);
                            break;
                        case "Tên khách hàng":
                            searchMatch = invoice.TenKH?.ToLower().Contains(searchText) == true;
                            break;
                        case "Tên nhân viên":
                            searchMatch = invoice.TenNV?.ToLower().Contains(searchText) == true;
                            break;
                        default:
                            searchMatch = true;
                            break;
                    }
                }

                return dateMatch && customerMatch && searchMatch;
            }).ToList();

            DisplayInvoices(filteredInvoices);
        }
    }
}
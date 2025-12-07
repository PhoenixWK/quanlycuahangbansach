using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BookStoreBLL;
using BookStoreDTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DrawingFont = System.Drawing.Font;

namespace BookStoreGUI
{
    public partial class EmployeeInvoiceViewPanel : UserControl
    {
        private readonly HoaDonBLL hoaDonBLL;
        private readonly ChiTietHDBLL chiTietHDBLL;

        private Panel headerPanel = null!;
        private DateTimePicker fromDatePicker = null!;
        private DateTimePicker toDatePicker = null!;
        private TextBox searchTextBox = null!;
        private ComboBox searchCriteriaComboBox = null!;
        private Button searchButton = null!;
        private Button refreshButton = null!;
        private Button exportPdfButton = null!;
        private DataGridView invoicesGridView = null!;
        private Panel invoiceDetailPanel = null!;

        private List<HoaDonDTO> allInvoices = new List<HoaDonDTO>();
        private List<HoaDonDTO> filteredInvoices = new List<HoaDonDTO>();
        private HoaDonDTO? selectedInvoice = null;

        public EmployeeInvoiceViewPanel()
        {
            hoaDonBLL = new HoaDonBLL();
            chiTietHDBLL = new ChiTietHDBLL();
            
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
                Text = "📄 Danh Sách Hóa Đơn",
                Location = new Point(20, 15),
                Size = new Size(400, 30),
                Font = new DrawingFont("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255)
            };

            Label infoLabel = new Label
            {
                Text = "Xem và xuất hóa đơn PDF",
                Location = new Point(550, 20),
                Size = new Size(400, 20),
                Font = new DrawingFont("Segoe UI", 10, FontStyle.Italic),
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
                Font = new DrawingFont("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            fromDatePicker = new DateTimePicker
            {
                Location = new Point(15, 45),
                Size = new Size(150, 25),
                Font = new DrawingFont("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };
            fromDatePicker.Value = DateTime.Now.AddMonths(-1);

            Label toDateLabel = new Label
            {
                Text = "Đến ngày:",
                Location = new Point(180, 15),
                Size = new Size(80, 25),
                Font = new DrawingFont("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            toDatePicker = new DateTimePicker
            {
                Location = new Point(180, 45),
                Size = new Size(150, 25),
                Font = new DrawingFont("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };

            // Search criteria
            Label criteriaLabel = new Label
            {
                Text = "Tìm theo:",
                Location = new Point(350, 15),
                Size = new Size(80, 25),
                Font = new DrawingFont("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            searchCriteriaComboBox = new ComboBox
            {
                Location = new Point(350, 45),
                Size = new Size(120, 25),
                Font = new DrawingFont("Segoe UI", 10),
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
            Label searchLabel = new Label
            {
                Text = "Nội dung:",
                Location = new Point(480, 15),
                Size = new Size(80, 25),
                Font = new DrawingFont("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            searchTextBox = new TextBox
            {
                Location = new Point(480, 45),
                Size = new Size(200, 25),
                Font = new DrawingFont("Segoe UI", 10),
                PlaceholderText = "Nhập thông tin cần tìm..."
            };

            // Buttons
            searchButton = new Button
            {
                Text = "🔍 Tìm kiếm",
                Location = new Point(700, 45),
                Size = new Size(100, 25),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new DrawingFont("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.Click += SearchButton_Click;

            refreshButton = new Button
            {
                Text = "🔄 Làm mới",
                Location = new Point(810, 45),
                Size = new Size(100, 25),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new DrawingFont("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.Click += RefreshButton_Click;

            exportPdfButton = new Button
            {
                Text = "📄 Xuất PDF",
                Location = new Point(920, 45),
                Size = new Size(100, 25),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new DrawingFont("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            exportPdfButton.FlatAppearance.BorderSize = 0;
            exportPdfButton.Click += ExportPdfButton_Click;

            Label countLabel = new Label
            {
                Name = "countLabel",
                Text = "Tổng: 0 hóa đơn",
                Location = new Point(15, 80),
                Size = new Size(200, 20),
                Font = new DrawingFont("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            searchPanel.Controls.AddRange(new Control[] { 
                fromDateLabel, fromDatePicker, toDateLabel, toDatePicker,
                criteriaLabel, searchCriteriaComboBox, searchLabel, searchTextBox, 
                searchButton, refreshButton, exportPdfButton, countLabel 
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
                Font = new DrawingFont("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            invoicesGridView = new DataGridView
            {
                Location = new Point(15, 40),
                Size = new Size(690, 405),
                Font = new DrawingFont("Segoe UI", 9),
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
            invoicesGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255);
            invoicesGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            invoicesGridView.ColumnHeadersDefaultCellStyle.Font = new DrawingFont("Segoe UI", 9, FontStyle.Bold);
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
                Font = new DrawingFont("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            Label selectPromptLabel = new Label
            {
                Text = "Chọn một hóa đơn để xem chi tiết",
                Location = new Point(15, 50),
                Size = new Size(290, 100),
                Font = new DrawingFont("Segoe UI", 10, FontStyle.Italic),
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
                Font = new DrawingFont("Segoe UI", 12, FontStyle.Bold),
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
                    Font = new DrawingFont("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                };

                Label valueLabel = new Label
                {
                    Text = value,
                    Location = new Point(115, yPosition),
                    Size = new Size(190, 20),
                    Font = new DrawingFont("Segoe UI", 9),
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
                    Font = new DrawingFont("Segoe UI", 10, FontStyle.Bold),
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
                        Font = new DrawingFont("Segoe UI", 9, FontStyle.Bold),
                        ForeColor = Color.FromArgb(60, 60, 60)
                    };

                    Label quantityLabel = new Label
                    {
                        Text = $"Số lượng: {chiTiet.SoLuong}",
                        Location = new Point(10, 25),
                        Size = new Size(120, 18),
                        Font = new DrawingFont("Segoe UI", 8),
                        ForeColor = Color.FromArgb(80, 80, 80)
                    };

                    Label priceLabel = new Label
                    {
                        Text = $"Đơn giá: {chiTiet.DonGia:N0} VNĐ",
                        Location = new Point(10, 43),
                        Size = new Size(120, 18),
                        Font = new DrawingFont("Segoe UI", 8),
                        ForeColor = Color.FromArgb(80, 80, 80)
                    };

                    Label totalLabel = new Label
                    {
                        Text = $"Thành tiền:",
                        Location = new Point(10, 61),
                        Size = new Size(90, 18),
                        Font = new DrawingFont("Segoe UI", 8, FontStyle.Bold),
                        ForeColor = Color.FromArgb(40, 167, 69)
                    };

                    Label totalAmountLabel = new Label
                    {
                        Text = $"{chiTiet.SoLuong * chiTiet.DonGia:N0} VNĐ",
                        Location = new Point(95, 61),
                        Size = new Size(175, 18),
                        Font = new DrawingFont("Segoe UI", 8, FontStyle.Bold),
                        ForeColor = Color.FromArgb(40, 167, 69)
                    };

                    itemPanel.Controls.AddRange(new Control[] { bookLabel, quantityLabel, priceLabel, totalLabel, totalAmountLabel });
                    invoiceDetailPanel.Controls.Add(itemPanel);
                    yPosition += 100;
                }
            }
            catch (Exception ex)
            {
                Label errorLabel = new Label
                {
                    Text = "Không thể tải chi tiết hóa đơn",
                    Location = new Point(15, yPosition),
                    Size = new Size(280, 20),
                    Font = new DrawingFont("Segoe UI", 9, FontStyle.Italic),
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
                    Font = new DrawingFont("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                });
                invoiceDetailPanel.Controls.Add(new Label
                {
                    Text = "Chọn một hóa đơn để xem chi tiết",
                    Location = new Point(15, 50),
                    Size = new Size(290, 100),
                    Font = new DrawingFont("Segoe UI", 10, FontStyle.Italic),
                    ForeColor = Color.FromArgb(108, 117, 125),
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportPdfButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedInvoice == null)
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn để xuất PDF!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"HoaDon_{selectedInvoice.MaHD}_{DateTime.Now:yyyyMMdd}.pdf"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportInvoiceToPdf(selectedInvoice, saveDialog.FileName);
                    MessageBox.Show("Xuất PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Ask if user wants to open the file
                    if (MessageBox.Show("Bạn có muốn mở file PDF vừa tạo?", "Mở file", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(saveDialog.FileName) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportInvoiceToPdf(HoaDonDTO invoice, string filePath)
        {
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            
            document.Open();

            // Fonts
            BaseFont baseFont = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 10);

            // Title
            Paragraph title = new Paragraph("HÓA ĐƠN BÁN HÀNG", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);
            document.Add(new Paragraph("\n"));

            // Store info
            document.Add(new Paragraph("CỬA HÀNG SÁCH ABC", headerFont));
            document.Add(new Paragraph("Địa chỉ: 123 Đường ABC, Quận XYZ, TP.HCM", normalFont));
            document.Add(new Paragraph("Điện thoại: 0123 456 789", normalFont));
            document.Add(new Paragraph("\n"));

            // Invoice info
            document.Add(new Paragraph($"Mã hóa đơn: {invoice.MaHD}", normalFont));
            document.Add(new Paragraph($"Ngày bán: {invoice.NgayBan:dd/MM/yyyy HH:mm}", normalFont));
            document.Add(new Paragraph($"Khách hàng: {invoice.TenKH}", normalFont));
            document.Add(new Paragraph($"Nhân viên bán: {invoice.TenNV}", normalFont));
            document.Add(new Paragraph("\n"));

            // Invoice details table
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 1, 3, 1, 2, 2 });

            // Table header
            AddCell(table, "STT", headerFont, Element.ALIGN_CENTER);
            AddCell(table, "Tên sách", headerFont, Element.ALIGN_CENTER);
            AddCell(table, "SL", headerFont, Element.ALIGN_CENTER);
            AddCell(table, "Đơn giá", headerFont, Element.ALIGN_CENTER);
            AddCell(table, "Thành tiền", headerFont, Element.ALIGN_CENTER);

            // Table content
            var chiTietList = chiTietHDBLL.LayChiTietTheoMaHD(invoice.MaHD);
            int stt = 1;
            foreach (var item in chiTietList)
            {
                AddCell(table, stt.ToString(), normalFont, Element.ALIGN_CENTER);
                AddCell(table, item.TenSach ?? "N/A", normalFont, Element.ALIGN_LEFT);
                AddCell(table, item.SoLuong.ToString(), normalFont, Element.ALIGN_CENTER);
                AddCell(table, $"{item.DonGia:N0}", normalFont, Element.ALIGN_RIGHT);
                AddCell(table, $"{item.SoLuong * item.DonGia:N0}", normalFont, Element.ALIGN_RIGHT);
                stt++;
            }

            document.Add(table);
            document.Add(new Paragraph("\n"));

            // Total
            Paragraph total = new Paragraph($"TỔNG CỘNG: {invoice.TongTien:N0} VNĐ", headerFont);
            total.Alignment = Element.ALIGN_RIGHT;
            document.Add(total);

            document.Add(new Paragraph("\n\n"));
            document.Add(new Paragraph("Cảm ơn quý khách đã mua hàng!", normalFont));

            document.Close();
        }

        private void AddCell(PdfPTable table, string text, iTextSharp.text.Font font, int alignment)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = alignment;
            cell.Padding = 5;
            table.AddCell(cell);
        }

        private void ApplyFilters()
        {
            string searchText = searchTextBox.Text.Trim().ToLower();
            string searchCriteria = searchCriteriaComboBox.SelectedItem?.ToString() ?? "";
            DateTime fromDate = fromDatePicker.Value.Date;
            DateTime toDate = toDatePicker.Value.Date.AddDays(1).AddTicks(-1);

            filteredInvoices = allInvoices.Where(invoice =>
            {
                // Date filter
                bool dateMatch = invoice.NgayBan >= fromDate && invoice.NgayBan <= toDate;

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

                return dateMatch && searchMatch;
            }).ToList();

            DisplayInvoices(filteredInvoices);
        }
    }
}

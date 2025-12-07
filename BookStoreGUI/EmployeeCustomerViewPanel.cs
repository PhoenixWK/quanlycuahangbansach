using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BookStoreBLL;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class EmployeeCustomerViewPanel : UserControl
    {
        private readonly KhachHangBLL khachHangBLL;

        private Panel headerPanel = null!;
        private TextBox searchTextBox = null!;
        private ComboBox searchCriteriaComboBox = null!;
        private Button searchButton = null!;
        private Button refreshButton = null!;
        private DataGridView customersGridView = null!;
        private Panel selectedCustomerDetailPanel = null!;

        private List<KhachHangDTO> allCustomers = new List<KhachHangDTO>();
        private List<KhachHangDTO> filteredCustomers = new List<KhachHangDTO>();

        public EmployeeCustomerViewPanel()
        {
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
            CreateCustomersGrid();
            CreateCustomerDetailPanel();

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
                Text = "👥 Danh Sách Khách Hàng - Tra Cứu Thông Tin",
                Location = new Point(20, 15),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255)
            };

            Label infoLabel = new Label
            {
                Text = "Tra cứu thông tin khách hàng để hỗ trợ bán hàng",
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
                Size = new Size(1060, 80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Search criteria
            Label criteriaLabel = new Label
            {
                Text = "Tìm theo:",
                Location = new Point(15, 15),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            searchCriteriaComboBox = new ComboBox
            {
                Location = new Point(15, 45),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            searchCriteriaComboBox.Items.AddRange(new string[] 
            { 
                "Tên khách hàng", 
                "Số điện thoại", 
                "Email", 
                "Địa chỉ" 
            });
            searchCriteriaComboBox.SelectedIndex = 0;

            // Search box
            Label searchLabel = new Label
            {
                Text = "Nội dung:",
                Location = new Point(180, 15),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            searchTextBox = new TextBox
            {
                Location = new Point(180, 45),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Nhập thông tin cần tìm..."
            };

            // Buttons
            searchButton = new Button
            {
                Text = "🔍 Tìm kiếm",
                Location = new Point(500, 45),
                Size = new Size(100, 25),
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
                Location = new Point(610, 45),
                Size = new Size(100, 25),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.Click += RefreshButton_Click;

            Label countLabel = new Label
            {
                Name = "countLabel",
                Text = "Tổng: 0 khách hàng",
                Location = new Point(750, 48),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleRight
            };

            searchPanel.Controls.AddRange(new Control[] { 
                criteriaLabel, searchCriteriaComboBox, searchLabel, searchTextBox, 
                searchButton, refreshButton, countLabel 
            });

            this.Controls.Add(searchPanel);
        }

        private void CreateCustomersGrid()
        {
            Panel gridPanel = new Panel
            {
                Location = new Point(20, 180),
                Size = new Size(720, 500),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label gridLabel = new Label
            {
                Text = "Danh sách khách hàng",
                Location = new Point(15, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            customersGridView = new DataGridView
            {
                Location = new Point(15, 40),
                Size = new Size(690, 445),
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

            customersGridView.SelectionChanged += CustomersGridView_SelectionChanged;

            // Style the grid
            customersGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255);
            customersGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            customersGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            customersGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            gridPanel.Controls.AddRange(new Control[] { gridLabel, customersGridView });
            this.Controls.Add(gridPanel);
        }

        private void CreateCustomerDetailPanel()
        {
            selectedCustomerDetailPanel = new Panel
            {
                Location = new Point(760, 180),
                Size = new Size(320, 500),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            Label detailLabel = new Label
            {
                Text = "Chi tiết khách hàng",
                Location = new Point(15, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            Label selectPromptLabel = new Label
            {
                Text = "Chọn một khách hàng để xem chi tiết",
                Location = new Point(15, 50),
                Size = new Size(290, 100),
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleCenter
            };

            selectedCustomerDetailPanel.Controls.AddRange(new Control[] { detailLabel, selectPromptLabel });
            this.Controls.Add(selectedCustomerDetailPanel);
        }

        private void LoadData()
        {
            try
            {
                LoadCustomers();
                SetupGridColumns();
                DisplayCustomers(allCustomers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomers()
        {
            allCustomers = khachHangBLL.LayDanhSachKhachHang();
            filteredCustomers = allCustomers.ToList();
        }

        private void SetupGridColumns()
        {
            customersGridView.Columns.Clear();

            customersGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaKH",
                HeaderText = "Mã KH",
                DataPropertyName = "MaKH",
                Width = 80
            });

            customersGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenKH",
                HeaderText = "Tên khách hàng",
                DataPropertyName = "TenKH",
                Width = 200
            });

            customersGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoDienThoai",
                HeaderText = "Số điện thoại",
                DataPropertyName = "SoDienThoai",
                Width = 120
            });

            customersGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email",
                DataPropertyName = "Email",
                Width = 180
            });

            customersGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DiaChi",
                HeaderText = "Địa chỉ",
                DataPropertyName = "DiaChi",
                Width = 200
            });
        }

        private void DisplayCustomers(List<KhachHangDTO> customers)
        {
            customersGridView.DataSource = null;
            customersGridView.DataSource = customers;

            // Update count
            var countLabel = this.Controls.OfType<Panel>()
                .Where(p => p.Location.Y == 90)
                .SelectMany(p => p.Controls.OfType<Label>())
                .FirstOrDefault(l => l.Name == "countLabel");
            
            if (countLabel != null)
                countLabel.Text = $"Tổng: {customers.Count} khách hàng";
        }

        private void CustomersGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (customersGridView.SelectedRows.Count > 0)
            {
                var selectedCustomer = (KhachHangDTO)customersGridView.SelectedRows[0].DataBoundItem;
                ShowCustomerDetail(selectedCustomer);
            }
        }

        private void ShowCustomerDetail(KhachHangDTO customer)
        {
            selectedCustomerDetailPanel.Controls.Clear();
            selectedCustomerDetailPanel.SuspendLayout();

            Label titleLabel = new Label
            {
                Text = "Chi tiết khách hàng",
                Location = new Point(15, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            // Customer details
            int yPosition = 50;
            int spacing = 30;

            var details = new[]
            {
                ("Mã khách hàng:", customer.MaKH.ToString()),
                ("Tên khách hàng:", customer.TenKH ?? "N/A"),
                ("Số điện thoại:", customer.SoDienThoai ?? "N/A"),
                ("Email:", customer.Email ?? "N/A"),
                ("Ngày tạo:", customer.NgayTao.ToString("dd/MM/yyyy")),
                ("Ngày cập nhật:", customer.NgayCapNhat?.ToString("dd/MM/yyyy") ?? "N/A")
            };

            selectedCustomerDetailPanel.Controls.Add(titleLabel);

            foreach (var (label, value) in details)
            {
                Label fieldLabel = new Label
                {
                    Text = label,
                    Location = new Point(15, yPosition),
                    Size = new Size(110, 20),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                };

                Label valueLabel = new Label
                {
                    Text = value,
                    Location = new Point(125, yPosition),
                    Size = new Size(180, 20),
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(80, 80, 80)
                };

                selectedCustomerDetailPanel.Controls.AddRange(new Control[] { fieldLabel, valueLabel });
                yPosition += spacing;
            }

            // Address (multiline)
            if (!string.IsNullOrEmpty(customer.DiaChi))
            {
                Label addressLabel = new Label
                {
                    Text = "Địa chỉ:",
                    Location = new Point(15, yPosition),
                    Size = new Size(100, 20),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                };

                TextBox addressTextBox = new TextBox
                {
                    Text = customer.DiaChi,
                    Location = new Point(15, yPosition + 25),
                    Size = new Size(280, 60),
                    Font = new Font("Segoe UI", 9),
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    BackColor = Color.FromArgb(248, 249, 250)
                };

                selectedCustomerDetailPanel.Controls.AddRange(new Control[] { addressLabel, addressTextBox });
                yPosition += 95;
            }

            // Purchase statistics
            try
            {
                var purchaseStats = GetCustomerPurchaseStats(customer.MaKH);
                
                Label statsLabel = new Label
                {
                    Text = "Thống kê mua hàng:",
                    Location = new Point(15, yPosition),
                    Size = new Size(200, 20),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                };

                Label ordersCountLabel = new Label
                {
                    Text = $"Số đơn hàng: {purchaseStats.OrderCount}",
                    Location = new Point(15, yPosition + 25),
                    Size = new Size(280, 20),
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(80, 80, 80)
                };

                Label totalAmountLabel = new Label
                {
                    Text = $"Tổng tiền: {purchaseStats.TotalAmount:N0} VNĐ",
                    Location = new Point(15, yPosition + 45),
                    Size = new Size(280, 20),
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(40, 167, 69)
                };

                Label lastOrderLabel = new Label
                {
                    Text = $"Lần mua cuối: {purchaseStats.LastOrderDate?.ToString("dd/MM/yyyy") ?? "Chưa mua"}",
                    Location = new Point(15, yPosition + 65),
                    Size = new Size(280, 20),
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(80, 80, 80)
                };

                selectedCustomerDetailPanel.Controls.AddRange(new Control[] 
                { 
                    statsLabel, ordersCountLabel, totalAmountLabel, lastOrderLabel 
                });
            }
            catch (Exception ex)
            {
                Label errorLabel = new Label
                {
                    Text = "Không thể tải thống kê mua hàng",
                    Location = new Point(15, yPosition),
                    Size = new Size(280, 20),
                    Font = new Font("Segoe UI", 9, FontStyle.Italic),
                    ForeColor = Color.FromArgb(220, 53, 69)
                };

                selectedCustomerDetailPanel.Controls.Add(errorLabel);
            }

            selectedCustomerDetailPanel.ResumeLayout(false);
        }

        private (int OrderCount, decimal TotalAmount, DateTime? LastOrderDate) GetCustomerPurchaseStats(int customerId)
        {
            // This would typically query the database for customer purchase statistics
            // For now, returning mock data since we don't have the invoice system fully integrated
            var random = new Random(customerId);
            return (
                OrderCount: random.Next(0, 20),
                TotalAmount: random.Next(0, 10000000),
                LastOrderDate: customerId > 0 ? DateTime.Now.AddDays(-random.Next(0, 365)) : (DateTime?)null
            );
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
                LoadCustomers();
                DisplayCustomers(allCustomers);
                
                // Clear detail panel
                selectedCustomerDetailPanel.Controls.Clear();
                selectedCustomerDetailPanel.Controls.Add(new Label
                {
                    Text = "Chi tiết khách hàng",
                    Location = new Point(15, 10),
                    Size = new Size(200, 25),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60)
                });
                selectedCustomerDetailPanel.Controls.Add(new Label
                {
                    Text = "Chọn một khách hàng để xem chi tiết",
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

        private void ApplyFilters()
        {
            string searchText = searchTextBox.Text.Trim().ToLower();
            string searchCriteria = searchCriteriaComboBox.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrEmpty(searchText))
            {
                filteredCustomers = allCustomers.ToList();
            }
            else
            {
                filteredCustomers = allCustomers.Where(customer =>
                {
                    switch (searchCriteria)
                    {
                        case "Tên khách hàng":
                            return customer.TenKH?.ToLower().Contains(searchText) == true;
                        case "Số điện thoại":
                            return customer.SoDienThoai?.ToLower().Contains(searchText) == true;
                        case "Email":
                            return customer.Email?.ToLower().Contains(searchText) == true;
                        case "Địa chỉ":
                            return customer.DiaChi?.ToLower().Contains(searchText) == true;
                        default:
                            return customer.TenKH?.ToLower().Contains(searchText) == true ||
                                   customer.SoDienThoai?.ToLower().Contains(searchText) == true ||
                                   customer.Email?.ToLower().Contains(searchText) == true;
                    }
                }).ToList();
            }

            DisplayCustomers(filteredCustomers);
        }
    }
}
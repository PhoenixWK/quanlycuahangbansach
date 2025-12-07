using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BookStoreBLL;
using BookStoreDTO;
using DrawingFont = System.Drawing.Font;

namespace BookStoreGUI
{
    public partial class StatisticsPanel : UserControl
    {
        private SachBLL sachBLL;
        private HoaDonBLL hoaDonBLL;
        private KhachHangBLL khachHangBLL;
        private NhanVienBLL nhanVienBLL;

        // UI Components
        private Panel statisticsCardsPanel;
        private Panel chartPanel;
        private ComboBox periodComboBox;
        private Label periodLabel;

        public StatisticsPanel()
        {
            sachBLL = new SachBLL();
            hoaDonBLL = new HoaDonBLL();
            khachHangBLL = new KhachHangBLL();
            nhanVienBLL = new NhanVienBLL();
            
            InitializeComponent();
            LoadStatistics();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 700);
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // Header
            Label headerLabel = new Label
            {
                Text = "📊 THỐNG KÊ TỔNG QUAN",
                Font = new DrawingFont("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(30, 20),
                Size = new Size(400, 40),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(headerLabel);

            // Period selection
            periodLabel = new Label
            {
                Text = "Thời gian:",
                Font = new DrawingFont("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(30, 80),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(periodLabel);

            periodComboBox = new ComboBox
            {
                Location = new Point(120, 78),
                Size = new Size(150, 25),
                Font = new DrawingFont("Segoe UI", 9),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            periodComboBox.Items.AddRange(new string[] { "Tháng này", "3 tháng gần nhất", "6 tháng gần nhất", "Năm nay", "Tất cả" });
            periodComboBox.SelectedIndex = 0;
            periodComboBox.SelectedIndexChanged += PeriodComboBox_SelectedIndexChanged;
            this.Controls.Add(periodComboBox);

            // Statistics cards panel
            statisticsCardsPanel = new Panel
            {
                Location = new Point(30, 120),
                Size = new Size(940, 200),
                BackColor = Color.Transparent
            };
            this.Controls.Add(statisticsCardsPanel);

            // Chart panel
            chartPanel = new Panel
            {
                Location = new Point(30, 340),
                Size = new Size(940, 320),
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            Label chartTitle = new Label
            {
                Text = "📈 BIỂU ĐỒ DOANH THU THEO THÁNG",
                Font = new DrawingFont("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 10),
                Size = new Size(400, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            chartPanel.Controls.Add(chartTitle);
            
            this.Controls.Add(chartPanel);
        }

        private void LoadStatistics()
        {
            try
            {
                CreateStatisticsCards();
                CreateRevenueChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thống kê: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateStatisticsCards()
        {
            statisticsCardsPanel.Controls.Clear();

            // Get statistics data
            var books = sachBLL.LayDanhSachSach();
            var invoices = hoaDonBLL.LayDanhSachHoaDon();
            var customers = khachHangBLL.LayDanhSachKhachHang();
            var employees = nhanVienBLL.LayDanhSachNhanVien();

            // Filter by selected period
            var filteredInvoices = FilterInvoicesByPeriod(invoices);

            // Calculate statistics
            int totalBooks = books.Count;
            int totalCustomers = customers.Count;
            int totalEmployees = employees.Count;
            int totalInvoices = filteredInvoices.Count;
            decimal totalRevenue = filteredInvoices.Sum(h => h.TongTien);
            decimal averageOrderValue = totalInvoices > 0 ? totalRevenue / totalInvoices : 0;

            // Create statistic cards
            var cards = new[]
            {
                new { Title = "Tổng sách", Value = totalBooks.ToString("N0"), Icon = "📚", Color = Color.FromArgb(52, 152, 219) },
                new { Title = "Khách hàng", Value = totalCustomers.ToString("N0"), Icon = "👥", Color = Color.FromArgb(155, 89, 182) },
                new { Title = "Nhân viên", Value = totalEmployees.ToString("N0"), Icon = "👨‍💼", Color = Color.FromArgb(46, 204, 113) },
                new { Title = "Hóa đơn", Value = totalInvoices.ToString("N0"), Icon = "📄", Color = Color.FromArgb(241, 196, 15) },
                new { Title = "Doanh thu", Value = $"{totalRevenue:N0} VNĐ", Icon = "💰", Color = Color.FromArgb(231, 76, 60) },
                new { Title = "Đơn hàng TB", Value = $"{averageOrderValue:N0} VNĐ", Icon = "📊", Color = Color.FromArgb(52, 73, 94) }
            };

            for (int i = 0; i < cards.Length; i++)
            {
                var card = cards[i];
                int x = (i % 3) * 310;
                int y = (i / 3) * 90;
                
                Panel cardPanel = CreateStatisticCard(card.Title, card.Value, card.Icon, card.Color, new Point(x, y));
                statisticsCardsPanel.Controls.Add(cardPanel);
            }
        }

        private Panel CreateStatisticCard(string title, string value, string icon, Color color, Point location)
        {
            Panel card = new Panel
            {
                Size = new Size(300, 80),
                Location = location,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Icon
            Label iconLabel = new Label
            {
                Text = icon,
                Font = new DrawingFont("Segoe UI", 24),
                ForeColor = color,
                Location = new Point(15, 15),
                Size = new Size(50, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(iconLabel);

            // Title
            Label titleLabel = new Label
            {
                Text = title,
                Font = new DrawingFont("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(80, 15),
                Size = new Size(200, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            card.Controls.Add(titleLabel);

            // Value
            Label valueLabel = new Label
            {
                Text = value,
                Font = new DrawingFont("Segoe UI", 14, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(80, 35),
                Size = new Size(200, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            card.Controls.Add(valueLabel);

            return card;
        }

        private void CreateRevenueChart()
        {
            // Clear existing chart
            var existingChart = chartPanel.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "chartArea");
            if (existingChart != null)
            {
                chartPanel.Controls.Remove(existingChart);
            }

            // Create chart area
            Panel chartArea = new Panel
            {
                Name = "chartArea",
                Location = new Point(20, 50),
                Size = new Size(900, 250),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            chartPanel.Controls.Add(chartArea);

            // Get revenue data by month
            var invoices = hoaDonBLL.LayDanhSachHoaDon();
            var monthlyRevenue = GetMonthlyRevenue(invoices);

            // Draw chart
            DrawRevenueChart(chartArea, monthlyRevenue);
        }

        private Dictionary<string, decimal> GetMonthlyRevenue(List<HoaDonDTO> invoices)
        {
            var monthlyRevenue = new Dictionary<string, decimal>();
            
            // Get last 6 months
            for (int i = 5; i >= 0; i--)
            {
                var month = DateTime.Now.AddMonths(-i);
                var monthKey = month.ToString("MM/yyyy");
                
                var revenue = invoices
                    .Where(h => h.NgayBan.Month == month.Month && h.NgayBan.Year == month.Year)
                    .Sum(h => h.TongTien);
                
                monthlyRevenue[monthKey] = revenue;
            }
            
            return monthlyRevenue;
        }

        private void DrawRevenueChart(Panel chartArea, Dictionary<string, decimal> monthlyRevenue)
        {
            chartArea.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.Clear(Color.White);
                
                if (monthlyRevenue.Count == 0) return;

                // Chart settings
                int margin = 40;
                int chartWidth = chartArea.Width - 2 * margin;
                int chartHeight = chartArea.Height - 2 * margin;
                Rectangle chartRect = new Rectangle(margin, margin, chartWidth, chartHeight);
                
                // Draw axes
                g.DrawLine(Pens.Black, chartRect.Left, chartRect.Bottom, chartRect.Right, chartRect.Bottom); // X-axis
                g.DrawLine(Pens.Black, chartRect.Left, chartRect.Top, chartRect.Left, chartRect.Bottom); // Y-axis
                
                // Calculate scale
                decimal maxRevenue = monthlyRevenue.Values.Max();
                if (maxRevenue == 0) maxRevenue = 1;
                
                // Draw bars
                int barWidth = chartWidth / monthlyRevenue.Count - 10;
                int x = chartRect.Left + 5;
                
                foreach (var kvp in monthlyRevenue)
                {
                    decimal revenue = kvp.Value;
                    int barHeight = (int)((revenue / maxRevenue) * chartHeight);
                    
                    Rectangle barRect = new Rectangle(x, chartRect.Bottom - barHeight, barWidth, barHeight);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(52, 152, 219)), barRect);
                    g.DrawRectangle(Pens.Black, barRect);
                    
                    // Draw month label
                    string monthText = kvp.Key;
                    SizeF textSize = g.MeasureString(monthText, new DrawingFont("Segoe UI", 8));
                    g.DrawString(monthText, new DrawingFont("Segoe UI", 8), Brushes.Black, 
                        x + barWidth/2 - textSize.Width/2, chartRect.Bottom + 5);
                    
                    // Draw revenue value
                    string revenueText = $"{revenue:N0}";
                    SizeF revenueSize = g.MeasureString(revenueText, new DrawingFont("Segoe UI", 7));
                    if (barHeight > 20)
                    {
                        g.DrawString(revenueText, new DrawingFont("Segoe UI", 7), Brushes.White, 
                            x + barWidth/2 - revenueSize.Width/2, chartRect.Bottom - barHeight + 5);
                    }
                    
                    x += barWidth + 10;
                }
            };
            
            chartArea.Invalidate();
        }

        private List<HoaDonDTO> FilterInvoicesByPeriod(List<HoaDonDTO> invoices)
        {
            var selectedPeriod = periodComboBox.SelectedItem?.ToString() ?? "Tháng này";
            var now = DateTime.Now;
            
            return selectedPeriod switch
            {
                "Tháng này" => invoices.Where(h => h.NgayBan.Month == now.Month && h.NgayBan.Year == now.Year).ToList(),
                "3 tháng gần nhất" => invoices.Where(h => h.NgayBan >= now.AddMonths(-3)).ToList(),
                "6 tháng gần nhất" => invoices.Where(h => h.NgayBan >= now.AddMonths(-6)).ToList(),
                "Năm nay" => invoices.Where(h => h.NgayBan.Year == now.Year).ToList(),
                _ => invoices
            };
        }

        private void PeriodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateStatisticsCards();
        }
    }
}
using BookStoreBLL;
using BookStoreDTO;

namespace BookStoreGUI
{
    public partial class BookForm : Form
    {
        private SachBLL sachBLL;
        private TacGiaBLL tacGiaBLL;
        private TheLoaiBLL theLoaiBLL;
        private SachDTO? currentBook;
        private bool isAddMode = true;

        private Panel headerPanel = null!;
        private Label titleLabel = null!;
        private string generatedCode = "";
        private Label codeValueLabel = null!;
        private TextBox nameTextBox = null!;
        private ComboBox authorComboBox = null!;
        private ComboBox categoryComboBox = null!;
        private NumericUpDown priceNumeric = null!;
        private NumericUpDown stockNumeric = null!;
        private NumericUpDown pagesNumeric = null!;
        private TextBox publisherTextBox = null!;
        private DateTimePicker publishDatePicker = null!;
        private TextBox descriptionTextBox = null!;
        private PictureBox bookImagePictureBox = null!;
        private Button uploadImageButton = null!;
        private Button saveButton = null!;
        private Button cancelButton = null!;
        private string selectedImagePath = "";
        
        // Auto-display labels
        private Label authorDisplayLabel = null!;
        private Label categoryDisplayLabel = null!;

        public SachDTO? BookResult { get; private set; }

        public BookForm()
        {
            InitializeComponent();
            sachBLL = new SachBLL();
            tacGiaBLL = new TacGiaBLL();
            theLoaiBLL = new TheLoaiBLL();
            currentBook = null;
            LoadComboBoxData();
            LoadBookData(); // Call LoadBookData for new books too
        }

        public BookForm(SachDTO book) : this()
        {
            currentBook = book;
            isAddMode = false;
            LoadBookData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Quản lý Sách";
            this.Size = new Size(1000, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Header Panel with gradient background
            headerPanel = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(96, 91, 255)
            };
            headerPanel.Paint += (s, e) =>
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    headerPanel.ClientRectangle,
                    Color.FromArgb(96, 91, 255),
                    Color.FromArgb(67, 56, 202),
                    45))
                {
                    e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
                }
            };

            titleLabel = new Label
            {
                Text = "📚 Thêm Sách Mới",
                Location = new Point(30, 25),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(titleLabel);

            // Main content panel with responsive layout
            Panel contentPanel = new Panel
            {
                Location = new Point(0, 80),
                Size = new Size(1000, 670),
                BackColor = Color.White,
                Padding = new Padding(30),
                AutoScroll = true
            };

            // Image section
            Panel imagePanel = new Panel
            {
                Location = new Point(30, 30),
                Size = new Size(250, 400),
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label imageLabel = new Label
            {
                Text = "Hình ảnh sách",
                Location = new Point(10, 10),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            bookImagePictureBox = new PictureBox
            {
                Location = new Point(25, 40),
                Size = new Size(200, 280),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };
            SetDefaultImage();

            uploadImageButton = new Button
            {
                Text = "📁 Upload Ảnh",
                Location = new Point(50, 330),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            uploadImageButton.FlatAppearance.BorderSize = 0;
            uploadImageButton.Click += UploadImageButton_Click;

            imagePanel.Controls.AddRange(new Control[] { imageLabel, bookImagePictureBox, uploadImageButton });

            // Form fields panel with responsive flow layout
            FlowLayoutPanel fieldsPanel = new FlowLayoutPanel
            {
                Location = new Point(300, 30),
                Size = new Size(650, 520),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            // Auto-generated code display (read-only)
            Panel codeDisplayPanel = new Panel
            {
                Size = new Size(300, 70),
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5)
            };
            
            Label codeInfoLabel = new Label
            {
                Text = "Mã sách (tự động)",
                Location = new Point(10, 10),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            
            Label codeValueLabel = new Label
            {
                Location = new Point(10, 35),
                Size = new Size(280, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255),
                Text = "Đang tạo mã..."
            };
            
            this.codeValueLabel = codeValueLabel;
            
            codeDisplayPanel.Controls.AddRange(new Control[] { codeInfoLabel, codeValueLabel });

            // Book Name field
            Panel namePanel = new Panel
            {
                Size = new Size(300, 70),
                Margin = new Padding(5)
            };
            
            Label nameLabel = new Label
            {
                Text = "Tên sách *",
                Location = new Point(0, 0),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            
            nameTextBox = new TextBox
            {
                Location = new Point(0, 25),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            namePanel.Controls.AddRange(new Control[] { nameLabel, nameTextBox });

            // Author field
            Panel authorPanel = new Panel
            {
                Size = new Size(300, 95),
                Margin = new Padding(5)
            };
            
            Label authorLabel = new Label
            {
                Text = "Tác giả *",
                Location = new Point(0, 0),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            
            authorComboBox = new ComboBox
            {
                Location = new Point(0, 25),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            authorComboBox.SelectedIndexChanged += AuthorComboBox_SelectedIndexChanged;
            
            authorDisplayLabel = new Label
            {
                Location = new Point(0, 60),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 100),
                Text = ""
            };
            
            authorPanel.Controls.AddRange(new Control[] { authorLabel, authorComboBox, authorDisplayLabel });

            // Category field
            Panel categoryPanel = new Panel
            {
                Size = new Size(300, 95),
                Margin = new Padding(5)
            };
            
            Label categoryLabel = new Label
            {
                Text = "Thể loại *",
                Location = new Point(0, 0),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            
            categoryComboBox = new ComboBox
            {
                Location = new Point(0, 25),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            categoryComboBox.SelectedIndexChanged += CategoryComboBox_SelectedIndexChanged;
            
            categoryDisplayLabel = new Label
            {
                Location = new Point(0, 60),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 100),
                Text = ""
            };
            
            categoryPanel.Controls.AddRange(new Control[] { categoryLabel, categoryComboBox, categoryDisplayLabel });

            // Price field
            Panel pricePanel = new Panel
            {
                Size = new Size(190, 70),
                Margin = new Padding(5)
            };
            
            Label priceLabel = new Label
            {
                Text = "Giá bán (VND) *",
                Location = new Point(0, 0),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            
            priceNumeric = new NumericUpDown
            {
                Location = new Point(0, 25),
                Size = new Size(180, 30),
                Font = new Font("Segoe UI", 11),
                Minimum = 0,
                Maximum = 10000000,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };
            
            pricePanel.Controls.AddRange(new Control[] { priceLabel, priceNumeric });

            // Stock field
            Panel stockPanel = new Panel
            {
                Size = new Size(140, 70),
                Margin = new Padding(5)
            };
            
            Label stockLabel = new Label
            {
                Text = "Số lượng tồn *",
                Location = new Point(0, 0),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            
            stockNumeric = new NumericUpDown
            {
                Location = new Point(0, 25),
                Size = new Size(130, 30),
                Font = new Font("Segoe UI", 11),
                Minimum = 0,
                Maximum = 10000
            };
            
            stockPanel.Controls.AddRange(new Control[] { stockLabel, stockNumeric });

            // Pages field
            Panel pagesPanel = new Panel
            {
                Size = new Size(140, 70),
                Margin = new Padding(5)
            };
            
            Label pagesLabel = new Label
            {
                Text = "Số trang",
                Location = new Point(0, 0),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            
            pagesNumeric = new NumericUpDown
            {
                Location = new Point(0, 25),
                Size = new Size(130, 30),
                Font = new Font("Segoe UI", 11),
                Minimum = 0,
                Maximum = 10000
            };
            
            pagesPanel.Controls.AddRange(new Control[] { pagesLabel, pagesNumeric });

            // Publisher field
            Panel publisherPanel = new Panel
            {
                Size = new Size(300, 70),
                Margin = new Padding(5)
            };
            
            Label publisherLabel = new Label
            {
                Text = "Nhà xuất bản",
                Location = new Point(0, 0),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            
            publisherTextBox = new TextBox
            {
                Location = new Point(0, 25),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            publisherPanel.Controls.AddRange(new Control[] { publisherLabel, publisherTextBox });

            // Publish Date field
            Panel publishDatePanel = new Panel
            {
                Size = new Size(300, 70),
                Margin = new Padding(5)
            };
            
            Label publishDateLabel = new Label
            {
                Text = "Ngày xuất bản",
                Location = new Point(0, 0),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            
            publishDatePicker = new DateTimePicker
            {
                Location = new Point(0, 25),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 11),
                Format = DateTimePickerFormat.Short
            };
            
            publishDatePanel.Controls.AddRange(new Control[] { publishDateLabel, publishDatePicker });

            // Description field (full width)
            Panel descriptionPanel = new Panel
            {
                Size = new Size(620, 110),
                Margin = new Padding(5)
            };
            
            Label descriptionLabel = new Label
            {
                Text = "Mô tả",
                Location = new Point(0, 0),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            
            descriptionTextBox = new TextBox
            {
                Location = new Point(0, 25),
                Size = new Size(620, 80),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            
            descriptionPanel.Controls.AddRange(new Control[] { descriptionLabel, descriptionTextBox });

            // Add all field panels to the flow layout
            fieldsPanel.Controls.AddRange(new Control[] {
                codeDisplayPanel, namePanel, authorPanel, categoryPanel,
                pricePanel, stockPanel, pagesPanel, publisherPanel,
                publishDatePanel, descriptionPanel
            });

            // Buttons panel
            Panel buttonsPanel = new Panel
            {
                Location = new Point(500, 580),
                Size = new Size(200, 50)
            };
            
            saveButton = new Button
            {
                Text = "💾 Lưu",
                Location = new Point(0, 5),
                Size = new Size(90, 40),
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
                Location = new Point(100, 5),
                Size = new Size(90, 40),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.Click += CancelButton_Click;
            
            buttonsPanel.Controls.AddRange(new Control[] { saveButton, cancelButton });

            contentPanel.Controls.AddRange(new Control[] { imagePanel, fieldsPanel, buttonsPanel });

            this.Controls.Add(headerPanel);
            this.Controls.Add(contentPanel);

            this.ResumeLayout(false);
        }

        private void LoadComboBoxData()
        {
            try
            {
                // Load Authors
                var authors = tacGiaBLL.LayDanhSachTacGia();
                authorComboBox.DataSource = authors;
                authorComboBox.DisplayMember = "TenTG";
                authorComboBox.ValueMember = "MaTG";

                // Load Categories
                var categories = theLoaiBLL.LayDanhSachTheLoai();
                categoryComboBox.DataSource = categories;
                categoryComboBox.DisplayMember = "TenTL";
                categoryComboBox.ValueMember = "MaTL";
                
                // Update display labels for default selection
                UpdateDisplayLabels();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBookData()
        {
            try
            {
                if (currentBook != null)
                {
                    titleLabel.Text = "📝 Chỉnh sửa Sách";
                    // Load existing book code
                    generatedCode = currentBook.MaSach ?? "";
                    UpdateCodeDisplay(generatedCode);
                    nameTextBox.Text = currentBook.TenSach;
                
                // Set ComboBox values by ID - need to ensure correct data type matching
                if (int.TryParse(currentBook.MaTG, out int authorId))
                {
                    authorComboBox.SelectedValue = authorId;
                }
                
                if (int.TryParse(currentBook.MaTL, out int categoryId))
                {
                    categoryComboBox.SelectedValue = categoryId;
                }
                
                // Update display labels after setting values
                UpdateDisplayLabels();
                priceNumeric.Value = (decimal)(currentBook.GiaBan ?? 0);
                stockNumeric.Value = (decimal)(currentBook.SoLuongTon ?? 0);
                // Load publisher information from database
                publisherTextBox.Text = currentBook.NhaXuatBan ?? "";
                // Set default values for fields not in basic database
                pagesNumeric.Value = currentBook.SoTrang ?? 0;
                publishDatePicker.Value = currentBook.NgayXuatBan ?? DateTime.Now;
                descriptionTextBox.Text = currentBook.MoTa ?? "";
                
                // Load book image if exists
                LoadBookImage(currentBook.HinhAnh);
                
                // Update display labels manually since SelectedValue doesn't always trigger events
                UpdateDisplayLabels();
            }
            else
            {
                // Generate new book code
                try
                {
                    generatedCode = sachBLL.TaoMaSachMoi();
                    UpdateCodeDisplay(generatedCode);
                }
                catch (Exception ex)
                {
                    generatedCode = "S001"; // Fallback code
                    UpdateCodeDisplay(generatedCode);
                    MessageBox.Show("Cảnh báo: Không thể tự động tạo mã sách. Mã mặc định được sử dụng: " + generatedCode + "\nLỗi: " + ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCodeDisplay(string code)
        {
            if (codeValueLabel != null && !string.IsNullOrEmpty(code))
            {
                codeValueLabel.Text = code;
            }
        }

        private void UpdateDisplayLabels()
        {
            // Update author display  
            if (authorComboBox.SelectedItem is TacGiaDTO selectedAuthor)
            {
                authorDisplayLabel.Text = $"Tác giả: {selectedAuthor.TenTG}";
            }
            else if (authorComboBox.SelectedValue != null)
            {
                // Try to find by MaTG value when setting programmatically
                var authors = (List<TacGiaDTO>)authorComboBox.DataSource;
                var author = authors?.FirstOrDefault(a => a.MaTG.ToString() == authorComboBox.SelectedValue.ToString());
                if (author != null)
                {
                    authorDisplayLabel.Text = $"Tác giả: {author.TenTG}";
                }
                else
                {
                    authorDisplayLabel.Text = "";
                }
            }
            else
            {
                authorDisplayLabel.Text = "";
            }

            // Update category display  
            if (categoryComboBox.SelectedItem is TheLoaiDTO selectedCategory)
            {
                categoryDisplayLabel.Text = $"Thể loại: {selectedCategory.TenTL}";
            }
            else if (categoryComboBox.SelectedValue != null)
            {
                // Try to find by MaTL value when setting programmatically
                var categories = (List<TheLoaiDTO>)categoryComboBox.DataSource;
                var category = categories?.FirstOrDefault(c => c.MaTL.ToString() == categoryComboBox.SelectedValue.ToString());
                if (category != null)
                {
                    categoryDisplayLabel.Text = $"Thể loại: {category.TenTL}";
                }
                else
                {
                    categoryDisplayLabel.Text = "";
                }
            }
            else
            {
                categoryDisplayLabel.Text = "";
            }
        }        private void LoadBookImage(string? imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    string fullPath = imagePath;
                    
                    // If it's a relative path, make it absolute
                    if (!Path.IsPathRooted(imagePath))
                    {
                        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                        fullPath = Path.Combine(baseDir, imagePath);
                    }
                    
                    if (File.Exists(fullPath))
                    {
                        // Create a copy of the image to avoid file locking
                        using (var originalImage = Image.FromFile(fullPath))
                        {
                            bookImagePictureBox.Image = new Bitmap(originalImage);
                        }
                        selectedImagePath = fullPath;
                    }
                    else
                    {
                        // If file doesn't exist, set default image
                        SetDefaultImage();
                    }
                }
                catch (Exception ex)
                {
                    // Log the error and use default image
                    Console.WriteLine($"Error loading image: {ex.Message}");
                    SetDefaultImage();
                }
            }
            else
            {
                SetDefaultImage();
            }
        }

        private void SetDefaultImage()
        {
            // Set a default book cover image
            var defaultImage = new Bitmap(200, 280);
            using (Graphics g = Graphics.FromImage(defaultImage))
            {
                g.FillRectangle(Brushes.LightGray, 0, 0, 200, 280);
                g.DrawString("No Image", new Font("Segoe UI", 12), Brushes.DarkGray, 60, 130);
            }
            bookImagePictureBox.Image = defaultImage;
        }

        private void UploadImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openDialog.Title = "Chọn hình ảnh sách";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create images directory if it doesn't exist
                        string imagesDir = Path.Combine(Application.StartupPath, "BookImages");
                        if (!Directory.Exists(imagesDir))
                        {
                            Directory.CreateDirectory(imagesDir);
                        }

                        // Copy file to images directory
                        string fileName = Path.GetFileName(openDialog.FileName);
                        string destinationPath = Path.Combine(imagesDir, DateTime.Now.Ticks + "_" + fileName);
                        File.Copy(openDialog.FileName, destinationPath, true);

                        // Display image
                        bookImagePictureBox.Image = Image.FromFile(destinationPath);
                        selectedImagePath = destinationPath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi tải ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure code is generated if somehow missing
                if (string.IsNullOrWhiteSpace(generatedCode))
                {
                    try
                    {
                        generatedCode = sachBLL.TaoMaSachMoi();
                        UpdateCodeDisplay(generatedCode);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: Không thể tạo mã sách!\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(generatedCode))
                {
                    MessageBox.Show("Lỗi: Không thể tạo mã sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(nameTextBox.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nameTextBox.Focus();
                    return;
                }

                if (authorComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn tác giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    authorComboBox.Focus();
                    return;
                }

                if (categoryComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn thể loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    categoryComboBox.Focus();
                    return;
                }

                if (priceNumeric.Value <= 0)
                {
                    MessageBox.Show("Giá bán phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    priceNumeric.Focus();
                    return;
                }

                // Create book object with basic fields only
                var book = new SachDTO
                {
                    MaSach = generatedCode,
                    TenSach = nameTextBox.Text.Trim(),
                    MaTG = authorComboBox.SelectedValue.ToString(),
                    MaTL = categoryComboBox.SelectedValue.ToString(),
                    GiaBan = priceNumeric.Value,
                    SoLuongTon = (int)stockNumeric.Value,
                    TrangThai = true,
                    // Set default values for optional fields
                    SoTrang = (int)pagesNumeric.Value,
                    NhaXuatBan = publisherTextBox.Text.Trim(),
                    NgayXuatBan = publishDatePicker.Value,
                    MoTa = descriptionTextBox.Text.Trim(),
                    HinhAnh = selectedImagePath
                };

                bool result;
                if (isAddMode)
                {
                    result = sachBLL.ThemSach(book);
                    if (result)
                    {
                        MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    result = sachBLL.CapNhatSach(book);
                    if (result)
                    {
                        MessageBox.Show("Cập nhật sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (result)
                {
                    BookResult = book;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AuthorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDisplayLabels();
        }

        private void CategoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDisplayLabels();
        }
    }
}
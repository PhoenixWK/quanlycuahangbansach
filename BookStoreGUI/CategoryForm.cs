using BookStoreBLL;
using BookStoreDTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BookStoreGUI
{
    public partial class CategoryForm : Form
    {
        private readonly TheLoaiBLL theLoaiBLL = new TheLoaiBLL();
        private TheLoaiDTO currentCategory;
        private bool isEditMode = false;

        private TextBox nameTextBox;
        private Button saveButton;
        private Button cancelButton;

        public CategoryForm()
        {
            InitializeComponent();
            this.Text = "Thêm thể loại mới";
        }

        public CategoryForm(TheLoaiDTO category) : this()
        {
            currentCategory = category;
            isEditMode = true;
            this.Text = "Chỉnh sửa thông tin thể loại";
            LoadCategoryData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Size = new Size(550, 320);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Font = new Font("Segoe UI", 10);

            // Main panel with modern styling
            Panel mainPanel = new Panel
            {
                Size = new Size(500, 270),
                Location = new Point(25, 25),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            
            // Add shadow effect
            mainPanel.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, mainPanel.Width - 1, mainPanel.Height - 1);
                using (var pen = new Pen(Color.FromArgb(220, 220, 220), 2))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            // Header panel with gradient background
            Panel headerPanel = new Panel
            {
                Size = new Size(500, 60),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(40, 167, 69)
            };

            // Title label with icon
            Label titleLabel = new Label
            {
                Text = isEditMode ? "✏️ CHỈNH SỬA THỂ LOẠI" : "➕ THÊM THỂ LOẠI MỚI",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 18),
                Size = new Size(460, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Content panel
            Panel contentPanel = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(460, 120),
                BackColor = Color.Transparent
            };

            // Name label with modern styling
            Label nameLabel = new Label
            {
                Text = "Tên thể loại *",
                Location = new Point(0, 20),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51)
            };

            // Name textbox with improved styling
            nameTextBox = new TextBox
            {
                Location = new Point(0, 50),
                Size = new Size(460, 35),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                MaxLength = 100,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(51, 51, 51)
            };
            
            // Add border styling
            nameTextBox.Enter += (s, e) => nameTextBox.BackColor = Color.FromArgb(248, 249, 250);
            nameTextBox.Leave += (s, e) => nameTextBox.BackColor = Color.White;

            // Validation label with better positioning
            Label validationLabel = new Label
            {
                Text = "⚠️ Tên thể loại không được để trống và phải có ít nhất 2 ký tự",
                Location = new Point(0, 90),
                Size = new Size(460, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            // Button panel with better spacing
            Panel buttonPanel = new Panel
            {
                Location = new Point(20, 210),
                Size = new Size(460, 50),
                BackColor = Color.Transparent
            };

            // Save button with improved styling
            saveButton = new Button
            {
                Text = isEditMode ? "✔️ Cập nhật" : "➕ Lưu",
                Size = new Size(120, 40),
                Location = new Point(220, 5),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(33, 136, 56);
            saveButton.Click += SaveButton_Click;

            // Cancel button with improved styling
            cancelButton = new Button
            {
                Text = "❌ Hủy",
                Size = new Size(120, 40),
                Location = new Point(350, 5),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(88, 97, 105);
            cancelButton.Click += CancelButton_Click;

            // Add controls to panels
            headerPanel.Controls.Add(titleLabel);
            contentPanel.Controls.AddRange(new Control[] { nameLabel, nameTextBox, validationLabel });
            buttonPanel.Controls.AddRange(new Control[] { saveButton, cancelButton });
            mainPanel.Controls.AddRange(new Control[] { headerPanel, contentPanel, buttonPanel });

            // Add main panel to form
            this.Controls.Add(mainPanel);

            // Set tab order
            nameTextBox.TabIndex = 0;
            saveButton.TabIndex = 1;
            cancelButton.TabIndex = 2;

            // Set default button
            this.AcceptButton = saveButton;
            this.CancelButton = cancelButton;

            // Add Enter key handling with validation
            nameTextBox.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (ValidateInput())
                    {
                        SaveButton_Click(null, null);
                    }
                    e.Handled = true;
                }
            };

            this.ResumeLayout(false);
        }

        private void LoadCategoryData()
        {
            if (currentCategory != null)
            {
                nameTextBox.Text = currentCategory.TenTL;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                string categoryName = nameTextBox.Text.Trim();

                if (isEditMode)
                {
                    // Update existing category
                    currentCategory.TenTL = categoryName;
                    
                    if (theLoaiBLL.CapNhatTheLoai(currentCategory))
                    {
                        MessageBox.Show("Cập nhật thông tin thể loại thành công!", "Thông báo",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật thông tin thể loại!", "Lỗi",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Add new category
                    var newCategory = new TheLoaiDTO { TenTL = categoryName };
                    
                    if (theLoaiBLL.ThemTheLoai(newCategory))
                    {
                        MessageBox.Show("Thêm thể loại mới thành công!", "Thông báo",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm thể loại mới!", "Lỗi",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateInput()
        {
            string categoryName = nameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("Vui lòng nhập tên thể loại!", "Thông báo", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nameTextBox.Focus();
                return false;
            }

            if (categoryName.Length < 2)
            {
                MessageBox.Show("Tên thể loại phải có ít nhất 2 ký tự!", "Thông báo", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nameTextBox.Focus();
                return false;
            }

            return true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            nameTextBox.Focus();
            
            if (isEditMode)
            {
                nameTextBox.SelectAll();
            }
        }
    }
}
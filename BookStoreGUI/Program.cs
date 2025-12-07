namespace BookStoreGUI;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        // Set application-wide settings
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try
        {
            // Show login form first
            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Login successful, set session and show main form
                    SessionManager.Login(loginForm.DangNhapThanhCong!);
                    
                    // Run main application
                    Application.Run(new ModernMainForm());
                }
                // If login was cancelled or failed, application will exit
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Lỗi khởi động ứng dụng: {ex.Message}",
                "Lỗi hệ thống",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }    
}
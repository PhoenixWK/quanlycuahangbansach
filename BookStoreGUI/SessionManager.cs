using BookStoreDTO;

namespace BookStoreGUI
{
    public static class SessionManager
    {
        public static TaiKhoan? CurrentUser { get; private set; }
        public static bool IsLoggedIn => CurrentUser != null;
        public static bool IsAdmin => CurrentUser?.VaiTro == "Admin";
        public static bool IsNhanVien => CurrentUser?.VaiTro == "NhanVien";

        public static void Login(TaiKhoan user)
        {
            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static string GetUserDisplayName()
        {
            return CurrentUser?.TenNV ?? "Unknown User";
        }

        public static string GetUserRole()
        {
            return CurrentUser?.VaiTro ?? "Unknown";
        }
    }
}
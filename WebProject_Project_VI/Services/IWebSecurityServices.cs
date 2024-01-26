namespace WebProject_Project_VI.Services
{
    public interface IWebSecurityServices
    {
        Task<bool> LoginAsync(string username, string password);
        public bool Logout();
        bool ChangePassword(string username, string oldPassword, string newPassword);
        bool IsAuthenticated();
        bool IsAdmin();
        bool IsUser();
        string GetUserName();
    }
}

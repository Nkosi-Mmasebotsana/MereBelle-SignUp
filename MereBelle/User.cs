namespace MereBelle
{
    public class User
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string PasswordSalt { get; set; } = "";
        public string ConfirmationCode { get; set; } = "";
        public bool IsConfirmed { get; set; } = false;
    }
}
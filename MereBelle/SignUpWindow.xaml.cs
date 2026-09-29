using System;
using System.Security.Cryptography;
using System.Windows;

namespace MereBelle
{
    public partial class SignUpWindow : Window
    {
        public SignUpWindow() => InitializeComponent();

        private async void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";
            string name = FullNameBox.Text.Trim();
            string email = EmailBox.Text.Trim();
            string pw = PasswordBox.Password;
            string confirm = ConfirmBox.Password;

            if (name == "" || email == "" || pw == "")
            {
                ErrorText.Text = "Please fill in all fields.";
                return;
            }
            if (!LooksLikeEmail(email))
            {
                ErrorText.Text = "Please enter a valid email address.";
                return;
            }
            if (pw != confirm)
            {
                ErrorText.Text = "Passwords do not match.";
                return;
            }
            if (UserStore.EmailExists(email))
            {
                ErrorText.Text = "An account with that email already exists.";
                return;
            }

            CreateButton.IsEnabled = false;
            CreateButton.Content = "Sending...";
            try
            {
                string code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
                var (hash, salt) = PasswordHasher.Hash(pw);

                var user = new User
                {
                    FullName = name,
                    Email = email,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    ConfirmationCode = code,
                    IsConfirmed = false
                };
                UserStore.AddUser(user);


                await EmailService.SendConfirmationAsync(name, email, code);

                var verifyWindow = new VerifyCodeWindow(email);
                verifyWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorText.Text = "Could not send the confirmation email. " + ex.Message;
            }
            finally
            {
                CreateButton.IsEnabled = true;
                CreateButton.Content = "Create Account";
            }
        }

        private static bool LooksLikeEmail(string email)
        {
            int at = email.IndexOf('@');
            int dot = email.LastIndexOf('.');
            return at > 0 && dot > at + 1 && dot < email.Length - 1;
        }
    }
}
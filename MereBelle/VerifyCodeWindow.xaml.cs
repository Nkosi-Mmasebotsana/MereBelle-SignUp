using System.Windows;

namespace MereBelle
{
    public partial class VerifyCodeWindow : Window
    {
        private readonly string _email;

        public VerifyCodeWindow(string email)
        {
            InitializeComponent();
            _email = email;
            InstructionText.Text = $"Enter the 6-digit code we sent to {email}";
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            string code = CodeBox.Text.Trim();

            if (code.Length != 6)
            {
                ErrorText.Text = "Please enter the 6-digit code.";
                return;
            }

            if (UserStore.ConfirmUser(_email, code))
            {
                MessageBox.Show("Your Mère Belle account is now active. Welcome!",
                    "Account confirmed", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                ErrorText.Text = "That code doesn't match. Please try again.";
            }
        }
    }
}
using System;
using System.Windows.Forms;
using RevitPlugIn.Utils;

namespace RevitPlugIn.Forms
{
    public partial class LoginForm : System.Windows.Forms.Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void UploadButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UserNameTextBox.Text) && !string.IsNullOrEmpty(passwordTextBox.Text))
            {
                UploadButton.Enabled = false;
                if (await UserUtils.ManuallyLogin(UserNameTextBox.Text, passwordTextBox.Text))
                {
                    MessageBox.Show("Logged in! as " + UserUtils.LoggedInUser.Role.ToString());
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Your credentials aren't correct.");
                }
                UploadButton.Enabled = true;
            }
            else
            {
                MessageBox.Show("Please enter username and password.");
            }
        }
    }
}

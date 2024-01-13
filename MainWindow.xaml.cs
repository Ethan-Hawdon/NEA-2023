using System.Windows;
using System.Data.OleDb;
 
namespace NEA_Document
{ 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new();
            login.Show();
            Close();
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            textBoxUsername.Text = "";
            textBoxPassword.Password = "";
            textBoxPwdConfirm.Password = "";
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxUsername.Text.Length == 0)
            {
                errormessage.Text = "Enter an username.";
                textBoxUsername.Focus();
            }
            else
            {
                string username = textBoxUsername.Text;
                string password = textBoxPassword.Password;
                if (textBoxPassword.Password.Length == 0)
                {
                    errormessage.Text = "Enter password.";
                    textBoxPassword.Focus();
                }
                else if (textBoxPwdConfirm.Password.Length == 0)
                {
                    errormessage.Text = "Enter Confirm password.";
                    textBoxPwdConfirm.Focus();
                }
                else if (textBoxPassword.Password != textBoxPwdConfirm.Password)
                {
                    errormessage.Text = "Please ensure that both passwords are the same.";
                    textBoxPwdConfirm.Focus();
                }
                else
                {
                    errormessage.Text = "";
                    OleDbConnection connection = new(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\ethan_grb0ji1\OneDrive\Desktop\School\NEA\NEA Database 2002.mdb; Persist Security Info = True"); //Creates a connection to the access database I'm using
                    connection.Open();
                    string commandStr = "insert into Users ([Username], [Password]) values (?,?)";
                    OleDbCommand createUser = new(commandStr, connection);
                    createUser.Parameters.AddWithValue("username", username);
                    createUser.Parameters.AddWithValue("password", password);
 
                    createUser.ExecuteNonQuery();
                    connection.Close();
 
                    Login login = new();
                    login.Show();
                    Close();
 
                }
            }
        }
    }
}

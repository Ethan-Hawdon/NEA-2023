using System;
using System.Windows;
using System.Data.OleDb;
using System.Diagnostics;
 
namespace NEA_Document
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        MainWindow registration = new MainWindow();
        /*
         * Login constructor - initialise and focus on username text box to allow typing.
         */
        public Login()
        {
            InitializeComponent();
            usernameTextBox.Focus();   
        }
        /* Finds a User from the Users based on their username and password
         * Returns a User object
         */
        private User findUser(string username, string password)
        {
            User? user = null;
            string queryString = "SELECT * from Users WHERE Username = @username AND Password = @password";
            try
            {
                using (OleDbConnection connection = new OleDbConnection(DatabaseUtils.CONNECTION_STRING))
                {
                    OleDbCommand command = new OleDbCommand(queryString, connection);
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("password", password);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
 
                    while (reader.Read())
                    {
                        user = new User(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetInt32(3));
                    }
                    reader.Close();
                }
                // The connection is automatically closed when the
                // code exits the using block.
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to connect to data source" + ex.Message);
                MessageBox.Show("Failed to connect to data source");
            }
            return user;
        }
 
        /*
         * Respond to login click by performing validation - 
         * start the quiz if successful, otherwise show error message to the user.
         */
        private void loginButtonClicked(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Length == 0)
            {
                errormessage.Text = "Enter a username.";
                usernameTextBox.Focus();
            }
            else if (passwordTextBox.Password.Length == 0)
            {
                errormessage.Text = "Enter a password.";
                passwordTextBox.Focus();
            }
            else
            {
                string username = usernameTextBox.Text;
                string password = passwordTextBox.Password;
                User user = findUser(username, password);
                if (user != null)
                {
                    StartQuiz startQuiz = new StartQuiz(user);
                    startQuiz.Show();
                    Close();
                }
                else
                {
                    errormessage.Text = $"Unable to login. Please enter an existing username/password.";
                }
            }
        }
        /*
         * show the registration window
         */
        private void showRegistration(object sender, RoutedEventArgs e)
        {
            registration.Show();
            Close();
        }
    }
}

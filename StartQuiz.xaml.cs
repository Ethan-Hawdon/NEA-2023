using System;
using System.Diagnostics;
using System.Windows;
 
namespace NEA_Document
{
    /// <summary>
    /// Interaction logic for StartQuiz.xaml
    /// </summary>
    public partial class StartQuiz : Window
    {
        /* the logged in User */
        private User? user = null;
 
        /*
         * Constructor for StartQuiz
         */
        public StartQuiz(User currentUser)
        {
            InitializeComponent();
            user = currentUser;
            Debug.WriteLine("StartQuiz user " + user);
            SkillRating.Text = Convert.ToString(user.SkillRating);
            WelcomeTxt.Text += " " + user.Username +"!";
        }
        
        /*
         * responds to click of the begin test button by showing the TestQuestions window */
        private void beginTest(object sender, RoutedEventArgs e)
        {
            TestQuestions TestQuestions = new(user);
            TestQuestions.Show();
            Close();
        }
 
        /* close window*/
        private void cancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

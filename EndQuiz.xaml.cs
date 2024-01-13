using System;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows;
 
namespace NEA_Document
{
    /// <summary>
    /// Interaction logic for EndQuiz.xaml
    /// </summary>
    public partial class EndQuiz : Window
    {
        private int maxSkillRating = 0;
        private User? user = null;
 
        /*
         * Constructor for the EndQuiz window. Calculates the Users new skill rating and saves to database.
         */
        public EndQuiz(int score, User currentUser)
        {
            InitializeComponent();
            user = currentUser;
            maxSkillRating = findHighestDifficulty() - 2;
            if (score >= 8)
            {
                results.Text = ("The test is now complete. You have scored " + score + "/10 which has increased your Skill Rating by 2!");
                if(currentUser.SkillRating <= maxSkillRating - 2)
                {
                    currentUser.SkillRating = currentUser.SkillRating + 2;
                }
                
            }
            else if(score >= 5 && score < 8)
            {
                results.Text = ("The test is now complete. You have scored " + score + "/10 which has increased your Skill Rating by 1!");
                if (currentUser.SkillRating <= maxSkillRating - 1)
                {
                    currentUser.SkillRating = currentUser.SkillRating + 1;
                }
            }
            else if(score > 2 && score < 5)
            {
                results.Text = ("The test is now complete. You have scored " + score + "/10 which has not altered your Skill Rating.");
            }
            else
            {
                results.Text = ("The test is now complete. You have scored " + score + "/10 which has decreased your Skill Rating by 1.");
                if(currentUser.SkillRating > 0)
                {
                    currentUser.SkillRating = currentUser.SkillRating - 1;
                }
            }
            updateSkillRating(currentUser);
            SkillRating.Text = Convert.ToString(currentUser.SkillRating);
        }
 
 
        /*
         * Update the User's skill rating on the database
         */
        private void updateSkillRating(User currentUser)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(DatabaseUtils.CONNECTION_STRING))
                {
                    connection.Open();
                    string commandStr = "UPDATE Users SET SkillRating = @newSkillRating WHERE Username = @currentUser";
                    OleDbCommand updateSkillRating = new(commandStr, connection);
                    updateSkillRating.Parameters.AddWithValue("newSkillRating", currentUser.SkillRating);
                    updateSkillRating.Parameters.AddWithValue("currentUser", currentUser.Username);
                    updateSkillRating.ExecuteNonQuery();
                }
                // The connection is automatically closed when the
                // code exits the using block.
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to connect to data source" + ex.Message);
                MessageBox.Show("Failed to connect to data source");
            }
        }
 
        /*
         * Find the max difficulty level from the database
         */
        private int findHighestDifficulty()
        {
            string queryString = "SELECT MAX(Difficulty) from Questions";
            int maxDifficulty = 0;
            try
            {
                using (OleDbConnection connection = new OleDbConnection(DatabaseUtils.CONNECTION_STRING))
                {
                    OleDbCommand command = new OleDbCommand(queryString, connection);
                    connection.Open();
                    maxDifficulty = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to connect to data source" + ex.Message);
                MessageBox.Show("Failed to connect to data source");
            }
            return maxDifficulty;
        }
 
        /*
         * Trigger a retry of the test
         */
        private void RetryButton(object sender, RoutedEventArgs e)
        {
            TestQuestions testQuestions = new(user);
            testQuestions.Show();
            Close();
        }
 
        /*
         * Finish and close the window
         */
        private void FinishButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

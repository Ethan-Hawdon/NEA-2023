using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
 
namespace NEA_Document
{
    /// <summary>
    /// Interaction logic for TestQuestions.xaml
    /// </summary>
    public partial class TestQuestions : Window
    {
        private int score = 0;
        private int currentQuestionNumber = 0;
        private List<Question> questions = new List<Question>();
        private User? user = null;
 
        /*
         * Constructor for TestQuestions.
         * Finds all questions appropriate for the User's skill rating, sorts them by difficulty and displays the first (easiest) question 
         */
        public TestQuestions(User currentUser)
        {
            InitializeComponent();
            score = 0;
            user = currentUser;
            questions = findQuestions(user.SkillRating);
            foreach(var question in questions)
            {
                Debug.WriteLine(question);
                questions = bubbleSort(questions);
            }
            displayQuestion();
        }
 
        /*
         * Sort the Questions by difficulty level. We do not care if consecutive Questions have the same difficulty level.
         */
        private List<Question> bubbleSort(List<Question> questions)
        {
            //I considered using List.Sort but have implemented my own bubble sort instead
 
            Question? store;
            Question[] questionArray = questions.ToArray();
 
            for (int j = 0; j < questionArray.Length - 1; j++)
            {
                for (int i = 0; i < questionArray.Length - 1; i++)
                {
                    if (questionArray[i].Difficulty > questionArray[i + 1].Difficulty)
                    {
                        store = questionArray[i + 1];
                        questionArray[i + 1] = questionArray[i];
                        questionArray[i] = store;
                    }
                }
            }
            for (int i = 0; i < questionArray.Length; i++)
            {
                Debug.WriteLine(questionArray[i]);
            }
            return questionArray.ToList();
        }
 
        /*
         * Display a question, answers are randomly allocated to each answer button 
         * This avoids putting the correct answer on the same button each time
         * If there are no more questions then shows the EndQuiz page
         */
        private void displayQuestion()
        {
            if (currentQuestionNumber < questions.Count)
            {   
                Question question = questions[currentQuestionNumber];
                txtQuestion.Text = question.QuestionTxt;
                String[] randomisedAnswers = randomiseAnswers(question);
                ans1.Content = randomisedAnswers[0];
                ans2.Content = randomisedAnswers[1];
                ans3.Content = randomisedAnswers[2];
                ans4.Content = randomisedAnswers[3];
                
            } 
            else
            {
                EndQuiz endQuiz = new(score, user);
                endQuiz.Show();
                Close();
            }
        }
 
        /*
         * randomise the answers to ensure that the correct answer is not in the same place continuously
         */
        private String[] randomiseAnswers(Question question)
        {
            Random rnd = new Random();
            String[] answers = new String[4];
            answers[0] = question.CorrectAnswer;
            answers[1] = question.IncorrectAnswer1;
            answers[2] = question.IncorrectAnswer2;
            answers[3] = question.IncorrectAnswer3;
 
            return answers.OrderBy(x => rnd.Next()).ToArray();
        }
 
        /* Finds a List of Questions from the Questions table based on skill rating of a User
         * Returns a List of Questions
         */
        private List<Question> findQuestions(int skillRating)
        {
            List<Question> questions = new List<Question>();
            int difficultyUpperLimit = skillRating + 2;
            int difficultyLowerLimit = skillRating - 2;
 
            string queryString = "SELECT * from Questions WHERE Difficulty <= @difficultyUpperLimit AND Difficulty >= difficultyLowerLimit";
            try
            {
                using (OleDbConnection connection = new OleDbConnection(DatabaseUtils.CONNECTION_STRING))
                {
                    OleDbCommand command = new OleDbCommand(queryString, connection);
                    command.Parameters.AddWithValue("difficultyUpperLimit", difficultyUpperLimit);
                    command.Parameters.AddWithValue("difficultyLowerLimit", difficultyLowerLimit);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    Question? question = null;
                    //read each row from the database and construct a Question object, add it to the list of Questions
                    while (reader.Read() && questions.Count < 10)
                    {
                        question = new Question(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetInt32(6));
                        questions.Add(question);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to connect to data source" + ex.Message);
                MessageBox.Show("Failed to connect to data source");
            }
            Debug.WriteLine("found " + questions.Count + "questions");
            return questions;
        }
 
        private void checkAnswer(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Answered with " + e.OriginalSource); //Used to see whether the button they pressed was the correct answer or not
            string givenAnswer = Convert.ToString((e.OriginalSource as Button).Content);
            if (givenAnswer == questions[currentQuestionNumber].CorrectAnswer)
            {
                //answered correctly so increment the score
                score++;
            }
            currentQuestionNumber++;
            displayQuestion(); //display next question
        }
    }
}

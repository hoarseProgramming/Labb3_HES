using Labb3_HES.ViewModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace Labb3_HES.Views
{
    /// <summary>
    /// Interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        private List<Button> buttons = new();
        private Color colorAliceBlue = new() { R = 240, G = 248, B = 255, A = 255 };
        private Color colorGreen = new() { G = 255, A = 255 };
        private Color colorRed = new() { R = 255, A = 255 };

        public PlayerView()
        {
            InitializeComponent();
            buttons.Add(buttonOne);
            buttons.Add(buttonTwo);
            buttons.Add(buttonThree);
            buttons.Add(buttonFour);

        }
        public void OnAnswerRecievedMessageRecieved(object sender, EventArgs args)
        {
            foreach (Button button in buttons)
            {
                string answerInButton = (button.Content as TextBlock).Text;
                bool answerInButtonIsCorrectAnswer = answerInButton == (DataContext as PlayerViewModel).ActiveQuestion.CorrectAnswer;
                bool answerInButtonIsGivenAnswer = answerInButton == (DataContext as PlayerViewModel).GivenAnswer;

                if (answerInButtonIsCorrectAnswer)
                {
                    button.Background = new SolidColorBrush(colorGreen);
                }
                else if (answerInButtonIsGivenAnswer && !answerInButtonIsCorrectAnswer)
                {
                    button.Background = new SolidColorBrush(colorRed);
                }
            }
        }

        public void OnNoAnswerRecievedMessageRecieved(object sender, EventArgs args)
        {
            foreach (Button button in buttons)
            {
                string answerInButton = (string)button.Content;
                bool answerInButtonIsCorrectAnswer = answerInButton == (DataContext as PlayerViewModel).ActiveQuestion.CorrectAnswer;

                if (answerInButtonIsCorrectAnswer)
                {
                    button.Background = new SolidColorBrush(colorGreen);
                }
            }
        }
        public void OnIsNewQuestionMessageRecieved(object sender, EventArgs args)
        {
            foreach (Button button in buttons)
            {
                button.Background = new SolidColorBrush(colorAliceBlue);
            }
        }
    }
}

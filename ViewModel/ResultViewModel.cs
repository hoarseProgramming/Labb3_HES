namespace Labb3_HES.ViewModel
{
    internal class ResultViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        private bool _isResultMode;
        public bool IsResultMode
        {
            get => _isResultMode;
            set
            {
                _isResultMode = value;
                RaisePropertyChanged();
            }
        }
        private int _correctAnswers;
        public int CorrectAnswersGiven
        {
            get => _correctAnswers;
            set
            {
                _correctAnswers = value;
                RaisePropertyChanged();
            }
        }
        private int _numberOfQuestionsInPack;
        public int NumberOfQuestionsInPack
        {
            get => _numberOfQuestionsInPack;
            set
            {
                _numberOfQuestionsInPack = value;
                RaisePropertyChanged();
            }
        }

        private string _resultMessage;
        public string ResultMessage
        {
            get => _resultMessage;
            set
            {
                _resultMessage = value;
                RaisePropertyChanged();
            }
        }

        public event EventHandler IsResultModeMessage;
        public ResultViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public void OnIsOtherModeMessageRecieved(object sender, EventArgs args)
        {
            IsResultMode = false;
        }
        public void EnableResultMode(int numberOfQuestionsInPack, int correctAnswersGiven)
        {
            NumberOfQuestionsInPack = numberOfQuestionsInPack;
            CorrectAnswersGiven = correctAnswersGiven;

            double resultPercentage = (double)correctAnswersGiven / numberOfQuestionsInPack;

            ResultMessage = resultPercentage switch
            {
                < 0.5 => "Try a bit harder, champ",
                < 0.8 => "Good job, you could do better though.",
                < 1 => "Come on! Almost a perfect score!",
                _ => "Very impressive!",
            };

            IsResultModeMessage.Invoke(this, EventArgs.Empty);
            IsResultMode = true;
        }
    }
}

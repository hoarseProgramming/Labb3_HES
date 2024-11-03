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
        public event EventHandler IsResultModeMessage;
        public ResultViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            mainWindowViewModel.ConstructorsAreLoadedMessage += OnConstructorsAreLoadedMessageRecieved;
        }
        public void OnConstructorsAreLoadedMessageRecieved(object sender, EventArgs args)
        {
            mainWindowViewModel.PlayerViewModel.IsPlayerModeMessage += OnIsOtherModeMessageRecieved;
            mainWindowViewModel.ConfigurationViewModel.IsConfigurationModeMessage += OnIsOtherModeMessageRecieved;
        }
        public void OnIsOtherModeMessageRecieved(object sender, EventArgs args)
        {
            IsResultMode = false;
        }
        public void EnableResultMode()
        {
            IsResultModeMessage.Invoke(this, EventArgs.Empty);
            NumberOfQuestionsInPack = mainWindowViewModel.PlayerViewModel.NumberOfQuestionsInPack;
            CorrectAnswersGiven = mainWindowViewModel.PlayerViewModel.CorrectAnswersGiven;
            IsResultMode = true;
        }
    }
}

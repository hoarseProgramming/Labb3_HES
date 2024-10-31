using Labb3_HES.Command;
using Labb3_HES.Model;

namespace Labb3_HES.ViewModel
{
    class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        private bool _isConfigurationMode;
        public bool IsConfigurationMode
        {
            get => _isConfigurationMode;
            set
            {
                _isConfigurationMode = value;
                RaisePropertyChanged();
            }
        }

        //private bool _isVisible;
        //public bool IsVisible
        //{
        //    get => _isVisible;
        //    set
        //    {
        //        _isVisible = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private bool _isEnabled;
        //public bool IsEnabled
        //{
        //    get => _isEnabled;
        //    set
        //    {
        //        _isEnabled = value;
        //        RaisePropertyChanged();
        //    }
        //}
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel.ActivePack; }

        public Question _activeQuestion;

        public Question ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
                RemoveQuestionCommand.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public DelegateCommand EnableConfigurationCommand { get; }

        public event EventHandler IsConfigurationModeMessage;

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            IsConfigurationMode = true;
            AddQuestionCommand = new DelegateCommand(AddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);
            EnableConfigurationCommand = new DelegateCommand(EnableConfiguration, CanEnableConfiguration);

            mainWindowViewModel.ConstructorsIsLoadedMessage += OnConstructorsIsLoadedMessageRecieved;


        }
        public void OnConstructorsIsLoadedMessageRecieved(object sender, EventArgs args)
        {
            mainWindowViewModel.PlayerViewModel.IsPlayerModeMessage += OnIsPlayerModeMessageRecieved;
        }
        private void EnableConfiguration(object obj)
        {
            SendIsConfigurationModeMessage();
            ReverseIsConfigurationMode();
            //EnableConfigurationCommand.RaiseCanExecuteChanged();           
            //mainWindowViewModel.PlayerViewModel.PlayQuizCommand.RaiseCanExecuteChanged();
        }

        private bool CanEnableConfiguration(object? arg) => !IsConfigurationMode;
        private void SendIsConfigurationModeMessage()
        {
            IsConfigurationModeMessage.Invoke(this, EventArgs.Empty);
        }
        private void ReverseIsConfigurationMode()
        {
            IsConfigurationMode = !IsConfigurationMode;
            EnableConfigurationCommand.RaiseCanExecuteChanged();
        }
        public void OnIsPlayerModeMessageRecieved(object sender, EventArgs args)
        {
            ReverseIsConfigurationMode();
            EnableConfigurationCommand.RaiseCanExecuteChanged();
        }

        private bool CanRemoveQuestion(object? arg) => ActiveQuestion != null;

        private void RemoveQuestion(object obj)
        {
            ActivePack.Questions.Remove(ActiveQuestion);
            mainWindowViewModel.PlayerViewModel.PlayQuizCommand.RaiseCanExecuteChanged();
        }
        private void AddQuestion(object obj)
        {
            ActivePack.Questions.Add(new Question());
            mainWindowViewModel.PlayerViewModel.PlayQuizCommand.RaiseCanExecuteChanged();
        }
    }
}

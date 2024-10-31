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
        public DelegateCommand OpenPackOptionsCommand { get; }

        public event EventHandler ShouldOpenPackOptionsMessage;
        public DelegateCommand EnableConfigurationCommand { get; }

        public event EventHandler IsConfigurationModeMessage;

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            IsConfigurationMode = true;

            AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);
            OpenPackOptionsCommand = new DelegateCommand(OpenPackOptions, CanOpenPackOptions);
            EnableConfigurationCommand = new DelegateCommand(EnableConfiguration, CanEnableConfiguration);

            mainWindowViewModel.ConstructorsAreLoadedMessage += OnConstructorsAreLoadedMessageRecieved;

        }


        private void AddQuestion(object obj)
        {
            ActivePack.Questions.Add(new Question());
            mainWindowViewModel.PlayerViewModel.PlayQuizCommand.RaiseCanExecuteChanged();
        }
        private bool CanAddQuestion(object? arg) => IsConfigurationMode;
        private void RemoveQuestion(object obj)
        {
            ActivePack.Questions.Remove(ActiveQuestion);
            mainWindowViewModel.PlayerViewModel.PlayQuizCommand.RaiseCanExecuteChanged();
        }
        private bool CanRemoveQuestion(object? arg) => ActiveQuestion != null && IsConfigurationMode;
        private void OpenPackOptions(object obj)
        {
            ShouldOpenPackOptionsMessage.Invoke(this, EventArgs.Empty);
        }
        private bool CanOpenPackOptions(object? arg) => IsConfigurationMode;

        public void OnConstructorsAreLoadedMessageRecieved(object sender, EventArgs args)
        {
            mainWindowViewModel.PlayerViewModel.IsPlayerModeMessage += OnIsPlayerModeMessageRecieved;
        }
        private void EnableConfiguration(object obj)
        {
            SendIsConfigurationModeMessage();
            ReverseIsConfigurationMode();
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
            AddQuestionCommand.RaiseCanExecuteChanged();
            RemoveQuestionCommand.RaiseCanExecuteChanged();
            OpenPackOptionsCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.CreateNewPackCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.DeletePackCommand.RaiseCanExecuteChanged();
        }
        public void OnIsPlayerModeMessageRecieved(object sender, EventArgs args)
        {
            ReverseIsConfigurationMode();
            EnableConfigurationCommand.RaiseCanExecuteChanged();
        }


    }
}

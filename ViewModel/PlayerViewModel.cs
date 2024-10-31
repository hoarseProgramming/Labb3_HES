using Labb3_HES.Command;
using System.Windows.Threading;

namespace Labb3_HES.ViewModel
{
    class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        private bool _isPlayerMode;
        public bool IsPlayerMode
        {
            get => _isPlayerMode;
            set
            {
                _isPlayerMode = value;
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

        private DispatcherTimer timer;

        private int _timeLimit;

        public int TimeLimit
        {
            get => _timeLimit;
            private set
            {
                _timeLimit = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand UpdateLabelCommand { get; }
        public DelegateCommand PlayQuizCommand { get; }
        public event EventHandler IsPlayerModeMessage;
        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            //TODO: Make logic for countdown, this can't be in constructor!
            //int timeInSeconds = mainWindowViewModel.ActivePack.TimeLimitInSeconds;
            ////TimeLimit = timeInSeconds;
            //TimeLimit = 5;

            //timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += Timer_Tick;
            ////timer.Start();

            UpdateLabelCommand = new DelegateCommand(UpdateLabel, CanUpdateLabel);
            PlayQuizCommand = new DelegateCommand(PlayQuiz, CanPlayQuiz);

            mainWindowViewModel.ConstructorsIsLoadedMessage += OnConstructorsIsLoadedMessageRecieved;

        }

        private void OnConstructorsIsLoadedMessageRecieved(object? sender, EventArgs e)
        {
            mainWindowViewModel.ConfigurationViewModel.IsConfigurationModeMessage += OnIsConfigurationModeMessageRecieved;
        }

        private void PlayQuiz(object obj)
        {
            SendIsPlayerModeMessage();
            ReverseIsPlayerMode();
            //PlayQuizCommand.RaiseCanExecuteChanged();

            //mainWindowViewModel.ConfigurationViewModel.ReverseIsConfigurationMode();
            //mainWindowViewModel.ConfigurationViewModel.EnableConfigurationCommand.RaiseCanExecuteChanged();

        }
        private bool CanPlayQuiz(object? arg) => mainWindowViewModel.ActivePack.Questions.Count > 0 && !IsPlayerMode;

        public void ReverseIsPlayerMode()
        {
            IsPlayerMode = !IsPlayerMode;
            PlayQuizCommand.RaiseCanExecuteChanged();
        }
        private void SendIsPlayerModeMessage()
        {
            IsPlayerModeMessage.Invoke(this, EventArgs.Empty);
        }
        public void OnIsConfigurationModeMessageRecieved(object sender, EventArgs args)
        {
            ReverseIsPlayerMode();
            PlayQuizCommand.RaiseCanExecuteChanged();
        }

        //Don't call methods something from view, ex "button" or "label"
        private void UpdateLabel(object obj)
        {
            TimeLimit++;
            UpdateLabelCommand.RaiseCanExecuteChanged();
        }

        private bool CanUpdateLabel(object? arg) => TimeLimit < 10;

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (TimeLimit > 0)
            {
                TimeLimit--;
            }
            else
            {
                //Go to ResultView
            }

        }
    }
}

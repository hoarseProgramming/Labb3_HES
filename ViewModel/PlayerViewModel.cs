using Labb3_HES.Command;
using System.Windows.Threading;

namespace Labb3_HES.ViewModel
{
    class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

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

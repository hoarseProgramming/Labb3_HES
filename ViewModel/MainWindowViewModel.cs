using Labb3_HES.Command;
using Labb3_HES.Model;
using System.Collections.ObjectModel;

namespace Labb3_HES.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel>? Packs { get; set; }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }
        public ResultViewModel ResultViewModel { get; }

        private QuestionPackViewModel? _activePack;

        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged();
                ConfigurationViewModel.RaisePropertyChanged("ActivePack");
            }
        }

        public DelegateCommand CreateNewPackCommand { get; }
        public DelegateCommand SelectActivePackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand ExitApplicationCommand { get; }
        public DelegateCommand ToggleFullScreenCommand { get; }

        public event EventHandler ShouldCreateNewPackMessage;
        public event EventHandler DeletePackMessage;
        public event EventHandler ConstructorsAreLoadedMessage;
        public event EventHandler ShouldExitApplicationMessage;
        public event EventHandler ShouldToggleFullScreenMessage;

        public MainWindowViewModel()
        {
            CreateNewPackCommand = new DelegateCommand(CreateNewPack, CanCreateNewPack);
            DeletePackCommand = new DelegateCommand(DeletePack, CanDeletePack);
            SelectActivePackCommand = new DelegateCommand(SelectActivePack);
            ExitApplicationCommand = new DelegateCommand(ExitApplication);
            ToggleFullScreenCommand = new DelegateCommand(ToggleFullScreen);


            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);
            ResultViewModel = new ResultViewModel(this);

            Packs = JsonHandler.LoadJsonFile();

            ActivePack = Packs.FirstOrDefault();

            SendConstructorsAreLoadedMessage();


        }

        private void ToggleFullScreen(object obj)
        {
            ShouldToggleFullScreenMessage.Invoke(this, EventArgs.Empty);
        }

        private void CreateNewPack(object obj)
        {
            ShouldCreateNewPackMessage.Invoke(this, EventArgs.Empty);
        }
        private bool CanCreateNewPack(object? arg) => ConfigurationViewModel.IsConfigurationMode;
        public void AddNewPack(string name, int difficultyIndex, int timeLimitInSeconds)
        {
            var newQuestionPack = new QuestionPackViewModel(new QuestionPack(name, (Difficulty)difficultyIndex, timeLimitInSeconds));
            Packs.Add(newQuestionPack);
            ActivePack = newQuestionPack;
            DeletePackCommand.RaiseCanExecuteChanged();
        }

        private void DeletePack(object obj) => SendDeletePackMessage();
        private void SendDeletePackMessage() => DeletePackMessage.Invoke(this, EventArgs.Empty);
        private bool CanDeletePack(object? arg) => Packs.Count > 1 && ConfigurationViewModel.IsConfigurationMode;
        public void DeletePackAfterConfirmation()
        {
            var currentActivePack = ActivePack;
            SelectNewActivePackBeforeDeletingCurrentPack();

            Packs.Remove(currentActivePack);
            DeletePackCommand.RaiseCanExecuteChanged();
        }
        private void SelectNewActivePackBeforeDeletingCurrentPack()
        {
            int currentActivePackIndex = Packs.IndexOf(ActivePack);
            if (currentActivePackIndex == 0)
            {
                ActivePack = Packs[1];
            }
            else
            {
                ActivePack = Packs[currentActivePackIndex - 1];
            }
        }

        private void SelectActivePack(object obj) => ActivePack = obj as QuestionPackViewModel;

        private void ExitApplication(object obj) => ShouldExitApplicationMessage.Invoke(this, EventArgs.Empty);
        private void SendConstructorsAreLoadedMessage() => ConstructorsAreLoadedMessage.Invoke(this, EventArgs.Empty);

    }
}

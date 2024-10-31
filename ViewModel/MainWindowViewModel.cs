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
        public event EventHandler ShouldCreateNewPackMessage;
        public DelegateCommand SelectActivePackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public event EventHandler DeletePackMessage;


        public event EventHandler ConstructorsAreLoadedMessage;



        public MainWindowViewModel()
        {
            CreateNewPackCommand = new DelegateCommand(CreateNewPack, CanCreateNewPack);
            DeletePackCommand = new DelegateCommand(DeletePack, CanDeletePack);
            SelectActivePackCommand = new DelegateCommand(SelectActivePack);

            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);

            SendConstructorsAreLoadedMessage();

            Packs = JsonHandler.LoadJsonFile();

            ActivePack = Packs.FirstOrDefault();
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

        private void SendConstructorsAreLoadedMessage()
        {
            ConstructorsAreLoadedMessage.Invoke(this, EventArgs.Empty);
        }
        private void SendDeletePackMessage()
        {
            DeletePackMessage.Invoke(this, EventArgs.Empty);
        }

        private void SelectActivePack(object obj) => ActivePack = obj as QuestionPackViewModel;



        private void DeletePack(object obj)
        {
            SendDeletePackMessage();
        }
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
        private bool CanDeletePack(object? arg) => Packs.Count > 1 && ConfigurationViewModel.IsConfigurationMode;


    }
}

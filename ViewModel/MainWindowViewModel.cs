using Labb3_HES.Command;
using Labb3_HES.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Threading;

namespace Labb3_HES.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }
        public ResultViewModel ResultViewModel { get; }

        public ObservableCollection<QuestionPackViewModel>? Packs { get; set; }

        private QuestionPackViewModel? _activePack = new QuestionPackViewModel(new QuestionPack());
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

        private DispatcherTimer autoSaveTimer = new DispatcherTimer(DispatcherPriority.Background);

        private int applicationRunTimeInSeconds = 0;

        public string pathToJsonFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Labb3_HES\\hoarseQuizzerer.json";


        public DelegateCommand CreateNewPackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand SelectActivePackCommand { get; }
        public DelegateCommand ExitApplicationCommand { get; }
        public DelegateCommand ToggleFullScreenCommand { get; }

        public event EventHandler? ShouldCreateNewPackMessage;
        public event EventHandler? ShouldDeletePackMessage;
        public event EventHandler? ShouldExitApplicationMessage;
        public event EventHandler? ShouldToggleFullScreenMessage;

        public MainWindowViewModel()
        {
            Packs = new ObservableCollection<QuestionPackViewModel>();

            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);
            ResultViewModel = new ResultViewModel(this);

            CreateNewPackCommand = new DelegateCommand(CreateNewPack, CanCreateNewPack);
            DeletePackCommand = new DelegateCommand(DeletePack, CanDeletePack);
            SelectActivePackCommand = new DelegateCommand(SelectActivePack);
            ExitApplicationCommand = new DelegateCommand(ExitApplication);
            ToggleFullScreenCommand = new DelegateCommand(ToggleFullScreen);

            SubscribeToEvents();
            StartAutoSaveTimer();
        }

        private void CreateNewPack(object obj) => ShouldCreateNewPackMessage.Invoke(this, EventArgs.Empty);
        private bool CanCreateNewPack(object? arg) => ConfigurationViewModel.IsConfigurationMode;
        public void AddNewPack(string name, int difficultyIndex, int timeLimitInSeconds)
        {
            var newQuestionPack = new QuestionPackViewModel(new QuestionPack(name, (Difficulty)difficultyIndex, timeLimitInSeconds));
            Packs.Add(newQuestionPack);
            ActivePack = newQuestionPack;
            DeletePackCommand.RaiseCanExecuteChanged();
        }

        private void DeletePack(object obj) => SendDeletePackMessage();
        private bool CanDeletePack(object? arg) => Packs.Count > 1 && ConfigurationViewModel.IsConfigurationMode;
        private void SendDeletePackMessage() => ShouldDeletePackMessage.Invoke(this, EventArgs.Empty);
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
        private void ToggleFullScreen(object obj) => ShouldToggleFullScreenMessage.Invoke(this, EventArgs.Empty);
        private void SubscribeToEvents()
        {
            ConfigurationViewModel.IsConfigurationModeMessage += PlayerViewModel.OnIsOtherModeMessageRecieved;
            ConfigurationViewModel.IsConfigurationModeMessage += ResultViewModel.OnIsOtherModeMessageRecieved;

            PlayerViewModel.IsPlayerModeMessage += ResultViewModel.OnIsOtherModeMessageRecieved;
            PlayerViewModel.IsPlayerModeMessage += ConfigurationViewModel.OnIsOtherModeMessageRecieved;

            ResultViewModel.IsResultModeMessage += PlayerViewModel.OnIsOtherModeMessageRecieved;
        }
        private void StartAutoSaveTimer()
        {
            autoSaveTimer.Interval = TimeSpan.FromSeconds(1);
            autoSaveTimer.Tick += AutoSaveTimer_Tick;
            autoSaveTimer.Start();
        }
        private async void AutoSaveTimer_Tick(object? sender, EventArgs e)
        {
            applicationRunTimeInSeconds++;

            if (applicationRunTimeInSeconds % 30 == 0)
            {
                await SavePacks();
            }
        }
        public async Task LoadPacks()
        {
            Task<List<QuestionPack>> loadQuestions = JsonHandler.LoadJsonFile(pathToJsonFile);

            var packs = await loadQuestions;

            foreach (var pack in packs)
            {
                Packs.Add(new QuestionPackViewModel(pack));
            }
        }
        public async Task SavePacks()
        {
            List<QuestionPack> questionPacks = GetQuestionPackModelsFrom(Packs);

            await JsonHandler.SaveJsonFile(questionPacks, pathToJsonFile);
        }
        private List<QuestionPack> GetQuestionPackModelsFrom(ObservableCollection<QuestionPackViewModel> packs)
        {
            List<QuestionPack> questionPacks = new();

            foreach (var pack in packs)
            {
                QuestionPack currentQuestionPack = pack.GetQuestionPackModel();

                List<Question> questions = new();

                foreach (var question in pack.Questions)
                {
                    questions.Add(question);
                }

                currentQuestionPack.Questions = questions;

                questionPacks.Add(pack.GetQuestionPackModel());
            }

            return questionPacks;
        }

        internal void CreateDirectory()
        {
            string pathToDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Labb3_HES";
            Directory.CreateDirectory(pathToDirectory);
        }

        internal void CreateDefaultQuestionpack()
        {
            QuestionPackViewModel defaultQuestionpack = new(new QuestionPack("hoarse Default Question Pack!"));

            defaultQuestionpack.Questions.Add(new Question(new string[] { "ViewModel, Model, View", "View, ViewModel, Model", "ViewModel, View, Model" }, "", "What does MVVM stand for?"));
            defaultQuestionpack.Questions.Add(new Question(new string[] { "UnDRY", "Weak", "Smelly" }, "SOLID", "Fredrik the ...?"));
            defaultQuestionpack.Questions.Add(new Question(new string[]
                {
                    "This is one of the longest answers in the world. And also the wrong one (1). " +
                    "Does this fit in the player? I guess we'll have to try it. Don't we?",
                    "This is one of the longest answers in the world. And also the wrong one (2). " +
                    "Does this fit in the player? I guess we'll have to try it. Don't we?",
                    "This is one of the longest answers in the world. And also the wrong one (3). " +
                    "Does this fit in the player? I guess we'll have to try it. Don't we?"
                },
                    "This is one of the longest answers in the world. And also the correct one. " +
                    "Does this fit in the player? I guess we'll have to try it. Don't we?",

                    "This is one of the longest questions in the world, with no apparent question or use, " +
                    "really, it's more of a test to see how the configuration view handles extremely long " +
                    "questions"
                ));

            Packs.Add(defaultQuestionpack);
        }
    }
}

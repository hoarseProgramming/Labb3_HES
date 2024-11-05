using Labb3_HES.Command;
using Labb3_HES.Model;
using System.Collections.ObjectModel;
using System.IO;

namespace Labb3_HES.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel>? Packs { get; set; }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }
        public ResultViewModel ResultViewModel { get; }

        private QuestionPackViewModel? _activePack = new QuestionPackViewModel(new QuestionPack());

        public string pathToJsonFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Labb3_HES\\hoarseQuizzerer.json";

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
        public event EventHandler ShouldExitApplicationMessage;
        public event EventHandler ShouldToggleFullScreenMessage;

        public MainWindowViewModel()
        {
            Packs = new ObservableCollection<QuestionPackViewModel>();

            CreateNewPackCommand = new DelegateCommand(CreateNewPack, CanCreateNewPack);
            DeletePackCommand = new DelegateCommand(DeletePack, CanDeletePack);
            SelectActivePackCommand = new DelegateCommand(SelectActivePack);
            ExitApplicationCommand = new DelegateCommand(ExitApplication);
            ToggleFullScreenCommand = new DelegateCommand(ToggleFullScreen);


            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);
            ResultViewModel = new ResultViewModel(this);

            ConfigurationViewModel.IsConfigurationModeMessage += PlayerViewModel.OnIsOtherModeMessageRecieved;
            ConfigurationViewModel.IsConfigurationModeMessage += ResultViewModel.OnIsOtherModeMessageRecieved;

            PlayerViewModel.IsPlayerModeMessage += ResultViewModel.OnIsOtherModeMessageRecieved;
            PlayerViewModel.IsPlayerModeMessage += ConfigurationViewModel.OnIsOtherModeMessageRecieved;

            ResultViewModel.IsResultModeMessage += PlayerViewModel.OnIsOtherModeMessageRecieved;


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
            List<QuestionPack> questionPacks = new();

            foreach (var pack in Packs)
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

            await JsonHandler.SaveJsonFile(questionPacks, pathToJsonFile);
        }

        internal void CreateDirectory()
        {
            string pathToDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Labb3_HES";
            Directory.CreateDirectory(pathToDirectory);
        }

        internal void CreateDefaultQuestionpack()
        {
            QuestionPackViewModel defaultQuestionpack = new(new QuestionPack());
            //TODO: Test
            defaultQuestionpack.Questions.Add(new Question(new string[] { "UnDRY", "Weak", "Smelly" }, "Fredrik the ...?", "SOLID"));
            defaultQuestionpack.Questions.Add(new Question(new string[]
                { "This is one of the longest answers in the world. And also the wrong one (1). " +
                    "Does this fit in the player? I guess we'll have to try it. Don't we?",
                    "This is one of the longest answers in the world. And also the wrong one (2). " +
                    "Does this fit in the player? I guess we'll have to try it. Don't we?",
                    "This is one of the longest answers in the world. And also the wrong one (3). " +
                    "Does this fit in the player? I guess we'll have to try it. Don't we?"
                },
                "This is one of the longest questions in the world, with no apparent question or use, " +
                "really, it's more of a test to see how the configuration view handles extremely long " +
                "questions",
                "This is one of the longest answers in the world. And also the correct one. " +
                "Does this fit in the player? I guess we'll have to try it. Don't we?"));

            //defaultQuestionpack.Questions.Add(new Question("Fredrik the ...?", "SOLID", "Weak", "Smelly", "Null Exception"));
            Packs.Add(defaultQuestionpack);
        }
    }
}

using Labb3_HES.Command;
using Labb3_HES.Model;

namespace Labb3_HES.ViewModel
{
    class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        private bool _isConfigurationMode = true;
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

        private CategoryList _categoryList;
        public CategoryList CategoryList
        {
            get => _categoryList;
            set
            {
                this._categoryList = value;
                RaisePropertyChanged();
            }
        }

        private APIQuestionRequest _aPIQuestionRequest;
        public APIQuestionRequest APIQuestionRequest
        {
            get => _aPIQuestionRequest;
            set
            {
                _aPIQuestionRequest = value;
                RaisePropertyChanged();
            }
        }

        private string _responseMessage;
        public string ResponseMessage
        {
            get => _responseMessage;
            set
            {
                _responseMessage = value;
                RaisePropertyChanged();
            }
        }

        private Category _selectedCategoryForImporting;
        public Category SelectedCategoryForImporting
        {
            get => _selectedCategoryForImporting;
            set
            {
                _selectedCategoryForImporting = value;
                RaisePropertyChanged();
            }
        }

        public Difficulty SelectedDifficultyForImporting { get; set; }

        public int SelectedNumberOfQuestionsForImporting { get; set; }


        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public DelegateCommand OpenPackOptionsCommand { get; }
        public DelegateCommand EnableConfigurationCommand { get; }
        public DelegateCommand ShouldOpenImportQuestionsCommand { get; }
        public DelegateCommand ShouldImportQuestionsCommand { get; }


        public event EventHandler IsConfigurationModeMessage;
        public event EventHandler ShouldOpenPackOptionsMessage;
        public event EventHandler ShouldOpenImportQuestionsMessage;
        public event EventHandler ShouldImportQuestionsMessage;

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            SetDefaultPropertiesOnStartup();

            AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);
            OpenPackOptionsCommand = new DelegateCommand(OpenPackOptions, CanOpenPackOptions);
            EnableConfigurationCommand = new DelegateCommand(EnableConfiguration, CanEnableConfiguration);
            ShouldOpenImportQuestionsCommand = new DelegateCommand(ShouldOpenImportQuestions, CanOpenImportQuestions);
            ShouldImportQuestionsCommand = new DelegateCommand(ShouldImportQuestions, CanImportQuestions);

        }

        private void SetDefaultPropertiesOnStartup()
        {
            SetDefaultCategoryList();
            SetDefaultAPIQuestionRequest();

            ResponseMessage = APIQuestionRequest.ActiveResponseCodeMessage;
            SelectedDifficultyForImporting = Difficulty.Medium;
            SelectedNumberOfQuestionsForImporting = 1;
        }
        public void SetDefaultCategoryList()
        {
            var listOfCategories = new List<Category>();
            listOfCategories.Add(new Category(0, "Loading"));

            CategoryList = new CategoryList(listOfCategories);
            SelectedCategoryForImporting = CategoryList.ListOfCategories[0];

            ShouldImportQuestionsCommand?.RaiseCanExecuteChanged();
        }
        private void SetDefaultAPIQuestionRequest()
        {
            var listOfQuestions = new List<Question>();
            APIQuestionRequest = new APIQuestionRequest(6, listOfQuestions);
        }

        private void AddQuestion(object obj)
        {
            ActivePack.Questions.Add(new Question(new string[] { "Answer2", "Answer3", "Answer4" }, "Answer1"));
            mainWindowViewModel.PlayerViewModel.PlayQuizCommand.RaiseCanExecuteChanged();
        }
        private bool CanAddQuestion(object? arg) => IsConfigurationMode;

        private void RemoveQuestion(object obj)
        {
            ActivePack.Questions.Remove(ActiveQuestion);
            mainWindowViewModel.PlayerViewModel.PlayQuizCommand.RaiseCanExecuteChanged();
        }
        private bool CanRemoveQuestion(object? arg) => ActiveQuestion != null && IsConfigurationMode;

        private void OpenPackOptions(object obj) => ShouldOpenPackOptionsMessage.Invoke(this, EventArgs.Empty);
        private bool CanOpenPackOptions(object? arg) => IsConfigurationMode;

        private void EnableConfiguration(object obj)
        {
            SendIsConfigurationModeMessage();
            ToggleConfigurationModeOnOrOff();
        }
        private bool CanEnableConfiguration(object? arg) => !IsConfigurationMode;
        private void SendIsConfigurationModeMessage() => IsConfigurationModeMessage.Invoke(this, EventArgs.Empty);
        public void OnIsOtherModeMessageRecieved(object sender, EventArgs args)
        {
            if (IsConfigurationMode) ToggleConfigurationModeOnOrOff();
        }
        private void ToggleConfigurationModeOnOrOff()
        {
            IsConfigurationMode = !IsConfigurationMode;

            EnableConfigurationCommand.RaiseCanExecuteChanged();
            AddQuestionCommand.RaiseCanExecuteChanged();
            RemoveQuestionCommand.RaiseCanExecuteChanged();
            OpenPackOptionsCommand.RaiseCanExecuteChanged();
            ShouldOpenImportQuestionsCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.CreateNewPackCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.DeletePackCommand.RaiseCanExecuteChanged();
        }

        private async void ShouldOpenImportQuestions(object obj) => ShouldOpenImportQuestionsMessage.Invoke(this, EventArgs.Empty);
        private bool CanOpenImportQuestions(object? arg) => IsConfigurationMode;

        private void ShouldImportQuestions(object obj) => ShouldImportQuestionsMessage.Invoke(this, EventArgs.Empty);
        private bool CanImportQuestions(object? arg) => CategoryList.ListOfCategories.Count > 1;

        public async Task ImportQuestions()
        {
            APIQuestionRequest = await APIHandler.GetQuestions(SelectedNumberOfQuestionsForImporting, SelectedCategoryForImporting.Id, SelectedDifficultyForImporting.ToString().ToLower());
            foreach (var question in APIQuestionRequest.ImportedQuestions)
            {
                mainWindowViewModel.ActivePack.Questions.Add(question);
            }
        }
    }
}

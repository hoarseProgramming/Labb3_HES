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
        public DelegateCommand ShouldOpenImportQuestionsCommand { get; }
        public event EventHandler ShouldOpenImportQuestionsMessage;

        //private List<Category> _categoryList;

        //public List<Category> CategoryList
        //{
        //    get => _categoryList;
        //    set
        //    {
        //        this._categoryList = value;
        //        RaisePropertyChanged();
        //    }
        //}
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

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            var listOfCategories = new List<Category>();
            listOfCategories.Add(new Category(0, "Loading"));
            //CategoryList = listOfCategories;
            CategoryList = new CategoryList(listOfCategories);

            IsConfigurationMode = true;

            AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);
            OpenPackOptionsCommand = new DelegateCommand(OpenPackOptions, CanOpenPackOptions);
            EnableConfigurationCommand = new DelegateCommand(EnableConfiguration, CanEnableConfiguration);
            ShouldOpenImportQuestionsCommand = new DelegateCommand(OpenImportQuestions, CanOpenImportQuestions);


        }

        private bool CanOpenImportQuestions(object? arg) => IsConfigurationMode;

        private async void OpenImportQuestions(object obj)
        {
            ShouldOpenImportQuestionsMessage.Invoke(this, EventArgs.Empty);
            CategoryList = await APIHandler.GetQuestionCategories();
            //var categoryList = await APIHandler.GetQuestionCategories();
            //CategoryList = categoryList.trivia_categories;
        }
        public void ImportQuestions()
        {

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

        private void EnableConfiguration(object obj)
        {
            SendIsConfigurationModeMessage();
            ToggleConfigurationModeOn();
        }
        private bool CanEnableConfiguration(object? arg) => !IsConfigurationMode;
        private void SendIsConfigurationModeMessage()
        {
            IsConfigurationModeMessage.Invoke(this, EventArgs.Empty);
        }
        private void ToggleConfigurationModeOn()
        {
            IsConfigurationMode = true;
            EnableConfigurationCommand.RaiseCanExecuteChanged();
            AddQuestionCommand.RaiseCanExecuteChanged();
            RemoveQuestionCommand.RaiseCanExecuteChanged();
            OpenPackOptionsCommand.RaiseCanExecuteChanged();
            ShouldOpenImportQuestionsCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.CreateNewPackCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.DeletePackCommand.RaiseCanExecuteChanged();
        }
        private void ToggleConfigurationModeOff()
        {
            IsConfigurationMode = false;
            EnableConfigurationCommand.RaiseCanExecuteChanged();
            AddQuestionCommand.RaiseCanExecuteChanged();
            RemoveQuestionCommand.RaiseCanExecuteChanged();
            OpenPackOptionsCommand.RaiseCanExecuteChanged();
            ShouldOpenImportQuestionsCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.CreateNewPackCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.DeletePackCommand.RaiseCanExecuteChanged();
        }
        public void OnIsOtherModeMessageRecieved(object sender, EventArgs args)
        {
            ToggleConfigurationModeOff();
        }
    }
}

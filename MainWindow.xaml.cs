using Labb3_HES.Dialogs;
using Labb3_HES.ViewModel;
using System.IO;
using System.Windows;

namespace Labb3_HES
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel mainWindowViewModel = new();

        private WindowState stateBeforeFullScreenToggled;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = mainWindowViewModel;
            playerView.DataContext = mainWindowViewModel.PlayerViewModel;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            mainWindowViewModel.ShouldCreateNewPackMessage += OnShouldCreateNewPackMessageRecieved;
            mainWindowViewModel.ShouldDeletePackMessage += OnShouldDeletePackMessageRecieved;
            mainWindowViewModel.ShouldToggleFullScreenMessage += OnShouldToggleFullScreenMessageRecieved;
            mainWindowViewModel.PlayerViewModel.ActivePackIsNotPlayableMessage += OnActivePackIsNotPlayableMessageRecieved;
            mainWindowViewModel.ShouldExitApplicationMessage += OnShouldExitApplicationMessageRecieved;

            mainWindowViewModel.ConfigurationViewModel.ShouldOpenPackOptionsMessage += OnShouldOpenPackOptionsMessageRecieved;
            mainWindowViewModel.ConfigurationViewModel.ShouldOpenImportQuestionsMessage += OnShouldOpenImportQuestionsMessageRecieved;

            mainWindowViewModel.PlayerViewModel.AnswerRecievedMessage += playerView.OnAnswerRecievedMessageRecieved;
            mainWindowViewModel.PlayerViewModel.NoAnswerRecievedMessage += playerView.OnNoAnswerRecievedMessageRecieved;
            mainWindowViewModel.PlayerViewModel.IsNewQuestionMessage += playerView.OnIsNewQuestionMessageRecieved;
        }

        public void OnShouldCreateNewPackMessageRecieved(object sender, EventArgs args)
        {
            CreateNewPackDialog createNewPackDialog = new();

            var result = createNewPackDialog.ShowDialog();

            if (result == true)
            {
                string name = createNewPackDialog.NewQuestionPackName;
                int difficultyIndex = createNewPackDialog.Index;
                int timeLimitInSeconds = createNewPackDialog.TimeLimitInSeconds;
                mainWindowViewModel.AddNewPack(name, difficultyIndex, timeLimitInSeconds);
            }
        }

        public void OnShouldDeletePackMessageRecieved(object sender, EventArgs args)
        {
            var result = MessageBox.Show
                ($"Are you sure you want to delete {mainWindowViewModel.ActivePack.Name}",
                 "Delete question pack?",
                 MessageBoxButton.YesNo
                );

            if (result == MessageBoxResult.Yes) mainWindowViewModel.DeletePackAfterConfirmation();
        }

        public void OnShouldToggleFullScreenMessageRecieved(object sender, EventArgs args)
        {
            if (!(this.WindowState == WindowState.Maximized))
            {
                stateBeforeFullScreenToggled = WindowState;
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = stateBeforeFullScreenToggled;
                WindowStyle = WindowStyle.SingleBorderWindow;
                ResizeMode = ResizeMode.CanResize;
            }
        }

        private void OnActivePackIsNotPlayableMessageRecieved(object? sender, EventArgs e)
        {
            ActivePackNotPlayableDialog activePackNotPlayableDialog = new();

            activePackNotPlayableDialog.ShowDialog();
        }

        public void OnShouldExitApplicationMessageRecieved(object sender, EventArgs args) => this.Close();

        public void OnShouldOpenPackOptionsMessageRecieved(object sender, EventArgs args)
        {
            PackOptionsDialog packOptionsDialog = new();

            packOptionsDialog.ShowDialog();
        }

        public void OnShouldOpenImportQuestionsMessageRecieved(object sender, EventArgs args)
        {
            mainWindowViewModel.ConfigurationViewModel.SetDefaultCategoryList();
            ImportQuestionsDialog importQuestionsDialog = new();
            var result = importQuestionsDialog.ShowDialog();

            if (result == true)
            {
                ImportStatusDialog importStatusDialog = new();
                var importStatusDialogResult = importStatusDialog.ShowDialog();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Path.Exists(mainWindowViewModel.pathToJsonFile))
            {
                await mainWindowViewModel.LoadPacks();
            }
            else
            {
                mainWindowViewModel.CreateDirectory();
                mainWindowViewModel.CreateDefaultQuestionpack();
            }

            mainWindowViewModel.ActivePack = mainWindowViewModel.Packs.FirstOrDefault();
            mainWindowViewModel.PlayerViewModel.QuestionPackWithRandomizedOrder = mainWindowViewModel.ActivePack.GetQuestionPackWithRandomizedOrderOfQuestions();
            mainWindowViewModel.PlayerViewModel.ActiveAnswers = new string[4];
        }
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await mainWindowViewModel.SavePacks();
        }
    }
}
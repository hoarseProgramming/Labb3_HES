using Labb3_HES.Dialogs;
using Labb3_HES.ViewModel;
using System.IO;
using System.Windows;

namespace Labb3_HES
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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


            //TODO: Make Importer class for VG
        }
        private void SubscribeToEvents()
        {
            mainWindowViewModel.DeletePackMessage += OnDeletePackMessageRecieved;
            mainWindowViewModel.ConfigurationViewModel.ShouldOpenPackOptionsMessage += OnShouldOpenPackOptionsMessageRecieved;
            mainWindowViewModel.ShouldCreateNewPackMessage += OnShouldCreateNewPackMessageRecieved;
            mainWindowViewModel.ShouldToggleFullScreenMessage += OnShouldToggleFullScreenMessageRecieved;
            mainWindowViewModel.ShouldExitApplicationMessage += OnShouldExitApplicationMessageRecieved;
            mainWindowViewModel.PlayerViewModel.AnswerRecievedMessage += playerView.OnAnswerRecievedMessageRecieved;
            mainWindowViewModel.PlayerViewModel.NoAnswerRecievedMessage += playerView.OnNoAnswerRecievedMessageRecieved;
            mainWindowViewModel.PlayerViewModel.IsNewQuestionMessage += playerView.OnIsNewQuestionMessageRecieved;

        }
        public void OnDeletePackMessageRecieved(object sender, EventArgs args)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {mainWindowViewModel.ActivePack.Name}",
                                         "Delete question pack?",
                                         MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                mainWindowViewModel.DeletePackAfterConfirmation();
            }
        }
        public void OnShouldOpenPackOptionsMessageRecieved(object sender, EventArgs args)
        {
            PackOptionsDialog packOptionsDialog = new();

            packOptionsDialog.ShowDialog();
        }
        public void OnShouldCreateNewPackMessageRecieved(object sender, EventArgs args)
        {
            CreateNewPackDialog createNewPackDialog = new();

            var result = createNewPackDialog.ShowDialog();

            if (result == true)
            {
                string name = createNewPackDialog.Name;
                int difficultyIndex = createNewPackDialog.Index;
                int timeLimitInSeconds = createNewPackDialog.TimeLimitInSeconds;
                mainWindowViewModel.AddNewPack(name, difficultyIndex, timeLimitInSeconds);
            }
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
        public void OnShouldExitApplicationMessageRecieved(object sender, EventArgs args) => this.Close();
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await mainWindowViewModel.SavePacks();
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
    }
}
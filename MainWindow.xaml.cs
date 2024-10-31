using Labb3_HES.Dialogs;
using Labb3_HES.ViewModel;
using System.Windows;

namespace Labb3_HES
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel mainWindowViewModel = new();
        public MainWindow()
        {
            InitializeComponent();

            DataContext = mainWindowViewModel;
            mainWindowViewModel.DeletePackMessage += OnDeletePackMessageRecieved;
            mainWindowViewModel.ConfigurationViewModel.ShouldOpenPackOptionsMessage += OnShouldOpenPackOptionsMessageRecieved;
            mainWindowViewModel.ShouldCreateNewPackMessage += OnShouldCreateNewPackMessageRecieved;
            mainWindowViewModel.ShouldExitApplicationMessage += OnShouldExitApplicationMessageRecieved;

            //DataContext = new MainWindowViewModel();
            //(DataContext as MainWindowViewModel).DeletePackMessage += OnDeletePackMessageRecieved;
            //(DataContext as MainWindowViewModel).ConfigurationViewModel.ShouldOpenPackOptionsMessage += OnShouldOpenPackOptionsMessageRecieved;
            //(DataContext as MainWindowViewModel).ShouldCreateNewPackMessage += OnShouldCreateNewPackMessageRecieved;


            //TODO: Make Importer class for VG
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
        public void OnShouldExitApplicationMessageRecieved(object sender, EventArgs args) => this.Close();
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            JsonHandler.SaveJsonFile(mainWindowViewModel.Packs);
        }

    }
}
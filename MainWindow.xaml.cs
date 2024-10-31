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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            (DataContext as MainWindowViewModel).DeletePackMessage += OnDeletePackMessageRecieved;
            (DataContext as MainWindowViewModel).ConfigurationViewModel.ShouldOpenPackOptionsMessage += OnShouldOpenPackOptionsMessageRecieved;
            (DataContext as MainWindowViewModel).ShouldCreateNewPackMessage += OnShouldCreateNewPackMessageRecieved;


            //TODO: Make Importer class for VG
        }
        public void OnDeletePackMessageRecieved(object sender, EventArgs args)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {(DataContext as MainWindowViewModel).ActivePack.Name}",
                                         "Delete question pack?",
                                         MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                (DataContext as MainWindowViewModel).DeletePackAfterConfirmation();
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
                (DataContext as MainWindowViewModel).AddNewPack(name, difficultyIndex, timeLimitInSeconds);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            JsonHandler.SaveJsonFile((DataContext as MainWindowViewModel).Packs);
        }

    }
}
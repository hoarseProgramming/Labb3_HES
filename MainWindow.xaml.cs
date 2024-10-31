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

            //TODO: Make Importer class for VG
        }
        public void OnDeletePackMessageRecieved(object sender, EventArgs args)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {(DataContext as MainWindowViewModel).ActivePack.Name}",
                                         "Delete question pack?",
                                         MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                (DataContext as MainWindowViewModel).DeletePackTest();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            JsonHandler.SaveJsonFile((DataContext as MainWindowViewModel).Packs);
        }

        private void OpenPackOptionsCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            PackOptionsDialog packOptionsDialog = new();

            packOptionsDialog.ShowDialog();
        }
    }
}
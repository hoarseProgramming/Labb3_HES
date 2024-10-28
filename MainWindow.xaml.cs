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

            //Make Importer class for VG
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            JsonHandler.SaveJsonFile((DataContext as MainWindowViewModel).Packs);
        }
    }
}
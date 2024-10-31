using Labb3_HES.Dialogs;
using Labb3_HES.ViewModel;
using System.Windows.Controls;

namespace Labb3_HES.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void OpenPackOptionsCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            PackOptionsDialog packOptionsDialog = new();

            packOptionsDialog.ShowDialog();
        }

        private void CreateNewPackCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
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
    }
}

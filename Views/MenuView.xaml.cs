using Labb3_HES.Dialogs;
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
    }
}

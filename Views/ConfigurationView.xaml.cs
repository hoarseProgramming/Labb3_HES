using Labb3_HES.Dialogs;
using System.Windows.Controls;

namespace Labb3_HES.Views
{
    /// <summary>
    /// Interaction logic for ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : UserControl
    {
        public ConfigurationView()
        {
            InitializeComponent();
        }

        private void ButtonOpenPackOptions_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PackOptionsDialog packOptionsDialog = new();

            packOptionsDialog.ShowDialog();
        }
    }
}

using System.Windows;

namespace Labb3_HES.Dialogs
{
    public partial class PackOptionsDialog : Window
    {
        public PackOptionsDialog()
        {
            InitializeComponent();
            DataContext = (App.Current.MainWindow as MainWindow).DataContext;
        }
    }
}

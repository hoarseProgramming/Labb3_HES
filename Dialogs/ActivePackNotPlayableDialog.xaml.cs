using System.Windows;

namespace Labb3_HES.Dialogs
{
    public partial class ActivePackNotPlayableDialog : Window
    {
        public ActivePackNotPlayableDialog()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

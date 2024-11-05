using Labb3_HES.ViewModel;
using System.Windows;

namespace Labb3_HES.Dialogs
{

    public partial class ImportQuestionsDialog : Window
    {
        private ConfigurationViewModel configurationViewModel;
        public ImportQuestionsDialog()
        {
            InitializeComponent();
            DataContext = (App.Current.MainWindow as MainWindow).DataContext;
            configurationViewModel = (DataContext as MainWindowViewModel).ConfigurationViewModel;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            configurationViewModel.CategoryList = await APIHandler.GetQuestionCategories();
            comboboxCategories.SelectedIndex = 0;
        }
    }
}

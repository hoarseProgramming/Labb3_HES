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
            configurationViewModel.ShouldImportQuestionsMessage += StartQuestionImport;
        }

        private void StartQuestionImport(object? sender, EventArgs e)
        {
            DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                configurationViewModel.CategoryList = await APIHandler.GetQuestionCategories();
                configurationViewModel.SelectedCategoryForImporting = configurationViewModel.CategoryList.ListOfCategories[0];
                configurationViewModel.ShouldImportQuestionsCommand.RaiseCanExecuteChanged();
            }
            catch (Exception exception)
            {
                this.Close();
                ImportStatusDialog importStatusDialog = new();
                var result = importStatusDialog.ShowDialog();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            configurationViewModel.ShouldImportQuestionsMessage -= StartQuestionImport;
        }
    }
}

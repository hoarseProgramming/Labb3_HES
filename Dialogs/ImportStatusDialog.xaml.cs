using Labb3_HES.ViewModel;
using System.Net.Http;
using System.Windows;

namespace Labb3_HES.Dialogs
{
    /// <summary>
    /// Interaction logic for ImportStatusDialog.xaml
    /// </summary>
    public partial class ImportStatusDialog : Window
    {
        private ConfigurationViewModel configurationViewModel;
        public ImportStatusDialog()
        {
            InitializeComponent();
            DataContext = (App.Current.MainWindow as MainWindow).DataContext;
            configurationViewModel = (DataContext as MainWindowViewModel).ConfigurationViewModel;
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            configurationViewModel.ResponseMessage = configurationViewModel.APIQuestionRequest.ResponseCodeMessages[6];
            try
            {
                await configurationViewModel.ImportQuestions();
                configurationViewModel.ResponseMessage = configurationViewModel.APIQuestionRequest.ActiveResponseCodeMessage;

            }
            catch (HttpRequestException exception)
            {
                if (exception.Message == "No such host is known. (opentdb.com:443)")
                {
                    configurationViewModel.ResponseMessage = "No connection to host, check your internet connection!";
                }
                else if (exception.Message == "Response status code does not indicate success: 429 (Too Many Requests).")
                {
                    configurationViewModel.ResponseMessage = exception.Message;
                    configurationViewModel.ResponseMessage = "Too many requests, have some patience young padawan!";
                }
                else
                {
                    configurationViewModel.ResponseMessage = exception.Message;
                }

            }
            catch (Exception exception)
            {
                configurationViewModel.ResponseMessage = exception.Message;
            }
        }
    }
}

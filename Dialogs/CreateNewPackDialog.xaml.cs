using System.Windows;
using System.Windows.Controls;

namespace Labb3_HES.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateNewPackDialog.xaml
    /// </summary>
    public partial class CreateNewPackDialog : Window
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public int TimeLimitInSeconds { get; set; }
        public CreateNewPackDialog()
        {
            InitializeComponent();
            DataContext = (App.Current.MainWindow as MainWindow).DataContext;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void sliderTimeLimitInSeconds_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider? slider = sender as Slider;
            int value = (int)slider.Value;
            TimeLimitInSeconds = value;
        }

        private void comboboxDifficulty_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = sender as ComboBox;
            Index = comboBox.SelectedIndex;
        }

        private void textboxPackName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            Name = textBox.Text;
        }
    }
}

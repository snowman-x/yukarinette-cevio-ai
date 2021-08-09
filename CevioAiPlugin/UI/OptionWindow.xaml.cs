using System.Windows;

namespace CevioAiPlugin.UI
{
    /// <summary>
    /// OptionWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class OptionWindow 
    {
        public OptionWindow(bool exitWhenFinished)
        {
            InitializeComponent();
            CheckBox.IsChecked = exitWhenFinished;
        }

        public bool ExitWhenFinished => CheckBox.IsChecked ?? false;

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

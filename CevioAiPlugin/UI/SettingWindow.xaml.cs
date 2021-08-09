using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CeVIO.Talk.RemoteService2;
using CevioAiPlugin.Data;
using CevioAiPlugin.Properties;
using CevioAiPlugin.ViewModel;

namespace CevioAiPlugin.UI
{
    /// <summary>
    /// SettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow
    {
        public SettingWindow()
        {
            InitializeComponent();
            DataContext = SettingWindowViewModel.Instance;
        }

        private SettingWindowViewModel ViewModel => DataContext as SettingWindowViewModel;
        private Talker2 Talker { get; set; }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (Talker == null)
            {
                Talker = new Talker2();
            }

            Talker.Cast = ViewModel.Cast;
            ViewModel.Emotions.Clear();

            foreach (var component in Talker.Components)
            {
                var emotion = new Emotion
                {
                    Id = component.Id,
                    Name = component.Name,
                    Value = component.Value,
                };

                ViewModel.Emotions.Add(emotion);
            }
        }

        private void SettingWindow_OnClosing(object sender, CancelEventArgs e)
        {
        }
    }
}

using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CevioAiPlugin.Core;
using CevioAiPlugin.Data;

namespace CevioAiPlugin.ViewModel
{
    public class SettingWindowViewModel : Element
    {
        private static SettingWindowViewModel _instance;
        public static SettingWindowViewModel Instance => _instance ?? (_instance = new SettingWindowViewModel());

        public SettingWindowViewModel()
        {
        }

        public IEnumerable<string> Casts { get; set; } = Enumerable.Empty<string>();
        public string Cast { get; set; } = string.Empty;
        public uint Volume { get; set; }
        public uint Speed { get; set; }
        public uint Tone { get; set; }
        public uint Alpha { get; set; }
        public uint ToneScale { get; set; }

        public ObservableCollection<Emotion> Emotions { get; } = new ObservableCollection<Emotion>();
    }
}
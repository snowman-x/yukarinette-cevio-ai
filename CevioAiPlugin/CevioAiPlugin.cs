using System;
using System.Linq;
using System.Threading;
using System.Windows;
using Yukarinette;
using CeVIO.Talk.RemoteService2;
using CevioAiPlugin.Properties;
using CevioAiPlugin.UI;
using CevioAiPlugin.ViewModel;

namespace CevioAiPlugin
{
    public class CevioAiPlugin : IYukarinetteInterface
    {
        public override string Name => "CeVIO AI 連携プラグイン";
        public override string GUID => "5F1562B8-C0CF-43AA-AB83-AA19F9656B27";

        private Talker2 Talker { get; set; }
        private SettingWindow SettingWindow { get; set; }
        private Window Window { get; } = new Window();

        private void Logging(string text)
        {
            YukarinetteLogger.Instance?.Debug(text);
        }

        public override void Loaded()
        {
            Logging("Loaded Start.");

            var setting = SettingWindowViewModel.Instance;
            setting.Cast = Settings.Default.Cast;
            setting.Volume = Settings.Default.Volume;
            setting.Speed = Settings.Default.Speed;
            setting.Tone = Settings.Default.Tone;
            setting.Alpha = Settings.Default.Alpha;
            setting.ToneScale = Settings.Default.ToneScale;

            Logging("Loaded End.");
        }

        public override void Closed()
        {
            Logging("Closed Start.");

            var setting = SettingWindowViewModel.Instance;
            Settings.Default.Cast = setting.Cast;
            Settings.Default.Volume = setting.Volume;
            Settings.Default.Speed = setting.Speed;
            Settings.Default.Tone = setting.Tone;
            Settings.Default.Alpha = setting.Alpha;
            Settings.Default.ToneScale = setting.ToneScale;
            Settings.Default.Save();

            Logging("Closed End.");
        }

        public override void Setting()
        {
            Logging("Setting Start.");

            var window = new OptionWindow(Settings.Default.ExitWhenFinished);
            if (window.ShowDialog() ?? false)
            {
                Settings.Default.ExitWhenFinished = window.ExitWhenFinished;
            }

            Logging("Setting End.");
        }

        public override void SpeechRecognitionStart()
        {
            Logging("SpeechRecognitionStart Start.");

            var succeeded = false;
            foreach (var i in Enumerable.Range(0, 3))
            {
                var result = ServiceControl2.StartHost(false);

                if (result == HostStartResult.Succeeded ||
                    result == HostStartResult.AlreadyStarted)
                {
                    succeeded = true;
                    break;
                }

                Thread.Sleep((int) (1000 * Math.Pow(2, i)));
            }

            if (!succeeded)
            {
                MessageBox.Show("CeVIO AI の起動ができませんでした");
                Logging("SpeechRecognitionStart End.");
            }

            Talker = new Talker2();

            var setting = SettingWindowViewModel.Instance;
            setting.Casts = TalkerAgent2.AvailableCasts ?? Enumerable.Empty<string>();

            Window.Dispatcher.Invoke(() =>
            {
                SettingWindow = new SettingWindow();
                SettingWindow.Show();
            });

            Logging("SpeechRecognitionStart End.");
        }

        public override void SpeechRecognitionStop()
        {
            Logging("SpeechRecognitionStop Start.");

            Window.Dispatcher.Invoke(() =>
            {
                SettingWindow?.Close();

                if (Settings.Default.ExitWhenFinished)
                {
                    ServiceControl2.CloseHost();
                }
            });

            Logging("SpeechRecognitionStop End.");
        }

        public override void Speech(string text)
        {
            Logging("Speech Start.");

            var setting = SettingWindowViewModel.Instance;

            if (!string.IsNullOrWhiteSpace(setting.Cast))
            {
                Talker.Cast = setting.Cast;
                Talker.Volume = setting.Volume;
                Talker.Speed = setting.Speed;
                Talker.Tone = setting.Tone;
                Talker.Alpha = setting.Alpha;
                Talker.ToneScale = setting.ToneScale;

                var emotions = setting.Emotions.ToList();
                foreach (var component in Talker.Components)
                {
                    var emotion = emotions.FirstOrDefault(e => e.Id == component.Id);
                    if (emotion == null)
                    {
                        continue;
                    }

                    component.Value = emotion.Value;
                }

                text = text.Replace(" ", "").Replace("　", "");

                var state = Talker.Speak(text);
                state.Wait();
            }

            Logging("Speech End.");
        }
    }
}
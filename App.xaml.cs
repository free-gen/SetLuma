using System;
using System.Windows;
using System.Windows.Input;
using NHotkey;
using NHotkey.Wpf;

namespace SetLuma
{
    public partial class App : Application
    {
        private HotkeyConfig _config;
        private HotkeyPopup _hotkeyPopup;
        private int _brightness;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _config = HotkeyConfig.Load("hotkey.ini");

            _hotkeyPopup = new HotkeyPopup();
            _brightness = BrightnessControl.GetBrightness();

            RegisterHotkeys();
        }

        private void RegisterHotkeys()
        {
            try
            {
                var increaseKey = new KeyGestureConverter().ConvertFromString(_config.HotkeyUp) as KeyGesture;
                var decreaseKey = new KeyGestureConverter().ConvertFromString(_config.HotkeyDown) as KeyGesture;

                if (increaseKey != null)
                {
                    HotkeyManager.Current.AddOrReplace("IncreaseBrightness", increaseKey.Key, increaseKey.Modifiers, (s, e) => IncreaseBrightness());
                }

                if (decreaseKey != null)
                {
                    HotkeyManager.Current.AddOrReplace("DecreaseBrightness", decreaseKey.Key, decreaseKey.Modifiers, (s, e) => DecreaseBrightness());
                }
            }
            catch (Exception)
            {
            }
        }

        private void IncreaseBrightness()
        {
            _brightness = Math.Min(100, _brightness + 10);
            UpdateBrightness();
        }

        private void DecreaseBrightness()
        {
            _brightness = Math.Max(0, _brightness - 10);
            UpdateBrightness();
        }

        private void UpdateBrightness()
        {
            BrightnessControl.SetBrightness(_brightness);
            _hotkeyPopup.ShowBrightness(_brightness);
        }
    }
}

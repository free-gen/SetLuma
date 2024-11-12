using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;

namespace SetLuma
{
    public partial class HotkeyPopup : Window
    {
        private DispatcherTimer _hideTimer;

        public HotkeyPopup()
        {
            InitializeComponent();

            SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;

            UpdateWindowPositionAndSize();

            _hideTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.5)
            };

            _hideTimer.Tick += (s, e) => Hide();

            UpdateColors();

            SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
        }

        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            UpdateWindowPositionAndSize();
						
						this.UseLayoutRounding = true;
        }

        private void UpdateWindowPositionAndSize()
        {
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            var screenWidth = SystemParameters.PrimaryScreenWidth;

            var dpi = 96.0;
            var source = PresentationSource.FromVisual(this);
            if (source != null)
            {
                dpi = source.CompositionTarget.TransformToDevice.M11 * 96;
            }

            this.Width = 192 * dpi / 96;
            this.Height = 68 * dpi / 96;

            this.Left = (screenWidth - this.Width) / 2;
            this.Top = screenHeight - this.Height - 50;
        }

        private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General || e.Category == UserPreferenceCategory.Color)
            {
                UpdateColors();
            }
        }

        private void UpdateColors()
        {
            bool isDarkTheme = ThemeHelper.IsDarkThemeEnabled();

            if (isDarkTheme)
            {
                Border.Background = new SolidColorBrush(Color.FromRgb(45, 45, 45));
                Border.BorderBrush = new SolidColorBrush(Color.FromArgb(120, 20, 20, 20));
                IconPath.Fill = new SolidColorBrush(Color.FromRgb(245, 245, 245));
                ProgressBar.Background = new SolidColorBrush(Color.FromRgb(160, 160, 160));
            }
            else
            {
                Border.Background = new SolidColorBrush(Color.FromRgb(250, 250, 250));
                Border.BorderBrush = new SolidColorBrush(Color.FromArgb(80, 200, 200, 200));
                IconPath.Fill = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                ProgressBar.Background = new SolidColorBrush(Color.FromRgb(140, 140, 140));
            }

            var accentColor = ThemeHelper.GetAccentColor();
            ProgressBar.Foreground = new SolidColorBrush(accentColor);
        }

        public void ShowBrightness(int brightness)
        {
            ProgressBar.Value = brightness;

            _hideTimer.Stop();
            Show();
            _hideTimer.Start();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;
            SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;
        }
    }
}

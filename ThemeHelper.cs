using Microsoft.Win32;
using System;
using System.Windows.Media;

namespace SetLuma
{
    public static class ThemeHelper
    {
        public static bool IsDarkThemeEnabled()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        var themeValue = key.GetValue("AppsUseLightTheme");
                        if (themeValue is int useLightTheme)
                        {
                            return useLightTheme == 0;
                        }
                    }
                }
            }
            catch
            {
            }

            return false;
        }

        public static Color GetAccentColor()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM"))
                {
                    if (key != null)
                    {
                        var accentColorValue = key.GetValue("AccentColor");

                        if (accentColorValue != null && accentColorValue is int colorValue)
                        {
                            byte a = (byte)((colorValue >> 24) & 0xFF);
                            byte b = (byte)((colorValue >> 16) & 0xFF);
                            byte g = (byte)((colorValue >> 8) & 0xFF);
                            byte r = (byte)(colorValue & 0xFF);

                            bool isDarkTheme = IsDarkThemeEnabled();

                            if (!isDarkTheme)
                            {
                                r = (byte)Math.Max(0, r - 20);
                                g = (byte)Math.Max(0, g - 20);
                                b = (byte)Math.Max(0, b - 20);
                            }
                            else
                            {
                                r = (byte)Math.Min(255, r + 70);
                                g = (byte)Math.Min(255, g + 70);
                                b = (byte)Math.Min(255, b + 70);
                            }

                            return Color.FromArgb(a, r, g, b);
                        }
                    }
                }
            }
            catch
            {
            }

            return Colors.Blue;
        }
    }
}

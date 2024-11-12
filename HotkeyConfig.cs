using System;
using System.IO;

namespace SetLuma
{
    public class HotkeyConfig
    {
        public string HotkeyUp { get; private set; }
        public string HotkeyDown { get; private set; }

        public static HotkeyConfig Load(string filePath)
        {
            var config = new HotkeyConfig();

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[Hotkeys]\nHotkeyUp = Ctrl+F2\nHotkeyDown = Ctrl+F1");
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (line.StartsWith("HotkeyUp"))
                    config.HotkeyUp = line.Split('=')[1].Trim();
                else if (line.StartsWith("HotkeyDown"))
                    config.HotkeyDown = line.Split('=')[1].Trim();
            }

            return config;
        }
    }
}

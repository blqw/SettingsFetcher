using blqw;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Settings.DateTime);
            Console.WriteLine(Settings.Url);
            Console.WriteLine(Settings.MaxLength);
            Console.WriteLine(Settings.MinLength);
            Console.WriteLine(Settings.Delay);
            Console.WriteLine(Settings.Enable);
        }
    }

    [SettingsGroupName("tests")]
    static class Settings
    {
        static Settings() => SettingsFetcher.Fill(typeof(Settings), false);

        public static DateTime DateTime { get; private set; } = new DateTime(1970, 1, 1);
        public static Uri Url { get; private set; }
        public static float MaxLength { get; private set; }
        public static float MinLength { get; private set; }
        public static int Delay { get; private set; }
        public static bool Enable { get; private set; }
    }
}

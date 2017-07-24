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
            var settings = new Settings();
            Console.WriteLine(settings.DateTime);
            Console.WriteLine(settings.Url);
        }
    }
    
    class Settings
    {
        public Settings() => SettingsBus.Fill(this);
        public DateTime DateTime { get; private set; }
        public Uri Url { get; private set; }
    }
}

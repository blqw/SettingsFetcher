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
            var a = new Settings();
            Console.WriteLine(a.DateTime);
        }
    }

    class Settings
    {
        public Settings()
        {
            
            SettingsBus.Fill(new SettingsBusArgs
            {
                Getter = name => ConfigurationManager.AppSettings[name]
            }, null, this);

        }
        public DateTime DateTime { get; set; }
    }
}

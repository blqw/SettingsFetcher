using System;
using blqw;

namespace Tests2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    class Settings
    {
        public Settings() => SettingsFetcher.Fill(o =>
        {
            o.JoinName = (pre, name) => pre == null ? name : $"{pre}@{name}";
        }, "Demo", this);

        public DateTime DateTime { get; private set; }
        public Uri Url { get; private set; }
    }
}
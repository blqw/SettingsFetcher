using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public sealed class SettingsBusArgs: ISettingsBusArgs
    {
        public Func<string, object> Getter { get; set; }

        public Func<object, Type, object> Converter { get; set; }

        public Func<string, string, string> JoinName { get; set; }
    }
}

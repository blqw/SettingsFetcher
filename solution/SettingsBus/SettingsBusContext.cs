using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace blqw
{
    /// <summary>
    /// 设置总线上下文
    /// </summary>
    public class SettingsBusContext : ISettingsBusContext
    {
        public static readonly SettingsBusContext Default = new SettingsBusContext(null, null, null);
        public Func<string, object> Getter { get; }

        public Func<object, Type, object> Converter { get; }

        public Func<string, string, string> JoinName { get; }

        public SettingsBusContext(Func<string, object> getter, Func<object, Type, object> converter, Func<string, string, string> joinName)
        {
            Getter = getter;
            Converter = converter;
            JoinName = joinName;
        }

        private string JoinNameImpl(string prefix, string name)
            => string.IsNullOrWhiteSpace(prefix) ? name : $"{prefix}.{name}";

        private object ConvertImpl(object value, Type conversionType)
        {
            if (value is string s)
            {
                if (conversionType == typeof(Guid))
                {
                    if (Guid.TryParse(s.Trim(), out var guid))
                    {
                        return guid;
                    }
                }
                else if (conversionType == typeof(TimeSpan))
                {
                    if (TimeSpan.TryParse(s.Trim(), out var time))
                    {
                        return time;
                    }
                }
                else if (conversionType == typeof(Uri))
                {
                    var s1 = s.Trim();
                    if (s1.Length > 2 && s1[0] == '/' && s1[1] == '/')
                    {
                        s1 = "http:" + s1;
                    }
                    if (Uri.TryCreate(s1, UriKind.RelativeOrAbsolute, out var uri))
                    {
                        return uri;
                    }
                }
            }
            return Convert.ChangeType(value, conversionType);
        }

        private object GetterImpl(string name)
#if NET45
            => System.Configuration.ConfigurationManager.AppSettings[name];
#else
            => throw new NotImplementedException();
#endif


        public object GetSetting(string group, string name, Type conversionType)
        {
            name = (JoinName ?? JoinNameImpl)(group, name);
            var value = (Getter ?? GetterImpl)(name);
            if (value != null)
            {
                value = (Converter ?? ConvertImpl)(value, conversionType);
            }
            return value;
        }
    }
}

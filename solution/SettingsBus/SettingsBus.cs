using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Reflection;
using System.Text;

namespace blqw
{
    public static class SettingsBus
    {
        public static Func<string, object> Getter { get; set; }

        public static Func<object, Type, object> Converter { get; set; }

        public static Func<string, string, string> JoinName { get; set; }

        private static SettingsBusContext _context = SettingsBusContext.Default;

        public static void Fill(string prefix, object instanceOrType)
            => Fill((ISettingsBusContext)null, prefix, instanceOrType);

        public static void Fill<T>(string prefix)
         => Fill((ISettingsBusContext)null, prefix, typeof(T));

        public static void Fill(ISettingsBusArgs args, string prefix, object instanceOrType)
            => Fill(new SettingsBusContext(args?.Getter, args?.Converter, args?.JoinName), prefix, instanceOrType);

        public static void Fill(ISettingsBusContext context, string prefix, object instance)
        {
            if (instance == null)
            {
                return;
            }

            if (context == null)
            {
                var cont = _context;
                if (cont == null || cont.Getter != Getter || cont.Converter != Converter || cont.JoinName != JoinName)
                {
                    _context = cont = new SettingsBusContext(Getter, Converter, JoinName);
                }
                context = cont;
            }
            TypeInfo type;
            if (instance is TypeInfo info)
            {
                type = info;
                instance = null;
            }
            else if (instance is Type t)
            {
                type = t.GetTypeInfo();
                instance = null;
            }
            else
            {
                type = instance.GetType().GetTypeInfo();
            }

            var isStatic = instance == null;
            foreach (var prop in type.DeclaredProperties)
            {
                if (prop.CanWrite && prop.SetMethod.IsStatic == isStatic)
                {
                    var value = context.GetSetting(prefix, prop.Name, prop.PropertyType);
                    if (value != null)
                    {
                        prop.SetValue(instance, value);
                    }
                }
            }
        }

#if NET45
        public static void Fill(ISettingsBusArgs args, object instanceOrType)
         => Fill(new SettingsBusContext(args?.Getter, args?.Converter, args?.JoinName), instanceOrType);

        public static void Fill(ISettingsBusContext context, object instanceOrType)
        {
            if (instanceOrType is TypeInfo info)
            {
                Fill(context, info.GetCustomAttribute<System.Configuration.SettingsGroupNameAttribute>()?.GroupName, info);
            }
            else if (instanceOrType is Type type)
            {
                Fill(context, type.GetCustomAttribute<System.Configuration.SettingsGroupNameAttribute>()?.GroupName, type.GetTypeInfo());
            }
            else if (instanceOrType != null)
            {
                Fill(context, instanceOrType.GetType().GetTypeInfo().GetCustomAttribute<System.Configuration.SettingsGroupNameAttribute>()?.GroupName, instanceOrType);
            }
        }

        public static void Fill(object instanceOrType)
         => Fill((ISettingsBusContext)null, instanceOrType);

        public static void Fill<T>()
         => Fill((ISettingsBusContext)null, typeof(T));
#endif
    }
}

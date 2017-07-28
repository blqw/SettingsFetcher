using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Reflection;
using System.Text;

namespace blqw
{
    /// <summary>
    /// 设置提取器
    /// </summary>
    public static class SettingsFetcher
    {
        /// <summary>
        /// 设置或获取 全局用于获取设置值的委托
        /// </summary>
        public static Func<string, object> Getter { get; set; }
        /// <summary>
        /// 设置或获取 全局用于类型转换的方法委托
        /// </summary>
        public static Func<object, Type, object> Converter { get; set; }
        /// <summary>
        /// 设置或获取 全局用于连接group和name的方法委托
        /// </summary>
        public static Func<string, string, string> JoinName { get; set; }

        /// <summary>
        /// 设置或获取 全局默认是否抛出异常的声明, 默认为true
        /// </summary>
        public static bool ThrowError { get; set; } = true;

        /// <summary>
        /// 方法提供程序
        /// </summary>
        private static SettingsFetcherMethod _method = new SettingsFetcherMethod(null, null, null, true);

        private static SettingsFetcherMethod _noThrowMethod = new SettingsFetcherMethod(null, null, null, false);

        /// <summary>
        /// 将设置值填充到指定对象的属性中
        /// </summary>
        /// <param name="groupName">设置组的名称, 没有可以为null</param>
        /// <param name="instanceOrType">实体对象(设置实例属性)或类型对象(静态属性)</param>
        /// <param name="throwError">是否在转换失败时抛出异常, 默认为 <seealso cref="ThrowError"/></param>
        public static void Fill(string groupName, object instanceOrType, bool? throwError = null)
            => FillImpl(throwError ?? ThrowError ? null : _noThrowMethod, groupName, instanceOrType);

        /// <summary>
        /// 将设置值填充到指定类型的静态属性中
        /// </summary>
        /// <typeparam name="T">需要设置静态属性的类型</typeparam>
        /// <param name="groupName">设置组的名称, 没有可以为null</param>
        /// <param name="throwError">是否在转换失败时抛出异常, 默认为 <seealso cref="ThrowError"/></param>
        public static void Fill<T>(string groupName, bool? throwError = null)
         => FillImpl(throwError ?? ThrowError ? null : _noThrowMethod, groupName, typeof(T));

        /// <summary>
        /// 将设置值填充到指定对象的属性中
        /// </summary>
        /// <param name="args">自定义参数</param>
        /// <param name="groupName">设置组的名称, 没有可以为null</param>
        /// <param name="instanceOrType">实体对象(设置实例属性)或类型对象(静态属性)</param>
        public static void Fill(ISettingsFetcherArgs args, string groupName, object instanceOrType)
            => FillImpl(new SettingsFetcherMethod(args), groupName, instanceOrType);

        private static void FillImpl(SettingsFetcherMethod method, string groupName, object instance)
        {
            if (instance == null)
            {
                return;
            }

            if (method == null)
            {
                var mhd = _method;
                if (mhd == null || mhd.Getter != Getter || mhd.Converter != Converter || mhd.JoinName != JoinName || mhd.ThrowError != ThrowError)
                {
                    _method = mhd = new SettingsFetcherMethod(Getter, Converter, JoinName, ThrowError);
                }
                method = mhd;
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
                    var value = method.GetSetting(groupName, prop.Name, prop.PropertyType);
                    if (value != null)
                    {
                        prop.SetValue(instance, value);
                    }
                }
            }
        }

#if NET45
        /// <summary>
        /// 将设置值填充到指定对象的属性中
        /// </summary>
        /// <param name="args">自定义参数</param>
        /// <param name="instanceOrType">实体对象(设置实例属性)或类型对象(静态属性)</param>
        public static void Fill(ISettingsFetcherArgs args, object instanceOrType)
         => FillImpl(new SettingsFetcherMethod(args), instanceOrType);

        private static void FillImpl(SettingsFetcherMethod method, object instanceOrType)
        {
            if (instanceOrType is TypeInfo info)
            {
                FillImpl(method, info.GetCustomAttribute<System.Configuration.SettingsGroupNameAttribute>()?.GroupName, info);
            }
            else if (instanceOrType is Type type)
            {
                FillImpl(method, type.GetCustomAttribute<System.Configuration.SettingsGroupNameAttribute>()?.GroupName, type.GetTypeInfo());
            }
            else if (instanceOrType != null)
            {
                FillImpl(method, instanceOrType.GetType().GetTypeInfo().GetCustomAttribute<System.Configuration.SettingsGroupNameAttribute>()?.GroupName, instanceOrType);
            }
        }

        /// <summary>
        /// 将设置值填充到指定对象的属性中
        /// </summary>
        /// <param name="instanceOrType">实体对象(设置实例属性)或类型对象(静态属性)</param>
        /// <param name="throwError">是否在转换失败时抛出异常, 默认为 <seealso cref="ThrowError"/></param>
        public static void Fill(object instanceOrType, bool? throwError = null)
         => FillImpl(throwError ?? ThrowError ? null : _noThrowMethod, instanceOrType);

        /// <summary>
        /// 将设置值填充到指定类型的静态属性中
        /// </summary>
        /// <typeparam name="T">需要设置静态属性的类型</typeparam>
        /// <param name="throwError">是否在转换失败时抛出异常, 默认为 <seealso cref="ThrowError"/></param>
        public static void Fill<T>(bool? throwError = null)
         => FillImpl(throwError ?? ThrowError ? null : _noThrowMethod, typeof(T));
#endif
    }
}

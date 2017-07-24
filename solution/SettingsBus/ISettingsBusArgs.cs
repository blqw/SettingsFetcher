using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw
{
    /// <summary>
    /// 设置总线参数
    /// </summary>
    public interface ISettingsBusArgs
    {
        /// <summary>
        /// 获取设置值
        /// </summary>
        Func<string, object> Getter { get; }

        /// <summary>
        /// 类型转换的方法
        /// </summary>
        /// <param name="value">设置值</param>
        /// <param name="conversionType">要返回的对象的类型</param>
        /// <returns></returns>
        Func<object, Type, object> Converter { get; }

        /// <summary>
        /// 用于连接group和name的方法
        /// </summary>
        Func<string, string, string> JoinName { get; }

    }
}

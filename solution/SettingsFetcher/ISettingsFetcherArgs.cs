using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw
{
    /// <summary>
    /// 设置提取器参数接口
    /// </summary>
    public interface ISettingsFetcherArgs
    {
        /// <summary>
        /// 用于获取设置值的委托
        /// </summary>
        Func<string, object> Getter { get; }

        /// <summary>
        /// 用于类型转换的方法委托
        /// </summary>
        Func<object, Type, object> Converter { get; }

        /// <summary>
        /// 用于连接group和name的方法委托
        /// </summary>
        Func<string, string, string> JoinName { get; }

        /// <summary>
        /// 是否抛出异常
        /// </summary>
        bool ThrowException { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary>
    /// 设置提取器参数
    /// </summary>
    public sealed class SettingsFetcherArgs: ISettingsFetcherArgs
    {
        /// <summary>
        /// 用于获取设置值的委托
        /// </summary>
        public Func<string, object> Getter { get; set; }
        /// <summary>
        /// 用于类型转换的方法委托
        /// </summary>
        public Func<object, Type, object> Converter { get; set; }
        /// <summary>
        /// 用于连接group和name的方法委托
        /// </summary>
        public Func<string, string, string> JoinName { get; set; }
    }
}

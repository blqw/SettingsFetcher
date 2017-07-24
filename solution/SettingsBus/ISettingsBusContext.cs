using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw
{
    /// <summary>
    /// 设置总线上下文
    /// </summary>
    public interface ISettingsBusContext
    {
        /// <summary>
        /// 根据名称获取设置的值
        /// </summary>
        /// <param name="group">设置分组</param>
        /// <param name="name">设置名称</param>
        /// <param name="conversionType">要返回的对象的类型</param>
        /// <returns></returns>
        object GetSetting(string group, string name, Type conversionType);
    }
}

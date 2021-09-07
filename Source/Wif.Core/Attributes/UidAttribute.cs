/**************************************************************************
*      File Name：UidAttribute.cs
*    Description：UidAttribute.cs class description...
*      Copyright：Copyright © 2021 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2021/4/13
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;

namespace Frontier.Wif.Core.Attributes
{
    /// <summary>
    /// 表示唯一标识的特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class UidAttribute : Attribute
    {
        /// <summary>
        /// 无参构造函数。
        /// </summary>
        public UidAttribute()
        {
        }

        /// <summary>
        /// 有参构造函数。
        /// </summary>
        /// <param name="uid">字符串表现形式的唯一标识。</param>
        public UidAttribute(string uid)
        {
            Uid = uid;
        }

        /// <summary>
        /// 获取字符串形式的唯一标识。
        /// </summary>
        public string Uid { get; }
    }
}
/**************************************************************************
*      File Name：EnumBindingSourceExtension.cs
*    Description：EnumBindingSourceExtension.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Markup;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// 枚举值绑定源的标记扩展。<remarks>参考 https://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/</remarks>
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        /// <summary>
        /// 定义枚举类型字段。
        /// </summary>
        private Type _enumType;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBindingSourceExtension" /> class.
        /// </summary>
        public EnumBindingSourceExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBindingSourceExtension" /> class.
        /// </summary>
        /// <param name="enumType">枚举类型。</param>
        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBindingSourceExtension" /> class.
        /// </summary>
        /// <param name="enumType">枚举类型。</param>
        /// <param name="isSorted">枚举成员是否排序。</param>
        public EnumBindingSourceExtension(Type enumType, bool isSorted)
        {
            EnumType = enumType;
            IsSorted = isSorted;
        }

        /// <summary>
        /// 获取或设置枚举类型。
        /// </summary>
        public Type EnumType
        {
            get { return _enumType; }
            set
            {
                if (value != EnumType)
                {
                    if (!Equals(value, null) && !(Nullable.GetUnderlyingType(value) ?? value).IsEnum)
                    {
                        throw new ArgumentException("Type must be an Enum.");
                    }

                    _enumType = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置是否排序，开启后获取EnumOrderAttribute值，若Attribute未设置，按照成员定义顺序排序。
        /// </summary>
        public bool IsSorted { get; set; }

        /// <summary>
        /// 返回用作此标记扩展的目标属性值的对象。
        /// </summary>
        /// <param name="serviceProvider">为标记扩展提供服务的对象。</param>
        /// <returns>枚举的所有值。</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Equals(EnumType, null))
            {
                throw new InvalidOperationException("The type of the Enum is undefined.");
            }

            Type underlyingEnumType = Nullable.GetUnderlyingType(EnumType) ?? EnumType;

            Array enumValues;
            if (IsSorted)
            {
                enumValues = underlyingEnumType.GetFields().Where(field => field.IsStatic).Select(field => new
                {
                    field,
                    attribute = field.GetCustomAttribute<EnumOrderAttribute>()
                }).Select(fieldInfo => new
                {
                    value = fieldInfo.field.GetValue(fieldInfo),
                    order = fieldInfo.attribute?.Order ?? 0
                }).OrderBy(field => field.order).Select(field => field.value).ToArray();
            }
            else
            {
                enumValues = Enum.GetValues(underlyingEnumType);
            }

            if (underlyingEnumType == EnumType)
            {
                return enumValues;
            }

            var nullableEnumValues = Array.CreateInstance(underlyingEnumType, enumValues.Length);
            enumValues.CopyTo(nullableEnumValues, 1);
            return nullableEnumValues;
        }
    }

    /// <summary>
    /// 表示在EnumBindingExtension中定义枚举成员顺序的特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class EnumOrderAttribute : Attribute
    {
        /// <summary>
        /// 有参构造函数。
        /// </summary>
        /// <param name="order">枚举成员序号。</param>
        public EnumOrderAttribute([CallerLineNumber] int order = 0)
        {
            Order = order;
        }

        /// <summary>
        /// 获取枚举成员序号。
        /// </summary>
        public int Order { get; }
    }
}
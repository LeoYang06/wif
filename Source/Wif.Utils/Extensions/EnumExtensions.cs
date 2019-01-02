using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Frontier.Wif.Utils.Extensions
{
    /// <summary>
    /// Defines the <see cref="EnumExtensions" />
    /// </summary>
    public static class EnumExtensions
    {
        #region Methods

        /// <summary>
        /// 获取枚举子项的数量。
        /// </summary>
        /// <typeparam name="T">枚举类型。</typeparam>
        /// <returns>枚举子项数量。</returns>
        public static int GetCount<T>()
        {
            return Enum.GetValues(typeof(T)).Length;
        }

        /// <summary>
        /// 获取枚举类子项描述信息。
        /// </summary>
        /// <param name="enumSubitem">枚举类子项。</param>
        /// <returns>The <see cref="string"/></returns>
        public static string GetDescription(this Enum enumSubitem)
        {
            try
            {
                var strValue = enumSubitem.ToString();
                var fieldInfo = enumSubitem.GetType().GetField(strValue);
                var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>(false);
                if (attribute != null)
                {
                    var da = attribute;
                    return da.Description;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("分解对象值出错", ex);
            }

            return "未知属性名称";
        }

        /// <summary>
        /// The GetIndex
        /// </summary>
        /// <param name="value">The <see cref="Enum" /></param>
        /// <returns>The <see cref="int" /></returns>
        public static int GetIndex(this Enum value)
        {
            var values = Enum.GetValues(value.GetType());
            return Array.IndexOf(values, value);
        }

        /// <summary>
        /// 检索指定枚举中常数值的集合。
        /// </summary>
        /// <typeparam name="T">枚举类型。</typeparam>
        /// <returns>枚举常数值的集合。</returns>
        public static IList<string> GetNames<T>()
        {
            return Enum.GetNames(typeof(T)).ToList();
        }

        /// <summary>
        /// 检索指定枚举中常数值的集合。
        /// </summary>
        /// <typeparam name="T">枚举类型。</typeparam>
        /// <returns>枚举常数值的集合。</returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Converts an integer value into an <see cref="Enum" />.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum" /> type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="T"/></returns>
        public static T ToEnum<T>(this int value)
                where T : struct
        {
            return InternalToEnum<T>(value);
        }

        /// <summary>
        /// Converts an integer value into an enum.
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="T?"/></returns>
        public static T? ToEnum<T>(this int? value)
                where T : struct
        {
            return InternalToNullableEnum<T>(value);
        }

        /// <summary>
        /// Converts a long value into an <see cref="Enum" />.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum" /> type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="T"/></returns>
        public static T ToEnum<T>(this long value)
                where T : struct
        {
            return InternalToEnum<T>(value);
        }

        /// <summary>
        /// Converts a short value into an <see cref="Enum" />.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum" /> type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="T"/></returns>
        public static T ToEnum<T>(this short value)
                where T : struct
        {
            return InternalToEnum<T>(value);
        }

        /// <summary>
        /// Converts a short value into an enum.
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="T?"/></returns>
        public static T? ToEnum<T>(this short? value)
                where T : struct
        {
            return InternalToNullableEnum<T>(value);
        }

        /// <summary>
        /// 将枚举常数的名称的字符串表示转换成等效的枚举对象。
        /// </summary>
        /// <typeparam name="T">枚举类型。</typeparam>
        /// <param name="text">要转换的枚举名称的字符串表示形式。</param>
        /// <returns>类型的对象。</returns>
        public static T ToEnum<T>(this string text) where T : struct, IConvertible
        {
            if (!Enum.TryParse(text, true, out T result)) return default;

            if (!Enum.IsDefined(typeof(T), result)) return default;

            return result;
        }

        /// <summary>
        /// Converts an unsigned integer value into an <see cref="Enum" />.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum" /> type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="T"/></returns>
        public static T ToEnum<T>(this uint value)
                where T : struct
        {
            return InternalToEnum<T>(value);
        }

        /// <summary>
        /// Converts an unsigned integer value into an enum.
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="T?"/></returns>
        public static T? ToEnum<T>(this uint? value)
                where T : struct
        {
            return InternalToNullableEnum<T>(value);
        }

        /// <summary>
        /// Converts an unsigned long value into an <see cref="Enum" />.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum" /> type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="T"/></returns>
        public static T ToEnum<T>(this ulong value)
                where T : struct
        {
            return InternalToEnum<T>(value);
        }

        /// <summary>
        /// Converts an unsigned short value into an <see cref="Enum" />.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum" /> type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="T"/></returns>
        public static T ToEnum<T>(this ushort value)
                where T : struct
        {
            return InternalToEnum<T>(value);
        }

        /// <summary>
        /// Converts an unsigned short value into an enum.
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="T?"/></returns>
        public static T? ToEnum<T>(this ushort? value)
                where T : struct
        {
            return InternalToNullableEnum<T>(value);
        }

        /// <summary>
        /// 将枚举转换成等效的整数值。
        /// </summary>
        /// <param name="value">枚举。</param>
        /// <returns>枚举常数的数字值。</returns>
        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// The InternalToEnum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The <see cref="object" /></param>
        /// <returns>The <see cref="T" /></returns>
        private static T InternalToEnum<T>(object value)
                where T : struct
        {
            Contract.Requires(value != null);
            var enumType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            if (!enumType.GetTypeInfo().IsEnum)
                throw new InvalidOperationException(enumType.Name + " is not an System.Enum.");

            try
            {
                // ReSharper disable once PossibleNullReferenceException
                return (T) Enum.ToObject(enumType, value);
            }
            catch (ArgumentException)
            {
                return default;
            }
        }

        /// <summary>
        /// The InternalToNullableEnum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The <see cref="object" /></param>
        /// <returns>The <see cref="T?" /></returns>
        private static T? InternalToNullableEnum<T>(object value)
                where T : struct
        {
            if (value == null)
                return null;

            return InternalToEnum<T>(value);
        }

        /// <summary>
        /// The ToInt64
        /// </summary>
        /// <param name="value">The <see cref="object" /></param>
        /// <returns>The <see cref="long" /></returns>
        private static long ToInt64(object value)
        {
            return Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
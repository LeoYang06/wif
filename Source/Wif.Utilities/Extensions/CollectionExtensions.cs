using System;
using System.Collections.Generic;
using System.Linq;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// 集合相关扩展方法。
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 将数组分割为指定大小数组。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">数组。</param>
        /// <param name="size">分割后每个数组元素数量。</param>
        /// <returns>分割后的二维数组 <see cref="IEnumerable{IEnumerable{T}}" />。</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
        {
            var partCount = (int)Math.Ceiling(array.Length / (decimal)size);
            for (var i = 0; i < partCount; i++)
                yield return array.Skip(i * size).Take(size);
        }

        /// <summary>
        /// 将数组分割为指定大小数组。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">可枚举的。</param>
        /// <param name="size">分割后每个数组元素数量。</param>
        /// <returns>分割后的二维数组 <see cref="IEnumerable{IEnumerable{T}}" />。</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> enumerable, int size)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();
            return Split(array, size);
        }
    }
}
/**************************************************************************
*      File Name：CloneExtensions.cs
*    Description：CloneExtensions.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// 对象深拷贝扩展方法（可参考第三方库DeepCloner或CloneExtensions）。
    /// </summary>
    public static class CloneExtensions
    {
        #region Methods

        /// <summary>
        /// 对象深拷贝。
        /// </summary>
        /// <typeparam name="T">类型参数。</typeparam>
        /// <param name="sourceObj">The <see cref="T" />待拷贝的对象。</param>
        /// <returns>The <see cref="T" />返回深拷贝的对象。</returns>
        public static T DeepClone<T>(this T sourceObj) where T : class
        {
            return TransExp<T, T>.Trans(sourceObj);
        }

        #endregion
    }

    /// <summary>
    /// 基于表达式树快速深拷贝对象泛型类。
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public static class TransExp<TIn, TOut>
    {
        #region Fields

        /// <summary>
        /// Defines the cache
        /// </summary>
        private static readonly Func<TIn, TOut> Cache = GetFunc();

        #endregion

        #region Methods

        /// <summary>
        /// The Trans
        /// </summary>
        /// <param name="tIn">The <see cref="TIn" /></param>
        /// <returns>The <see cref="TOut" /></returns>
        public static TOut Trans(TIn tIn)
        {
            return Cache(tIn);
        }

        /// <summary>
        /// The GetFunc
        /// </summary>
        /// <returns>The <see cref="Func{TIn, TOut}" /></returns>
        private static Func<TIn, TOut> GetFunc()
        {
            var parameterExpression = Expression.Parameter(typeof(TIn), "p");
            var memberBindingList = new List<MemberBinding>();

            foreach (var item in typeof(TOut).GetProperties())
            {
                if (!item.CanWrite)
                    continue;

                var property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }

            var memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            var lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, parameterExpression);

            return lambda.Compile();
        }

        #endregion
    }
}
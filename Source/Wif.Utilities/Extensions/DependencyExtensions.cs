/**************************************************************************
*      File Name：DependencyExtensions.cs
*    Description：DependencyExtensions.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Reflection;
using System.Windows;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// 依赖对象和依赖属性的扩展方法类。
    /// </summary>
    public static class DependencyExtensions
    {
        /// <summary>
        /// Gets the dependency property according to its name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static DependencyProperty GetDependencyProperty(this Type type, string propertyName)
        {
            DependencyProperty prop = null;
            if (type != null)
            {
                var fieldInfo = type.GetField(propertyName + "Property", BindingFlags.Static | BindingFlags.Public);
                if (fieldInfo != null)
                    prop = fieldInfo.GetValue(null) as DependencyProperty;
            }

            return prop;
        }

        /// <summary>
        /// Retrieves a <see cref="DependencyProperty" /> using reflection.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static DependencyProperty GetDependencyProperty(this DependencyObject o, string propertyName)
        {
            DependencyProperty prop = null;
            if (o != null)
                prop = GetDependencyProperty(o.GetType(), propertyName);
            return prop;
        }

        /// <summary>
        /// Sets the value of the <paramref name="property" /> only if it hasn't been explicitely set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The object.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool SetIfDefault<T>(this DependencyObject o, DependencyProperty property, T value)
        {
            if (o == null)
                throw new ArgumentNullException(nameof(o), "DependencyObject cannot be null");

            if (property == null)
                throw new ArgumentNullException(nameof(property), "DependencyProperty cannot be null");

            if (!property.PropertyType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException($"Expected {property.Name} to be of type {typeof(T).Name} but was {property.PropertyType}");

            if (DependencyPropertyHelper.GetValueSource(o, property).BaseValueSource == BaseValueSource.Default)
            {
                o.SetValue(property, value);
                return true;
            }

            return false;
        }
    }
}
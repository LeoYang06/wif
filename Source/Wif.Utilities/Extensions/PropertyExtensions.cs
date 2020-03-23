using System;
using System.ComponentModel;
using System.Reflection;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// 获取属性或枚举描述的扩展方法。
    /// </summary>
    public static class PropertyExtensions
    {
        #region Methods

        /// <summary>
        /// 获取属性的类别名称。
        /// </summary>
        /// <param name="propertyInfo">属性</param>
        /// <returns>属性类别名称</returns>
        public static string GetCategory(this PropertyInfo propertyInfo)
        {
            try
            {
                var attribute = propertyInfo.GetCustomAttribute<CategoryAttribute>(false);
                if (attribute != null)
                {
                    var da = attribute;
                    return da.Category;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("分解对象值出错", ex);
            }

            return "未知属性名称";
        }

        /// <summary>
        /// 获取属性的描述。
        /// </summary>
        /// <param name="propertyInfo">属性</param>
        /// <returns>属性描述。</returns>
        public static string GetDescription(this PropertyInfo propertyInfo)
        {
            try
            {
                var attribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>(false);
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
        /// 获取属性的描述。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="t">对象。</param>
        /// <param name="propertyName">属性名称。</param>
        /// <returns>属性描述。</returns>
        public static string GetDescription<T>(this T t, string propertyName)
        {
            try
            {
                var propertyInfo = typeof(T).GetProperty(propertyName);
                GetDescription(propertyInfo);
            }
            catch (Exception ex)
            {
                throw new Exception("分解对象值出错", ex);
            }

            return "未知属性名称";
        }

        #endregion
    }
}
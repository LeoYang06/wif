/**************************************************************************
*      File Name：ConverterFactoryExtension.cs
*    Description：ConverterFactoryExtension.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Reflection;
using System.Windows.Markup;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// 转换工厂，通过标记扩展完成对特定转换器的创建及引用。
    ///     <remarks>没有提供对在其它程序集中定义的转换器类型的支持。</remarks>
    /// </summary>
    public class ConverterFactoryExtension : MarkupExtension
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ConverterName
        /// 转换器名称。
        /// </summary>
        public Type ConverterName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 返回一个对象，此对象是作为此标记扩展的目标属性的值提供的。
        /// </summary>
        /// <param name="serviceProvider">服务提供者。</param>
        /// <returns>要在应用扩展的属性上设置的对象值。</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetType(ConverterName.FullName);
            if (type != null)
            {
                var defCons = GetDefaultConstructor(type);
                if (defCons != null)
                    return defCons.Invoke(new object[] { });
            }

            return null;
        }

        /// <summary>
        /// 获取默认的构造函数。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ConstructorInfo GetDefaultConstructor(Type type)
        {
            var infos = type.GetConstructors();
            foreach (var info in infos)
            {
                var paramInfos = info.GetParameters();
                if (paramInfos.Length == 0)
                    return info;
            }

            return null;
        }

        #endregion
    }
}
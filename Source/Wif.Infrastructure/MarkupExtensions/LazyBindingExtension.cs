/**************************************************************************
*      File Name：LazyBindingExtension.cs
*    Description：LazyBindingExtension.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// 延迟绑定扩展。
    /// </summary>
    [MarkupExtensionReturnType(typeof(object))]
    public class LazyBindingExtension : MarkupExtension
    {
        #region Fields

        /// <summary>
        /// 绑定的依赖属性。
        /// </summary>
        private DependencyProperty _property;

        /// <summary>
        /// 绑定的目标元素。
        /// </summary>
        private FrameworkElement _target;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Converter
        /// 获取或设置转换器。
        /// </summary>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Gets or sets the ConverterParameter
        /// 获取或设置转换器参数。
        /// </summary>
        public object ConverterParameter { get; set; }

        /// <summary>
        /// Gets or sets the ElementName
        /// 获取或设置元素名称。
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// Gets or sets the Path
        /// 获取或设置属性路径。
        /// </summary>
        public PropertyPath Path { get; set; }

        /// <summary>
        /// Gets or sets the RelativeSource
        /// 获取或设置绑定源的相对位置。
        /// </summary>
        public RelativeSource RelativeSource { get; set; }

        /// <summary>
        /// Gets or sets the Source
        /// 获取或设置用作绑定源的对象。
        /// </summary>
        public object Source { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyBindingExtension"/> class.
        /// </summary>
        public LazyBindingExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyBindingExtension"/> class.
        /// </summary>
        /// <param name="path">路径标记字符串。</param>
        public LazyBindingExtension(string path)
        {
            Path = new PropertyPath(path);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 返回一个对象，此对象是作为此标记扩展的目标属性的值提供的。
        /// </summary>
        /// <param name="serviceProvider">服务提供者。</param>
        /// <returns>要在应用扩展的属性上设置的对象值。</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService
                    (typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (service == null)
                return null;

            _target = service.TargetObject as FrameworkElement;
            _property = service.TargetProperty as DependencyProperty;
            if (_target != null && _property != null)
            {
                // 侦听IsVisible属性的更改，以在界面元素显示时通过OnIsVisibleChanged函数添加绑定。
                _target.IsVisibleChanged += OnIsVisibleChanged;
                return null;
            }

            var binding = CreateBinding();
            return binding.ProvideValue(serviceProvider);
        }

        /// <summary>
        /// 创建绑定类型实例。
        /// </summary>
        /// <returns></returns>
        private System.Windows.Data.Binding CreateBinding()
        {
            var binding = new System.Windows.Data.Binding(Path.Path);
            if (Source != null)
                binding.Source = Source;
            if (RelativeSource != null)
                binding.RelativeSource = RelativeSource;
            if (ElementName != null)
                binding.ElementName = ElementName;
            binding.Converter = Converter;
            binding.ConverterParameter = ConverterParameter;
            return binding;
        }

        /// <summary>
        /// 显隐状态改变事件处理函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // 添加绑定。
            var binding = CreateBinding();
            BindingOperations.SetBinding(_target, _property, binding);
        }

        #endregion
    }
}
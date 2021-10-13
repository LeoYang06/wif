/**************************************************************************
*      File Name：DesignerPropertiesHelper.cs
*    Description：DesignerPropertiesHelper.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System.ComponentModel;
using System.Windows;

namespace Frontier.Wif.Utilities.Helpers
{
    /// <summary>
    /// 设计器帮助类。
    /// </summary>
    public static class DesignerPropertiesHelper
    {
        #region Fields

        /// <summary>
        /// 是否为设计模式。
        /// </summary>
        private static bool? _isInDesignMode;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether IsInDesignMode
        /// 是否为设计模式。
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                    _isInDesignMode = (bool) DesignerProperties.IsInDesignModeProperty
                            .GetMetadata(typeof(DependencyObject)).DefaultValue;
                return _isInDesignMode.Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取指定元素是否为设计模式。
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool GetIsInDesignTool(DependencyObject element)
        {
            return DesignerProperties.GetIsInDesignMode(element);
        }

        #endregion
    }
}
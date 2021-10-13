/**************************************************************************
*      File Name：PropertyChangedSingletonBase.cs
*    Description：PropertyChangedSingletonBase.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frontier.Wif.Core.Generic;

namespace Frontier.Wif.Core.ComponentModel
{
    /// <summary>
    /// 属性改变通知泛型单例基类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyChangedSingletonBase<T> : SingletonBase<T>, INotifyPropertyChanged where T : class
    {
        #region Events

        /// <summary>
        /// Defines the PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Raises the changed event, notifying that all properties have changed, it is a short form of RaisePropertyChanged(null).
        /// </summary>
        protected void RaiseAllPropertyChanged()
        {
            RaisePropertyChanged(null);
        }

        /// <summary>
        /// 触发属性值改变通知事件
        /// </summary>
        /// <typeparam name="TProperty">属性值类型</typeparam>
        /// <param name="storage">属性值</param>
        /// <param name="newValue">属性的新值</param>
        /// <param name="propertyName">属性名称</param>
        protected void RaiseAndSetIfChanged<TProperty>(ref TProperty storage, TProperty newValue,
                [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, newValue))
                return;

            storage = newValue;
            RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// 触发属性值改变通知事件
        /// </summary>
        /// <param name="propertyName">The propertyName<see cref="string"/></param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
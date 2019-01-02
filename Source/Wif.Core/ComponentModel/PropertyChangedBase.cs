using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frontier.Wif.Core.ComponentModel
{
    /// <summary>
    /// 属性改变通知基类。
    /// </summary>
    [Serializable]
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// 属性值改变通知事件。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// 触发属性值改变通知事件，通知所有属性已更改，它是RaisePropertyChanged(null)的简短形式。
        /// </summary>
        protected void RaiseAllPropertyChanged()
        {
            RaisePropertyChanged(null);
        }

        /// <summary>
        /// 触发属性值改变通知事件
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="backingField">属性值</param>
        /// <param name="newValue">属性的新值</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>The <see cref="bool"/></returns>
        protected bool RaiseAndSetIfChanged<T>(ref T backingField, T newValue = default,
                [CallerMemberName] string propertyName = null)
        {
            if (Equals(backingField, newValue))
                return false;

            backingField = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 触发属性值改变通知事件
        /// </summary>
        /// <param name="propertyName">The propertyName<see cref="string"/></param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 触发属性值改变通知事件
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="backingField">属性值</param>
        /// <param name="newValue">属性的新值</param>
        /// <param name="propertyName">属性名称</param>
        protected void RaisePropertyChanged<T>(ref T backingField, T newValue = default,
                [CallerMemberName] string propertyName = null)
        {
            if (!Equals(backingField, newValue))
                backingField = newValue;
            RaisePropertyChanged(propertyName);
        }

        #endregion
    }
}
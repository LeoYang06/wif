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
        /// 触发属性值改变通知事件
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="field">存储属性值的字段</param>
        /// <param name="newValue">属性的新值</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>是否触发了属性改变通知</returns>
        protected bool RaiseAndSetIfChanged<T>(ref T field, T newValue = default,
                [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, newValue))
                return false;
            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 触发属性值改变通知事件
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frontier.Wif.Core.Setting
{
    /// <summary>
    /// 应用程序的基本设置类。
    /// </summary>
    public abstract class AppSettingsBase : MarshalByRefObject, INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Defines the _dictionary
        /// </summary>
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        #endregion

        #region Events

        /// <summary>
        /// Defines the PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// 获取设置项的值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Default">The <see cref="Func{T}" /></param>
        /// <param name="propertyName">The <see cref="string" /></param>
        /// <returns>The <see cref="T" /></returns>
        protected T Get<T>(Func<T> Default, [CallerMemberName] string propertyName = "")
        {
            lock (_dictionary)
            {
                if (_dictionary.TryGetValue(propertyName, out var obj) && obj is T val)
                    return val;
                var defaultValue = Default.Invoke();
                if (defaultValue != null) _dictionary.Add(propertyName, defaultValue);
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取设置项的值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Default"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected T Get<T>(T Default = default, [CallerMemberName] string propertyName = "")
        {
            lock (_dictionary)
            {
                if (_dictionary.TryGetValue(propertyName, out var obj) && obj is T val)
                    return val;
                if (Default != null)
                    _dictionary.Add(propertyName, Default);
                return Default;
            }
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
        /// 设置设置项的值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void Set<T>(T value, [CallerMemberName] string propertyName = "")
        {
            lock (_dictionary)
            {
                if (_dictionary.ContainsKey(propertyName))
                    _dictionary[propertyName] = value;
                else _dictionary.Add(propertyName, value);
            }

            RaisePropertyChanged(propertyName);
        }

        #endregion
    }
}
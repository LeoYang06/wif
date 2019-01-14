using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frontier.Wif.Core.Generic;

namespace Wif.Demo.Common
{
    /// <summary>
    /// Defines the <see cref="MobilePhone" />
    /// </summary>
    public sealed class MobilePhoneSingletonModel : SingletonBase<MobilePhoneSingletonModel>, INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Defines the _brand
        /// </summary>
        private Brand _brand;

        /// <summary>
        /// Defines the _NumberOfCPUCore
        /// </summary>
        private NumberOfCPUCore _NumberOfCPUCore;

        /// <summary>
        /// Defines the _RAM
        /// </summary>
        private RAM _RAM;

        /// <summary>
        /// Defines the _ROM
        /// </summary>
        private ROM _ROM;

        /// <summary>
        /// Defines the _ScreenResolution
        /// </summary>
        private ScreenResolution _ScreenResolution;

        /// <summary>
        /// Defines the _screenSize
        /// </summary>
        private float _screenSize;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Brand
        /// </summary>
        public Brand Brand
        {
            get => _brand;
            set => RaiseAndSetIfChanged(ref _brand, value);
        }

        /// <summary>
        /// Gets or sets the NumberOfCPUCore
        /// </summary>
        public NumberOfCPUCore NumberOfCPUCore
        {
            get => _NumberOfCPUCore;
            set => RaiseAndSetIfChanged(ref _NumberOfCPUCore, value);
        }

        /// <summary>
        /// Gets or sets the RAM
        /// </summary>
        public RAM RAM
        {
            get => _RAM;
            set => RaiseAndSetIfChanged(ref _RAM, value);
        }

        /// <summary>
        /// Gets or sets the ROM
        /// </summary>
        public ROM ROM
        {
            get => _ROM;
            set => RaiseAndSetIfChanged(ref _ROM, value);
        }

        /// <summary>
        /// Gets or sets the ScreenResolution
        /// </summary>
        public ScreenResolution ScreenResolution
        {
            get => _ScreenResolution;
            set => RaiseAndSetIfChanged(ref _ScreenResolution, value);
        }

        /// <summary>
        /// Gets or sets the ScreenSize
        /// </summary>
        public float ScreenSize
        {
            get => _screenSize;
            set => RaiseAndSetIfChanged(ref _screenSize, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="MobilePhoneSingletonModel"/> class from being created.
        /// </summary>
        private MobilePhoneSingletonModel()
        {
        }

        #endregion

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
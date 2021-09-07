/**************************************************************************
*      File Name：GlobalClock.cs
*    Description：GlobalClock.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Timers;
using Frontier.Wif.Core.ComponentModel;

namespace Frontier.Wif.Infrastructure.Common
{
    /// <summary>
    /// 全局的时钟。
    /// </summary>
    public class GlobalClock : PropertyChangedSingletonBase<GlobalClock>, IDisposable
    {
        #region Fields

        /// <summary>
        ///     时钟更新器。
        /// </summary>
        private readonly Timer _timeUpdater = new Timer();

        /// <summary>
        ///     获取或设置当前时间。
        /// </summary>
        private DateTime _currentDateTime;

        #endregion

        #region Constructors

        /// <summary>
        ///     有构造函数。
        /// </summary>
        private GlobalClock()
        {
            // 初始化时钟更新器。
            _timeUpdater.Interval = 1000;
            _timeUpdater.Elapsed += _timeUpdater_Elapsed;
            _timeUpdater.Start();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     获取或设置当前时间。
        /// </summary>
        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set => RaiseAndSetIfChanged(ref _currentDateTime, value);
        }

        /// <summary>
        ///     获取本机时间长字符串表现形式。
        /// </summary>
        public string LongLocalDateTimeString => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        /// <summary>
        ///     获取UTC时间长字符串表现形式。
        /// </summary>
        public string LongUtcDateTimeString => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");

        #endregion

        #region Methods

        /// <summary>
        ///     释放资源。
        /// </summary>
        public void Dispose()
        {
            _timeUpdater.Elapsed -= _timeUpdater_Elapsed;
            _timeUpdater.Stop();
            _timeUpdater.Close();
            _timeUpdater.Dispose();
        }

        /// <summary>
        ///     时钟更新方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timeUpdater_Elapsed(object sender, EventArgs e)
        {
            CurrentDateTime = DateTime.Now;
        }

        #endregion
    }
}
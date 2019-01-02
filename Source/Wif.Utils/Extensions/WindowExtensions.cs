using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace Frontier.Wif.Utils.Extensions
{
    /// <summary>
    /// Defines the <see cref="WindowExtensions" />
    /// </summary>
    public static class WindowExtensions
    {
        #region Methods

        /// <summary>
        /// 在指定显示器上打开窗口。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="screenIndex"></param>
        /// <param name="isSingleInstance"></param>
        /// <returns>The <see cref="T"/></returns>
        public static T OpenWindow<T>(int screenIndex, bool isSingleInstance = false) where T : Window
        {
            T window;
            if (isSingleInstance)
                window = Application.Current.Windows.OfType<T>().FirstOrDefault(x => x.GetType() == typeof(T))
                        ?? Activator.CreateInstance<T>();
            else
                window = Activator.CreateInstance<T>();
            window.OpenWindow(screenIndex);
            return window;
        }

        /// <summary>
        /// 在指定显示器上打开窗口。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="screenIndex"></param>
        /// <param name="isSingleInstance"></param>
        /// <param name="args"></param>
        /// <returns>The <see cref="T"/></returns>
        public static T OpenWindow<T>(int screenIndex, bool isSingleInstance = false, params object[] args)
                where T : Window
        {
            T window;
            if (isSingleInstance)
                window = Application.Current.Windows.OfType<T>().FirstOrDefault(x => x.GetType() == typeof(T))
                        ?? (T) Activator.CreateInstance(typeof(T), args);
            else
                window = (T) Activator.CreateInstance(typeof(T), args);
            window.OpenWindow(screenIndex);
            return window;
        }

        /// <summary>
        /// 在指定显示器上打开窗口。
        /// </summary>
        /// <param name="windowType"></param>
        /// <param name="screenIndex"></param>
        /// <returns>The <see cref="Window"/></returns>
        public static Window OpenWindow(this Type windowType, int screenIndex)
        {
            var window = Activator.CreateInstance(windowType) as Window;
            window.OpenWindow(screenIndex);
            return window;
        }

        /// <summary>
        /// 在指定显示器上打开窗口。
        /// </summary>
        /// <param name="windowType"></param>
        /// <param name="screenIndex"></param>
        /// <param name="args"></param>
        /// <returns>The <see cref="Window"/></returns>
        public static Window OpenWindow(this Type windowType, int screenIndex, params object[] args)
        {
            var window = Activator.CreateInstance(windowType, args) as Window;
            window.OpenWindow(screenIndex);
            return window;
        }

        /// <summary>
        /// 在指定显示器上打开窗口。
        /// </summary>
        /// <param name="window"></param>
        /// <param name="screenIndex"></param>
        public static void OpenWindow(this Window window, int screenIndex)
        {
            var index = screenIndex.Clip(Math.Min(screenIndex, Screen.AllScreens.Length - 1),
                    Screen.AllScreens.Length - 1);
            if (index == window.GetWindowOfScreenIndex() && window.IsLoaded)
                return;
            window.WindowState = WindowState.Minimized;
            var workingArea = Screen.AllScreens[index].WorkingArea;
            window.Width = workingArea.Width;
            window.Height = workingArea.Height;
            window.Left = workingArea.Location.X;
            window.Top = workingArea.Location.Y;
            window.Show();
            window.Activate();
            window.WindowState = WindowState.Maximized;
        }

        #endregion
    }
}
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wif.Demo.IocExtensions;

namespace Wif.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Private Fields

        /// <summary>
        /// 定义程序当前的UI调度器。
        /// </summary>
        public Dispatcher CurrentUIDispatcher { get; private set; }

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="App" /> class.
        /// </summary>
        public App()
        {
            InitHostBuilder();
            CurrentUIDispatcher = Current.Dispatcher;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        /// 定义通用主机字段。
        /// </summary>
        private IHost _host;

        /// <summary>
        /// 获取或设置依赖注入的容器。
        /// </summary>
        public IServiceProvider Container { get; private set; }

        #endregion Fields

        #region Methods

        /// <summary>
        /// UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleProcessCorruptedStateExceptions]
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show($"UI线程全局异常\r\n{e.Exception}", "警告");
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("应用程序发生不可恢复的异常，将要退出！", "警告");
            }
        }

        /// <summary>
        /// The App_OnExit
        /// </summary>
        /// <param name="sender">The sender <see cref="object" /></param>
        /// <param name="e">The e <see cref="ExitEventArgs" /></param>
        private async void App_OnExit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(1));
            }
        }

        /// <summary>
        /// The App_OnStartup
        /// </summary>
        /// <param name="sender">The sender <see cref="object" /></param>
        /// <param name="e">The e <see cref="StartupEventArgs" /></param>
        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            // 注册异常事件。
            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        /// <summary>
        /// 非UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleProcessCorruptedStateExceptions]
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.ExceptionObject is not Exception ex)
                    return;
                CurrentUIDispatcher?.InvokeAsync(() => { MessageBox.Show($"非UI线程全局异常\r\n{ex.Message}"); });
            }
            catch (Exception ex)
            {
                CurrentUIDispatcher?.InvokeAsync(() => { MessageBox.Show("应用程序发生不可恢复的异常，将要退出！"); });
            }
        }

        /// <summary>
        /// 创建和配置构建器对象。
        /// </summary>
        private void InitHostBuilder()
        {
            // 初始化依赖注入
            _host = new HostBuilder()
                .ConfigureView()
                .UseEnvironment(Environments.Development)
                .Build();

            Container = _host.Services;
            Locator.SetLocator(Container);
        }

        #endregion Methods
    }
}

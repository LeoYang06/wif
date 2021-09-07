using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wif.Demo.Examples.BindingDemo;
using Wif.Demo.Examples.FileHelperDemo;

namespace Wif.Demo.IocExtensions
{
    public static class HostBuilderViewExtensions
    {
        /// <summary>
        /// 配置视图依赖注入。
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureView(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                #region 注入View

                serviceCollection.AddTransient<MainWindow>();
                serviceCollection.AddTransient<BindingDemoView>();
                serviceCollection.AddTransient<FileHelperDemoView>();

                #endregion 注入View

                #region 注入ViewModel

                serviceCollection.AddTransient<MainViewModel>();
                serviceCollection.AddTransient<BindingDemoViewModel>();
                serviceCollection.AddTransient<BindingDemoViewModel>();

                #endregion 注入ViewModel
            });
            return hostBuilder;
        }
    }
}
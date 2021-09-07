using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Frontier.Wif.Core.ComponentModel;
using Wif.Demo.Common;

namespace Wif.Demo
{
    /// <summary>
    /// Defines the <see cref="MainViewModel" />
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private UserControl _demoView;
        /// <summary>
        /// 获取或设置Demo视图。
        /// </summary>
        public UserControl DemoView
        {
            get => _demoView;
            set => this.RaiseAndSetIfChanged(ref _demoView, value);
        }
    }
}

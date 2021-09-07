using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Frontier.Wif.Core.Collections;
using Frontier.Wif.Core.ComponentModel;
using Frontier.Wif.Infrastructure.Commands;
using Frontier.Wif.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Wif.Demo.Common;
using Wif.Demo.Examples.BindingDemo;
using Wif.Demo.Examples.FileHelperDemo;

namespace Wif.Demo
{
    /// <summary>
    /// Defines the <see cref="MainViewModel" />
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;

        private UserControl _currentDemoView;
        /// <summary>
        /// 获取或设置Demo视图。
        /// </summary>
        public UserControl CurrentDemoView
        {
            get => _currentDemoView;
            set => this.RaiseAndSetIfChanged(ref _currentDemoView, value);
        }

        private string _selectedDemoViewCategory;
        /// <summary>
        /// 获取或设置当前选中的Demo视图类别。
        /// </summary>
        public string SelectedDemoViewCategory
        {
            get { return _selectedDemoViewCategory; }
            set { this.RaiseAndSetIfChanged(ref _selectedDemoViewCategory, value); }
        }

        private ObservableCollection<string> _demoViewCategoriesCollection;
        /// <summary>
        /// 获取或设置Demo视图类别集合。
        /// </summary>
        public ObservableCollection<string> DemoViewCategoriesCollection
        {
            get => _demoViewCategoriesCollection;
            set => this.RaiseAndSetIfChanged(ref _demoViewCategoriesCollection, value);
        }

        public DelegateCommand<RoutedEventArgs> DemoViewCategoriesSelectionChangedCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            DemoViewCategoriesSelectionChangedCommand = new DelegateCommand<RoutedEventArgs>(ExecuteDemoViewCategoriesSelectionChangedCommand);
        }

        private void ExecuteDemoViewCategoriesSelectionChangedCommand(RoutedEventArgs args)
        {
            CurrentDemoView = SelectedDemoViewCategory.ToEnum<DemoViewCategories>() switch
            {
                DemoViewCategories.BindingDemoView    => _serviceProvider.GetRequiredService<BindingDemoView>(),
                DemoViewCategories.FileHelperDemoView => _serviceProvider.GetRequiredService<FileHelperDemoView>(),
                _                                     => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

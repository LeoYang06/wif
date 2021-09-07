/**************************************************************************
*      File Name：AssemblyInfo.cs
*    Description：AssemblyInfo.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

//将 ComVisible 设置为 false 将使此程序集中的类型
//对 COM 组件不可见。  如果需要从 COM 访问此程序集中的类型，
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

[assembly: SuppressMessage("Microsoft.Performance", "CA1824:MarkAssembliesWithNeutralResourcesLanguage")]
[assembly: System.Resources.NeutralResourcesLanguage("en", System.Resources.UltimateResourceFallbackLocation.MainAssembly)]

[assembly: XmlnsPrefix("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "wif")]
[assembly: XmlnsDefinition("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "Frontier.Wif.Infrastructure")]
[assembly: XmlnsDefinition("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "Frontier.Wif.Infrastructure.AttachedBehaviors")]
[assembly: XmlnsDefinition("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "Frontier.Wif.Infrastructure.Behaviors")]
[assembly: XmlnsDefinition("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "Frontier.Wif.Infrastructure.Binding")]
[assembly: XmlnsDefinition("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "Frontier.Wif.Infrastructure.Commands")]
[assembly: XmlnsDefinition("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "Frontier.Wif.Infrastructure.Common")]
[assembly: XmlnsDefinition("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "Frontier.Wif.Infrastructure.Converters")]
[assembly: XmlnsDefinition("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "Frontier.Wif.Infrastructure.EventToCommand")]
[assembly: XmlnsDefinition("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "Frontier.Wif.Infrastructure.MarkupExtensions")]
[assembly: XmlnsDefinition("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "Frontier.Wif.Infrastructure.PanelExtensions")]
[assembly: XmlnsDefinition("https://frontier-dn.github.io/winfx/xaml/presentation/wif", "Frontier.Wif.Infrastructure.ValidationRules")]

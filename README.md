##### [English](https://github.com/Leolion610/wif/blob/master/README.EN.md)

![Logo](https://github.com/Leolion610/wif/blob/master/Resources/Images/wif_logo.png?raw=true)

### WPF Infrastructure Framework（WIF）    　    　            　 　  

wif 一个基础设施框架，可帮助您快速开发WPF应用程序。

支持 .NET Framework (4.7.2+) 和 .NET Core (3.1 and 5.0)

[![Nuget](https://img.shields.io/nuget/dt/Wif.Infrastructure.svg)](https://www.nuget.org/packages/Wif.Infrastructure/) [![nuget-version](https://img.shields.io/nuget/v/Wif.Infrastructure.svg)](https://www.nuget.org/packages/Wif.Infrastructure) ![dotnet-version](https://img.shields.io/badge/.NET%20Framework-%3E%3D4.7.2-blue.svg) ![dotnetcore version](https://img.shields.io/badge/.NET%20Core-%3E%3D3.1-blue.svg) ![csharp-version](https://img.shields.io/badge/C%23-8.0-blue.svg) ![IDE-version](https://img.shields.io/badge/IDE-vs2019-blue.svg) [![博客园](https://img.shields.io/badge/%E5%8D%9A%E5%AE%A2%E5%9B%AD-%E6%A5%9A%E4%BA%BALeo-brightgreen.svg)](https://www.cnblogs.com/leolion/)




### 项目起源

在经过大量的WPF项目开发后，积累了大量好用的Infrastructure，故整理并发布出来，供后来者快速开发WPF应用程序。您可以选择引用生成的dll，也可以拷贝您需要的代码到您的项目中。严格的说该项目不是一个框架，而是由离散的基础设施组成。



## 项目组成

项目由Wif.Core、Wif.Utils和Wif.Infrastructure组成。

- ##### Wif.Core：WPF基础设施框架的核心部分，该部分代码一般是XAML不相关的，有最少的依赖项，包含Async、Cache、Collections、ComponentModel、Generic、Setting。

- ##### Wif.Utils：WPF基础设施框架的扩展部分，主要是一些Extensions、Helpers。

- ##### Wif.Infrastructure：WPF基础设施框架的主要部分，包含Binding、PropertyChanged、MarkupExtensions、Converters、Commands、EventToCommand、Behaviors、ValidationRules等大量实用基础设施。



## 组成图

![组成图](https://github.com/Leolion610/wif/blob/master/Resources/Images/wif_ConstitutionalDiagram.png?raw=true)


## 博客

> **【[wif 系列](https://www.cnblogs.com/leolion/p/10275027.html)】**



## 文档

- [在C#中实现单例模式](https://github.com/LeoYang610/wif/blob/master/Docs/%E5%9C%A8C%23%E4%B8%AD%E5%AE%9E%E7%8E%B0%E5%8D%95%E4%BE%8B%E6%A8%A1%E5%BC%8F.md)
- [C#之单例模式（Singleton Pattern）最佳实践](https://github.com/LeoYang610/wif/blob/master/Docs/C%23%E4%B9%8B%E5%8D%95%E4%BE%8B%E6%A8%A1%E5%BC%8F%EF%BC%88Singleton%20Pattern%EF%BC%89%E6%9C%80%E4%BD%B3%E5%AE%9E%E8%B7%B5.md)



## 致敬感谢

项目中大部分代码源自于个人积累，同时有一些代码参阅了WPF开发者的分享，这部分代码已注明出处和链接，特此致敬！



## 适用人群

该项目中有一些MVVM框架的实现，但是并不用于替代MVVM框架。如果您开发的是一个小型项目，不考虑使用MVVM框架，本项目或许适合您实现简单MVVM模式。

如果您觉得现有的MVVM框架中的基础设施不能完全满足您的需要，您可以看看该项目中是否有适合的，以作为MVVM框架的辅助库。

如果您有代码洁癖，追求精致代码，这个项目可能适合您。

如果您是一位WPF初学者，该项目或许可以为您打开一扇门，窥见WPF设计的精妙之处。



## Licence

该项目根据[MIT许可证授权](https://github.com/LeoYang-Chuese/wif/blob/master/LICENSE)。

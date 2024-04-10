##### [English](https://github.com/Leolion610/wif/blob/master/README.md)| [中文](https://github.com/Leolion610/wif/blob/master/README.cn.md)

![Logo](https://github.com/Leolion610/wif/blob/master/Resources/Images/wif_logo.png?raw=true)

### WPF Infrastructure Framework（WIF）    　    　            　 　  

wif is a basic infrastructure framework that can help you quickly develop WPF applications.

Supports .NET Framework (4.7.2+) and .NET Core (3.1 and 5.0)

[![Nuget](https://img.shields.io/nuget/dt/Wif.Utilities.svg)](https://www.nuget.org/packages/Wif.Utilities/) [![nuget-version](https://img.shields.io/nuget/v/Wif.Infrastructure.svg)](https://www.nuget.org/packages/Wif.Infrastructure) ![dotnet-version](https://img.shields.io/badge/.NET%20Framework-%3E%3D4.7.2-blue.svg) ![dotnetcore version](https://img.shields.io/badge/.NET%20Core-%3E%3D3.1-blue.svg) ![csharp-version](https://img.shields.io/badge/C%23-8.0-blue.svg) ![IDE-version](https://img.shields.io/badge/IDE-vs2019-blue.svg) [![博客园](https://img.shields.io/badge/%E5%8D%9A%E5%AE%A2%E5%9B%AD-%E6%A5%9A%E4%BA%BALeo-brightgreen.svg)](https://www.cnblogs.com/leolion/)



### Project Origin

After a large number of WPF project developments, a large amount of useful infrastructure has been accumulated, and it has been organized and published, so that later developers can quickly develop WPF applications. You can choose to reference the generated dll, or you can copy the code you need into your project. Strictly speaking, this project is not a framework, but a collection of discrete infrastructure.

## Project Components

The project consists of Wif.Core, Wif.Utils and Wif.Infrastructure.

- ##### Wif.Core: The core part of the WPF infrastructure framework, this part of the code is generally XAML-irrelevant, has the least dependencies, and includes Async, Cache, Collections, ComponentModel, Generic, Setting.

- ##### Wif.Utils: The extension part of the WPF infrastructure framework, mainly some Extensions and Helpers.

- ##### Wif.Infrastructure: The main part of the WPF infrastructure framework, including Binding, PropertyChanged, MarkupExtensions, Converters, Commands, EventToCommand, Behaviors, ValidationRules and many other useful infrastructure.

## Composition Diagram

![组成图](https://github.com/Leolion610/wif/blob/master/Resources/Images/wif_ConstitutionalDiagram.png?raw=true)

## Blog

> **【[wif 系列](https://www.cnblogs.com/leolion/p/10275027.html)】**



## Documentation

- [在C#中实现单例模式](https://github.com/LeoYang610/wif/blob/master/Docs/%E5%9C%A8C%23%E4%B8%AD%E5%AE%9E%E7%8E%B0%E5%8D%95%E4%BE%8B%E6%A8%A1%E5%BC%8F.md)
- [C#之单例模式（Singleton Pattern）最佳实践](https://github.com/LeoYang610/wif/blob/master/Docs/C%23%E4%B9%8B%E5%8D%95%E4%BE%8B%E6%A8%A1%E5%BC%8F%EF%BC%88Singleton%20Pattern%EF%BC%89%E6%9C%80%E4%BD%B3%E5%AE%9E%E8%B7%B5.md)



## Acknowledgments

Most of the code in the project comes from personal accumulation, and some code has referred to the sharing of WPF developers, and the source and links have been noted.

## Target Audience

This project includes some MVVM framework implementations, but it is not intended to replace the MVVM framework. If you are developing a small project and do not consider using an MVVM framework, this project may be suitable for you to implement a simple MVVM pattern.

If you find that the infrastructure in existing MVVM frameworks cannot fully meet your needs, you can see if there is anything suitable in this project to serve as an auxiliary library for your MVVM framework.

If you have a code obsession and pursue elegant code, this project may be suitable for you.

If you are a WPF beginner, this project may open a door for you to glimpse the exquisiteness of WPF design.

## Contact Information

Email: [leolion610@foxmail.com](mailto:leolion610@foxmail.com)

## Licence

This project is licensed under the [MIT License](https://github.com/LeoYang-Chuese/wif/blob/master/LICENSE).

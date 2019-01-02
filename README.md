![Logo](https://github.com/Leolion610/wif/blob/master/Pictures/wif_logo.png?raw=true)

# WPF Infrastructure Framework（WIF）[English](https://github.com/Leolion610/wif/blob/master/README.EN.md)

WPF Infrastructure Framework（WIF）是一个基础设施框架，可帮助您快速开发WPF应用程序。

## 项目起源

在经过大量的WPF项目开发后，积累了大量好用的Infrastructure，故整理并发布出来，供后来者快速开发WPF应用程序。您可以选择引用生成的dll，也可以拷贝您需要的代码到您的项目中。严格的说该项目不是一个框架，而是由离散的基础设施组成。

## 项目组成

项目由Wif.Core、Wif.Infrastructure和Wif.Utils组成。

- ##### Wif.Core：WPF基础设施框架的核心部分，该部分代码一般是XAML不相关的，有最少的依赖项，包含Async、Cache、Collections、ComponentModel、Generic、Setting。

- ##### Wif.Utils：WPF基础设施框架的扩展部分，主要是一些Extensions、Helpers。

- ##### Wif.Infrastructure：WPF基础设施框架的主要部分，包含Binding、PropertyChanged、MarkupExtensions、Converters、Commands、EventToCommand、Behaviors、ValidationRules等大量实用基础设施。

## 组成图

![组成图](https://github.com/Leolion610/wif/blob/master/Pictures/WifConstitutionalDiagram.png?raw=true)


# 博客

计划在博客园中写一系列文章介绍该框架的使用。

## 致敬感谢

项目中大部分代码源自于个人积累，同时有一些代码参阅了WPF开发者的分享，这部分代码已注明出处和链接，特此致敬！

## 适用人群

该项目中有一些MVVM框架的实现，但是并不用于替代MVVM框架。如果您开发的是一个小型项目，不考虑使用MVVM框架，本项目或许适合您实现简单MVVM模式。

如果您觉得现有的MVVM框架中的基础设施不能完全满足您的需要，您可以看看该项目中是否有适合的，以作为MVVM框架的辅助库。

如果您有代码洁癖，追求精致代码，这个项目可能适合您。

如果您是一位WPF初学者，该项目或许可以为您打开一扇门，窥见WPF设计的精妙之处。

## Licence

该项目根据[MIT许可证授权](https://github.com/Leolion610/wif/blob/master/LICENSE)。
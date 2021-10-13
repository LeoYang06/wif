/**************************************************************************
*      File Name：PathHelper.cs
*    Description：PathHelper.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Frontier.Wif.Utilities.Extensions;

namespace Frontier.Wif.Utilities.Helpers
{
    #region Enums

    /// <summary>
    /// 路径组成部分枚举。
    /// </summary>
    public enum PathComponent
    {
        /// <summary>
        /// 定义程序集公司名称。
        /// </summary>
        AssemblyCompany,

        /// <summary>
        /// 定义程序集名称。
        /// </summary>
        AssemblyName,

        /// <summary>
        /// 定义程序集版本号。
        /// </summary>
        AssemblyVersion,

        /// <summary>
        /// 定义程序集主版本号。
        /// </summary>
        AssemblyMajorVersion,

        /// <summary>
        /// 定义程序集主版本号，次版本号补0。
        /// </summary>
        AssemblyLongMajorVersion,

        /// <summary>
        /// 定义程序集次版本号。
        /// </summary>
        AssemblyMinorVersion,

        /// <summary>
        /// 定义程序集修订号，对应Version中的Build。
        /// </summary>
        AssemblyRevisionVersion,

        /// <summary>
        /// 定义程序集构建号，对应Version中的Revision。
        /// </summary>
        AssemblyBuildVersion
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="PathHelper" />
    /// </summary>
    public class PathHelper
    {
        #region Methods

        /// <summary>
        /// The GetSpecialFolder
        /// </summary>
        /// <param name="specialFolder">The specialFolder<see cref="Environment.SpecialFolder"/></param>
        /// <param name="paths">The paths<see cref="string[]"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GetSpecialFolder(Environment.SpecialFolder specialFolder, params string[] paths)
        {
            var rootFolder = Environment.GetFolderPath(specialFolder, Environment.SpecialFolderOption.Create);
            var pathList = new[] { rootFolder }.Concat(paths);
            return Path.Combine(pathList.ToArray());
        }

        /// <summary>
        /// The GetSpecialFolder
        /// </summary>
        /// <param name="specialFolder">The specialFolder<see cref="Environment.SpecialFolder"/></param>
        /// <param name="components">The components<see cref="PathComponent[]"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GetSpecialFolder(Environment.SpecialFolder specialFolder,
                params PathComponent[] components)
        {
            var paths = new List<string>();
            var assembly = Assembly.GetEntryAssembly();
            if (components.Contains(PathComponent.AssemblyCompany))
                paths.Add(assembly.GetCompany());

            if (components.Contains(PathComponent.AssemblyName))
                paths.Add(assembly?.GetName().Name);

            if (components.Contains(PathComponent.AssemblyVersion))
                paths.Add(assembly?.GetName().Version.ToString());

            if (components.Contains(PathComponent.AssemblyMajorVersion))
                paths.Add(assembly?.GetName().Version.Major.ToString());

            if (components.Contains(PathComponent.AssemblyLongMajorVersion))
                paths.Add(assembly?.GetName().Version.Major.ToString("F1"));

            if (components.Contains(PathComponent.AssemblyMinorVersion))
                paths.Add(assembly?.GetName().Version.Minor.ToString());

            if (components.Contains(PathComponent.AssemblyRevisionVersion))
                paths.Add(assembly?.GetName().Version.Build.ToString());

            if (components.Contains(PathComponent.AssemblyBuildVersion))
                paths.Add(assembly?.GetName().Version.Revision.ToString());

            return GetSpecialFolder(specialFolder, paths.ToArray());
        }

        #endregion
    }
}
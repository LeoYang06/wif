using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Frontier.Wif.Utils.Extensions;

namespace Frontier.Wif.Utils.Helpers
{
    #region Enums

    /// <summary>
    /// 路径组成部分枚举。
    /// </summary>
    public enum PathComponent
    {
        /// <summary>
        /// Defines the AssemblyCompany
        /// </summary>
        AssemblyCompany,

        /// <summary>
        /// Defines the AssemblyName
        /// </summary>
        AssemblyName,

        /// <summary>
        /// Defines the AssemblyVersion
        /// </summary>
        AssemblyVersion
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
            var pathList = new[] {rootFolder}.Concat(paths);
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
                paths.Add(assembly.GetName().Name);
            if (components.Contains(PathComponent.AssemblyVersion))
                paths.Add(assembly.GetName().Version.ToString());
            return GetSpecialFolder(specialFolder, paths.ToArray());
        }

        #endregion
    }
}
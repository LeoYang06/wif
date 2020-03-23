using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// Defines the <see cref="AssemblyExtensions" />
    /// </summary>
    public static class AssemblyExtensions
    {
        #region Methods

        /// <summary>
        /// The GetCompany
        /// </summary>
        /// <param name="assembly">The assembly<see cref="Assembly" /></param>
        /// <returns>The <see cref="string" /></returns>
        public static string GetCompany(this Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
            return attribute != null ? attribute.Company : "";
        }

        /// <summary>
        /// The GetCopyright
        /// </summary>
        /// <param name="assembly">The assembly<see cref="Assembly" /></param>
        /// <returns>The <see cref="string" /></returns>
        public static string GetCopyright(this Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            return attribute != null ? attribute.Copyright : "";
        }

        /// <summary>
        /// The GetDescription
        /// </summary>
        /// <param name="assembly">The assembly<see cref="Assembly" /></param>
        /// <returns>The <see cref="string" /></returns>
        public static string GetDescription(this Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
            return attribute != null ? attribute.Description : "";
        }

        /// <summary>
        /// The GetFileVersion
        /// </summary>
        /// <param name="assembly">The assembly<see cref="Assembly" /></param>
        /// <returns>The <see cref="string" /></returns>
        public static string GetFileVersion(this Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            return attribute != null ? attribute.Version : "";
        }

        /// <summary>
        /// The GetGUID
        /// </summary>
        /// <param name="assembly">The assembly<see cref="Assembly" /></param>
        /// <returns>The <see cref="string" /></returns>
        public static string GetGuid(this Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<GuidAttribute>();
            return attribute != null ? attribute.Value : "";
        }

        /// <summary>
        /// The GetInfoVersion
        /// </summary>
        /// <param name="assembly">The assembly<see cref="Assembly" /></param>
        /// <returns>The <see cref="string" /></returns>
        public static string GetInfoVersion(this Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return attribute != null ? attribute.InformationalVersion : "";
        }

        /// <summary>
        /// The GetName
        /// </summary>
        /// <param name="assembly">The assembly<see cref="Assembly" /></param>
        /// <returns>The <see cref="AssemblyName" /></returns>
        public static AssemblyName GetName(this Assembly assembly)
        {
            return new AssemblyName(assembly.FullName);
        }

        /// <summary>
        /// The GetProduct
        /// </summary>
        /// <param name="assembly">The assembly<see cref="Assembly" /></param>
        /// <returns>The <see cref="string" /></returns>
        public static string GetProduct(this Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<AssemblyProductAttribute>();
            return attribute != null ? attribute.Product : "";
        }

        /// <summary>
        /// The GetTitle
        /// </summary>
        /// <param name="assembly">The assembly<see cref="Assembly" /></param>
        /// <returns>The <see cref="string" /></returns>
        public static string GetTitle(this Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            return attribute != null ? attribute.Title : "";
        }

        /// <summary>
        /// The GetTypeLibGuid
        /// </summary>
        /// <param name="assembly">The assembly<see cref="Assembly" /></param>
        /// <returns>The <see cref="Guid" /></returns>
        public static Guid GetTypeLibGuid(this Assembly assembly)
        {
            return Marshal.GetTypeLibGuidForAssembly(assembly);
        }

        /// <summary>
        /// The GetVersion
        /// </summary>
        /// <param name="assembly">The assembly<see cref="Assembly" /></param>
        /// <returns>The <see cref="string" /></returns>
        public static string GetVersion(this Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
            return attribute != null ? attribute.Version : "";
        }

        #endregion
    }
}
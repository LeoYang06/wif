using System;

namespace Wif.Demo
{
    /// <summary>
    /// A Locator which will host the container for dependency injection based operations.
    /// </summary>
    public static class Locator
    {
        /// <summary>
        /// Gets the read only dependency resolver.
        /// </summary>
        public static IServiceProvider Container { get; private set; }

        /// <summary>Allows setting the dependency resolver.</summary>
        /// <param name="serviceProvider">The dependency resolver to set.</param>
        public static void SetLocator(IServiceProvider serviceProvider)
        {
            Container = serviceProvider;
        }
    }
}
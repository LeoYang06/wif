/**************************************************************************
*      File Name：SingletonProvider.cs
*    Description：SingletonProvider.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;

namespace Frontier.Wif.Core.Generic
{
    /// <summary>
    /// Static helper used to create or get a singleton from another class.
    /// </summary>
    /// <typeparam name="T">The type to create or get a singleton.</typeparam>
    public static class SingletonProvider<T> where T : class, new()
    {
        #region Fields

        /// <summary>
        /// Gets the singleton of the given type.
        /// </summary>
        private static readonly Lazy<T> _lazy = new Lazy<T>(() => new T());

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton of the given type.
        /// </summary>
        public static T Instance => _lazy.Value;

        #endregion
    }

    /// <summary>
    /// Static helper used to create or get a singleton from another class.
    /// </summary>
    public static class SingletonProvider
    {
        #region Methods

        /// <summary>
        /// Gets the singleton of the given type.
        /// </summary>
        /// <typeparam name="TParameter">Given singleton type.</typeparam>
        /// <returns>The <see cref="TParameter" />Singleton object.</returns>
        public static TParameter Get<TParameter>() where TParameter : class, new()
        {
            return SingletonProvider<TParameter>.Instance;
        }

        #endregion
    }

    /// <summary>
    /// This is just an example of a normal singleton.
    /// </summary>
    internal sealed class Singleton
    {
        #region Fields

        /// <summary>
        /// Defines the _lazy
        /// </summary>
        private static readonly Lazy<Singleton> _lazy = new Lazy<Singleton>(() => new Singleton());

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Instance
        /// </summary>
        public static Singleton Instance => _lazy.Value;

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="Singleton"/> class from being created.
        /// </summary>
        private Singleton()
        {
        }

        #endregion
    }
}
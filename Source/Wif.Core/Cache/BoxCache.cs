using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Frontier.Wif.Core.Cache
{
    /// <summary>
    /// 值类型缓存盒。Defines the <see cref="BoxCache" />
    ///     http://www.singulink.com/CodeIndex/post/value-type-box-cache
    /// </summary>
    public static class BoxCache
    {
        #region Fields

        /// <summary>
        /// Defines the _intialized
        /// </summary>
        internal static int _intialized;

        #endregion

        #region Methods

        /// <summary>
        /// Adds a box for the specified value into the cache. Note that the cache uses a copy-on-write mechanism to ensure
        ///     thread safety and
        ///     ensure fastest possible lookup time so use this sparingly and use 'AddValues()' for multiple values instead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value<see cref="T"/></param>
        public static void AddValue<T>(T value) where T : struct
        {
            BoxCache<T>.AddValue(value);
        }

        /// <summary>
        /// Adds boxes for the specified values into the cache. Note that the cache uses a copy-on-write mechanism to ensure
        ///     thread safety and
        ///     ensure fastest possible lookup time so use this sparingly after application startup.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The values<see cref="IEnumerable{T}"/></param>
        public static void AddValues<T>(IEnumerable<T> values) where T : struct, IComparable<T>
        {
            BoxCache<T>.AddValues(values);
        }

        /// <summary>
        /// Gets a cached box for the specified value if one is cached, otherwise it returns a new box for the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value<see cref="T"/></param>
        /// <returns>The <see cref="object"/></returns>
        public static object GetBox<T>(T value)
        {
            return BoxCache<T>.GetBox(value);
        }

        /// <summary>
        /// Gets a cached box for the specified value. A box is created and added to the cache if it doesn't already exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value<see cref="T"/></param>
        /// <returns>The <see cref="object"/></returns>
        public static object GetOrAddBox<T>(T value)
        {
            return BoxCache<T>.GetOrAddBox(value);
        }

        /// <summary>
        /// The AddValues
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The <see cref="T[]" /></param>
        internal static void AddValues<T>(params T[] values) where T : struct
        {
            BoxCache<T>.AddValues(values);
        }

        /// <summary>
        /// The IntializeDefaults
        /// </summary>
        internal static void IntializeDefaults()
        {
            if (Interlocked.CompareExchange(ref _intialized, 1, 0) == 0)
            {
                // If you are only caching a default value then don't bother doing it here,
                // it gets automatically added in the generic BoxCache<T> initializer.

                AddValues(false, true);

                AddValues(Enumerable.Range(0, 255).Select(v => (byte) v));
                AddValues(Enumerable.Range(-128, 127).Select(v => (sbyte) v));
                AddValues(Enumerable.Range(-10, 256).Select(v => (short) v));
                AddValues(Enumerable.Range(0, 256).Select(v => (ushort) v));
                AddValues(Enumerable.Range(-10, 256).Select(v => v));
                AddValues(Enumerable.Range(0, 256).Select(v => (uint) v));
                AddValues(Enumerable.Range(-10, 256).Select(v => (long) v));
                AddValues(Enumerable.Range(0, 256).Select(v => (ulong) v));

                AddValues<float>(-1, 0, 1);
                AddValues<double>(-1, 0, 1);
                AddValues<decimal>(-1, 0, 1);
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="Helpers" />
    /// </summary>
    public static class Helpers
    {
        #region Methods

        /// <summary>
        /// The IsNullable
        /// </summary>
        /// <param name="type">The <see cref="Type" /></param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="BoxCache{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class BoxCache<T>
    {
        #region Fields

        /// <summary>
        /// Defines the _init
        /// </summary>
        internal static readonly bool _init = Initialize(); // Avoid static constructor performance penalty

        /// <summary>
        /// Defines the _syncRoot
        /// </summary>
        internal static readonly object _syncRoot = new object();

        /// <summary>
        /// Defines the _boxLookup
        /// </summary>
        internal static Dictionary<T, object> _boxLookup;

        /// <summary>
        /// Defines the _getNullableBox
        /// </summary>
        internal static Func<T, object> _getNullableBox;

        /// <summary>
        /// Defines the _getOrAddNullableBox
        /// </summary>
        internal static Func<T, object> _getOrAddNullableBox;

        #endregion

        #region Methods

        /// <summary>
        /// The AddValue
        /// </summary>
        /// <param name="value">The <see cref="T" /></param>
        /// <returns>The <see cref="object" /></returns>
        internal static object AddValue(T value)
        {
            Debug.Assert(!typeof(T).IsNullable(), "nullable types should always be forwarded to non-nullable cache");

            lock (_syncRoot)
            {
                if (_boxLookup.TryGetValue(value, out var obj))
                    return obj;

                object box = value;
                var newBoxLookup = new Dictionary<T, object>(_boxLookup) {{value, box}};
                _boxLookup = newBoxLookup;

                return box;
            }
        }

        /// <summary>
        /// The AddValues
        /// </summary>
        /// <param name="values">The <see cref="IEnumerable{T}" /></param>
        internal static void AddValues(IEnumerable<T> values)
        {
            Debug.Assert(!typeof(T).IsNullable(), "nullable types should always be forwarded to non-nullable cache");

            if (values == null)
                throw new ArgumentNullException(nameof(values));

            lock (_syncRoot)
            {
                // Avoid copying the dictionary if there are no new values.
                var newBoxLookup = _boxLookup == null ? new Dictionary<T, object>() : null;

                foreach (var value in values)
                    if (newBoxLookup != null)
                    {
                        if (!newBoxLookup.ContainsKey(value))
                            newBoxLookup.Add(value, value);
                    }
                    else if (!_boxLookup.ContainsKey(value))
                    {
                        newBoxLookup = new Dictionary<T, object>(_boxLookup) {{value, value}};
                    }

                _boxLookup = newBoxLookup;
            }
        }

        /// <summary>
        /// The GetBox
        /// </summary>
        /// <param name="value">The <see cref="T" /></param>
        /// <returns>The <see cref="object" /></returns>
        internal static object GetBox(T value)
        {
            if (_boxLookup == null)
            {
                Debug.Assert(typeof(T).IsNullable(), "box lookup should only be null for nullable types");

                if (value == null)
                    return null;

                return _getNullableBox(value);
            }

            Debug.Assert(!typeof(T).IsNullable(), "box lookup should not be created for nullable types");

            if (_boxLookup.TryGetValue(value, out var obj))
                return obj;

            return value;
        }

        /// <summary>
        /// The GetOrAddBox
        /// </summary>
        /// <param name="value">The <see cref="T" /></param>
        /// <returns>The <see cref="object" /></returns>
        internal static object GetOrAddBox(T value)
        {
            if (_boxLookup == null)
            {
                Debug.Assert(typeof(T).IsNullable(), "box lookup should only be null for nullable types");

                if (value == null)
                    return null;

                return _getOrAddNullableBox(value);
            }

            Debug.Assert(!typeof(T).IsNullable(), "box lookup should not be created for nullable types");

            if (_boxLookup.TryGetValue(value, out var obj))
                return obj;

            var box = AddValue(value);
            return box;
        }

        /// <summary>
        /// The Initialize
        /// </summary>
        /// <returns>The <see cref="bool" /></returns>
        internal static bool Initialize()
        {
            if (!typeof(T).IsValueType)
                throw new InvalidOperationException("Only value types can be boxed.");

            if (typeof(T).IsNullable())
            {
                // This is nullable so force static initializer for the underlying type cache to run so default boxes get populated there
                // and the delegates for this class are assigned

                var valueType = Nullable.GetUnderlyingType(typeof(T));
                var boxCacheEnumType = typeof(BoxCache<>).MakeGenericType(valueType);
                RuntimeHelpers.RunClassConstructor(boxCacheEnumType.TypeHandle);

                // Assign delegates so box requests are forwarded to the non-nullable cache

                var underlyingType = Nullable.GetUnderlyingType(typeof(T));
                var underlyingCacheType = typeof(BoxCache<>).MakeGenericType(underlyingType);

                var getMethod = underlyingCacheType
                        .GetMethod(nameof(GetNullableBox), BindingFlags.Static | BindingFlags.NonPublic)
                        .MakeGenericMethod(underlyingType);
                var getOrAddMethod = underlyingCacheType
                        .GetMethod(nameof(GetOrAddNullableBox), BindingFlags.Static | BindingFlags.NonPublic)
                        .MakeGenericMethod(underlyingType);

                _getNullableBox = (Func<T, object>) Delegate.CreateDelegate(typeof(Func<T, object>), getMethod);
                _getOrAddNullableBox = (Func<T, object>) Delegate.CreateDelegate(typeof(Func<T, object>), getMethod);
            }
            else
            {
                // Ensure default boxes are initialized
                BoxCache.IntializeDefaults();

                if (typeof(T).IsEnum)
                {
                    var values = (T[]) Enum.GetValues(typeof(T));
                    AddValues(values);
                }

                if (_boxLookup == null)
                    _boxLookup = new Dictionary<T, object>();

                if (!_boxLookup.ContainsKey(default))
                    _boxLookup.Add(default, default(T));
            }

            return true;
        }

        /// <summary>
        /// The GetNullableBox
        /// </summary>
        /// <typeparam name="TNullable"></typeparam>
        /// <param name="value">The <see cref="TNullable?" /></param>
        /// <returns>The <see cref="object" /></returns>
        private static object GetNullableBox<TNullable>(TNullable? value) where TNullable : struct
        {
            Debug.Assert(value.HasValue, "value required");
            return GetBox((T) (object) value.Value);
        }

        /// <summary>
        /// The GetOrAddNullableBox
        /// </summary>
        /// <typeparam name="TNullable"></typeparam>
        /// <param name="value">The <see cref="TNullable?" /></param>
        /// <returns>The <see cref="object" /></returns>
        private static object GetOrAddNullableBox<TNullable>(TNullable? value) where TNullable : struct
        {
            Debug.Assert(value.HasValue, "value required");
            return GetOrAddBox((T) (object) value.Value);
        }

        #endregion
    }
}
namespace Frontier.Wif.Utils.Extensions
{
    /// <summary>
    /// Defines the <see cref="ObjectExtensions" />
    /// </summary>
    public static class ObjectExtensions
    {
        #region Methods

        /// <summary>
        /// Performs a cast from object to <typeparamref name="T" />, avoiding possible null violations if
        ///     <typeparamref name="T" /> is a value type.
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The value casted to <typeparamref name="T" />, or <c>default(T)</c> if value is <c>null</c>.</returns>
        public static T SafeCast<T>(this object value)
        {
            return value == null ? default : (T) value;
        }

        /// <summary>
        /// 返回Object类型的安全状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SafeToString(this object obj)
        {
            return obj?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// 返回Object类型的安全状态
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string SafeToString(this object obj, string defaultValue)
        {
            return obj?.ToString() ?? defaultValue;
        }

        #endregion
    }
}
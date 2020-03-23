using System;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// Defines the <see cref="DateTimeExtensions" />
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Methods

        /// <summary>
        /// 转换时间为无格式字符串。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToNoformatString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// 转换时间为普通字符串。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToNormalString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd hh:mm:ss");
        }

        /// <summary>
        /// 转换时间为含毫秒的字符串。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToWithMillisecondString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 转换时间为下划线拼接字符串。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToWithUnderlineString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy_MM_dd_HH_mm_ss");
        }

        #endregion
    }
}
/**************************************************************************
 *      File Name：SerializationHelper.cs
 *    Description：SerializationHelper.cs class description...
 *      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
 *        Creator：Leo Yang
 *    Create Time：2024/03/08
 *Project Address：https://github.com/LeoYang-Chuese/wif
 **************************************************************************/


using System;
using System.Text;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// 表示字符编码转换器。
    /// </summary>
    public class CharEncodingConverter
    {
        #region 字符串转16进制编解码

        /// <summary>
        /// 字符串转十六进制编码。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToHex(string str)
        {
            var sb = new StringBuilder();
            foreach (char c in str)
            {
                sb.Append(((int)c).ToString("X4"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 十六进制编码转字符串。
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="str">出现不符合十六进制字符则返回空字符。</param>
        /// <returns>true：转换成功；false：转换失败</returns>
        public static bool HexToString(string hex, out string str)
        {
            str = string.Empty;
            var sb = new StringBuilder();
            for (var i = 0; i < hex.Length; i += 4)
            {
                string hexChar = hex.Substring(i, 4);
                if (!IsHex(hexChar))
                {
                    return false;
                }

                sb.Append((char)Convert.ToInt32(hexChar, 16));
            }

            str = sb.ToString();
            return true;
        }

        /// <summary>
        /// 判断字符串是否符合十六进制数。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsHex(string str)
        {
            foreach (char c in str)
            {
                if (!Uri.IsHexDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
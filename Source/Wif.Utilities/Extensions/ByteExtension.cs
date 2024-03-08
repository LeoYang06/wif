/**************************************************************************
 *      File Name：SerializationHelper.cs
 *    Description：SerializationHelper.cs class description...
 *      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
 *        Creator：Leo Yang
 *    Create Time：2024/03/08
 *Project Address：https://github.com/LeoYang-Chuese/wif
 **************************************************************************/


using System;
using System.Collections;
using System.Linq;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// 表示bit、byte和数据类型转换的扩展方法。
    /// </summary>
    public static class ByteExtension
    {
        #region BitArray Extension

        /// <summary>
        /// Convert bits to bytes.
        /// </summary>
        /// <param name="bitArray"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this BitArray bitArray)
        {
            int length = bitArray.Length / 8 + (bitArray.Length % 8 == 0 ? 0 : 1);
            var array  = new byte[length];
            bitArray.CopyTo(array, 0);
            return array;
        }

        /// <summary>
        /// Convert bits to short.
        /// </summary>
        /// <param name="bitArray"></param>
        /// <returns></returns>
        public static short ToShort(this BitArray bitArray)
        {
            byte[] bytes = bitArray.ToBytes();
            return bytes.ToShort();
        }

        #endregion BitArray Extension

        #region ValueType  Extension

        /// <summary>
        /// Convert short to bits.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BitArray ToBitArray(this short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            var    bits  = new BitArray(bytes);
            return bits;
        }

        /// <summary>
        /// Convert bytes to bits.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static short ToShort(this byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                return BitConverter.ToInt16(bytes, 0);
            }

            byte[] revValue = bytes.Reverse().ToArray();
            return BitConverter.ToInt16(revValue, 0);
        }

        #endregion ValueType  Extension
    }
}
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// 对象二进制序列化扩展方法。
    /// </summary>
    public static class BinarySerializationExtensions
    {
        #region Methods

        /// <summary>
        /// 反序列化。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this Stream stream)
        {
            var formatter = new BinaryFormatter();
            stream.Position = 0;
            return (T) formatter.Deserialize(stream);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Stream Serialize(this object source)
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, source);
            return stream;
        }

        #endregion
    }
}
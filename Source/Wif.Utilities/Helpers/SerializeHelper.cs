using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Frontier.Wif.Utilities.Helpers
{
    /// <summary>
    /// 各种串行化的帮助类
    /// </summary>
    public static class SerializeHelper
    {
        #region Methods

        /// <summary>
        /// 将字符串反序列化成指定类型的对象。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="str">对象序列化后的字符串。</param>
        /// <returns></returns>
        public static T DeserializeFromBase64String<T>(string str)
        {
            var buffer = Convert.FromBase64String(str);
            return DeserializeFromBytes<T>(buffer);
        }

        /// <summary>
        /// 将文件反序列化成指定类型的对象。
        ///     <remarks>自动识别编码。</remarks>
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="path">路径。</param>
        /// <returns>反序列化的对象。</returns>
        public static T DeserializeFromXmlFile<T>(string path)
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(path);
                var xmlSerializer = new XmlSerializer(typeof(T));
                return (T) xmlSerializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader?.Close();
            }
        }

        /// <summary>
        /// 将Xml字符串反序列化成指定类型的对象。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="xml">对象序列化后的字符串。</param>
        /// <returns></returns>
        public static T DeserializeFromXmlString<T>(string xml)
        {
            T target = default;
            StringReader sr = null;

            try
            {
                var xs = new XmlSerializer(typeof(T));

                sr = new StringReader(xml); // containing the XML data to read
                using (var xtr = new XmlTextReader(sr))
                {
                    sr = null;
                    var o = xs.Deserialize(xtr);
                    if (o is T variable) target = variable;
                }
            }
            finally
            {
                sr?.Close();
            }

            return target;
        }

        /// <summary>
        /// 将对象序列化成字符串。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="obj">被序列化的对象。</param>
        /// <returns></returns>
        public static string SerializeToBase64String<T>(T obj)
        {
            var buffer = SerializeToBytes(obj);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 将对象序列化为Xml文件。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="path">路径。</param>
        public static void SerializeToXmlFile(object obj, string path)
        {
            XmlTextWriter writer = null;
            try
            {
                writer = new XmlTextWriter(path, Encoding.Default) {Formatting = Formatting.Indented};
                var xmlSerializer = new XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(writer, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                writer?.Close();
            }
        }

        /// <summary>
        /// 将对象序列化成Xml字符串。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="target">被序列化的对象。</param>
        /// <returns></returns>
        public static string SerializeToXmlString<T>(T target)
        {
            string str = null;
            MemoryStream ms = null;

            try
            {
                var xs = new XmlSerializer(typeof(T));
                // stream to which you want to write.
                ms = new MemoryStream();
                using (var xtw = new XmlTextWriter(ms, Encoding.Default)
                {
                        Formatting = Formatting.Indented
                })
                {
                    ms = null;
                    xs.Serialize(xtw, target);
                    xtw.BaseStream.Seek(0, SeekOrigin.Begin);
                    using (var sr = new StreamReader(xtw.BaseStream))
                    {
                        str = sr.ReadToEnd();
                    }
                }
            }
            finally
            {
                ms?.Close();
            }

            return str;
        }

        /// <summary>
        /// 将二进制数组反序列化成指定类型的对象。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="source">对象序列化后的byte[]。</param>
        /// <returns></returns>
        private static T DeserializeFromBytes<T>(byte[] source)
        {
            var obj = default(T);
            IFormatter formatter = new BinaryFormatter();
            T result;
            using (var memoryStream = new MemoryStream(source))
            {
                obj = (T) formatter.Deserialize(memoryStream);
                result = obj;
            }

            return result;
        }

        /// <summary>
        /// 将对象序列化成Bytes数组
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="obj">被序列化的对象</param>
        /// <returns></returns>
        private static byte[] SerializeToBytes<T>(T obj)
        {
            IFormatter formatter = new BinaryFormatter();
            byte[] result;
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Position = 0L;
                formatter.Serialize(memoryStream, obj);
                result = memoryStream.ToArray();
            }

            return result;
        }

        #endregion
    }
}
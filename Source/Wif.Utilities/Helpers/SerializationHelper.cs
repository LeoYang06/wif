/**************************************************************************
*      File Name：SerializationHelper.cs
*    Description：SerializationHelper.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace Frontier.Wif.Utilities.Helpers
{
    /// <summary>
    /// 各种串行化的帮助类。
    /// </summary>
    public static class SerializationHelper
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
            byte[] buffer = Convert.FromBase64String(str);
            return DeserializeFromBytes<T>(buffer);
        }

        /// <summary>
        /// 将Xml字符串反序列化成指定类型的对象。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="xml">对象序列化后的字符串。</param>
        /// <returns></returns>
        public static T DeserializeObjectFromXmlString<T>(string xml)
        {
            T target = default;
            StringReader strReader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                // containing the XML data to read
                strReader = new StringReader(xml);
                var settings = new XmlReaderSettings {Async = true};
                using (var xtr = XmlReader.Create(strReader, settings))
                {
                    strReader = null;
                    object o = serializer.Deserialize(xtr);
                    if (o is T variable)
                        target = variable;
                }
            }
            finally
            {
                strReader?.Close();
                strReader?.Dispose();
            }

            return target;
        }

        /// <summary>
        /// 将文件反序列化成指定类型的对象。
        /// </summary>
        /// <typeparam name="T">目标对象的类型。</typeparam>
        /// <param name="sourceFile">源文件路径。</param>
        /// <returns>反序列化的目标对象。</returns>
        public static T DeserializeObjectFromXmlFile<T>(string sourceFile) where T : class
        {
            return DeserializeObjectFromXmlFile(sourceFile, typeof(T)) as T;
        }

        /// <summary>
        /// 将文件反序列化成指定类型的对象。
        /// </summary>
        /// <param name="sourceFile">源文件路径。</param>
        /// <param name="targetTypeInfo">目标对象的类型</param>
        /// <returns>反序列化的目标对象。</returns>
        public static object DeserializeObjectFromXmlFile(string sourceFile, Type targetTypeInfo)
        {
            FileStream fileStream = null;
            try
            {
                using (fileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                {
                    var serializer = new XmlSerializer(targetTypeInfo);
                    object targetObject = serializer.Deserialize(fileStream);
                    return targetObject;
                }
            }
            finally
            {
                fileStream?.Close();
                fileStream?.Dispose();
            }
        }

        /// <summary>
        /// 将对象序列化成字符串。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="obj">被序列化的对象。</param>
        /// <returns></returns>
        public static string SerializeToBase64String<T>(T obj)
        {
            byte[] buffer = SerializeToBytes(obj);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 将对象序列化成Xml字符串。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="sourceObject">被序列化的对象。</param>
        /// <param name="prefix">The prefix associated with an XML namespace.</param>
        /// <param name="ns">An XML namespace.</param>
        /// <returns></returns>
        public static string SerializeObjectToXmlString<T>(T sourceObject, string prefix = "", string ns = "")
        {
            string xmlString = null;
            StringWriter stringWriter = null;
            XmlWriter xmlWriter = null;
            try
            {
                stringWriter = new StringWriter();

                var xmlNamespaces = new XmlSerializerNamespaces();
                xmlNamespaces.Add(prefix, ns);
                var settings = new XmlWriterSettings {Indent = true, IndentChars = "\t"};
                xmlWriter = XmlWriter.Create(stringWriter, settings);

                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(xmlWriter, sourceObject, xmlNamespaces);
                xmlString = xmlWriter.ToString();
            }
            finally
            {
                xmlWriter?.Close();
                xmlWriter?.Dispose();
                stringWriter?.Close();
                stringWriter?.Dispose();
            }

            return xmlString;
        }

        /// <summary>
        /// 将对象序列化为Xml文件。
        /// </summary>
        /// <param name="sourceObject">源对象。</param>
        /// <param name="targetFile">序列化文件存储路径。</param>
        /// <param name="prefix">The prefix associated with an XML namespace.</param>
        /// <param name="ns">An XML namespace.</param>
        public static void SerializeObjectToXmlFile(object sourceObject, string targetFile, string prefix = "", string ns = "")
        {
            XmlWriter xmlWriter = null;
            try
            {
                var xmlNamespaces = new XmlSerializerNamespaces();
                xmlNamespaces.Add(prefix, ns);
                var settings = new XmlWriterSettings {Indent = true, IndentChars = "\t"};
                xmlWriter = XmlWriter.Create(targetFile, settings);
                var xmlSerializer = new XmlSerializer(sourceObject.GetType());
                xmlSerializer.Serialize(xmlWriter, sourceObject, xmlNamespaces);
            }
            finally
            {
                xmlWriter?.Close();
                xmlWriter?.Dispose();
            }
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
                obj    = (T) formatter.Deserialize(memoryStream);
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
/**************************************************************************
*      File Name：FileHelper.cs
*    Description：FileHelper.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Frontier.Wif.Utilities.Helpers
{
    /// <summary>
    /// 文件目录帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 向文本文件的尾部追加内容，换行。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="contents">写入的内容集合</param>
        public static void AppendAllLines(string path, IEnumerable<string> contents)
        {
            using (var writer = new StreamWriter(path, true, Encoding.UTF8))
            {
                foreach (string content in contents)
                    writer.WriteLine(content);
            }
        }

        /// <summary>
        /// 向文本文件的尾部追加内容，换行。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">写入的内容</param>
        public static void AppendLine(string path, string content)
        {
            using (var writer = new StreamWriter(path, true, Encoding.UTF8))
            {
                writer.WriteLine(content);
            }
        }

        /// <summary>
        /// 向文本文件的尾部追加内容，不换行。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">写入的内容</param>
        public static void AppendText(string path, string content)
        {
            using (var writer = new StreamWriter(path, true, Encoding.UTF8))
            {
                writer.Write(content);
            }
        }

        /// <summary>
        /// 清空指定目录下所有文件及子目录，但该目录依然保留。
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                //删除目录中所有的文件
                string[] fileNames = Directory.GetFiles(directoryPath);
                foreach (string t in fileNames)
                    DeleteFile(t);

                //删除目录中所有的子目录
                string[] directoryNames = Directory.GetDirectories(directoryPath);
                foreach (string t in directoryNames)
                    Directory.Delete(t);
            }
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">The searchPattern <see cref="string" /></param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //获取指定的文件列表
            string[] fileNames = GetFileFullNames(directoryPath, searchPattern, isSearchChild);

            //判断指定文件是否存在
            if (fileNames.Length == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 将源文件的内容复制到目标文件中
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="destFilePath">目标文件的绝对路径</param>
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }

        /// <summary>
        /// 指定文件夹下面的所有内容copy到目标文件夹下面
        /// <remarks>如果目标文件夹为只读属性就会报错</remarks>
        /// </summary>
        /// <param name="srcPath">原始路径</param>
        /// <param name="destPath">目标文件夹</param>
        public static void CopyDir(string srcPath, string destPath)
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (destPath[destPath.Length - 1] != Path.DirectorySeparatorChar)
                destPath += Path.DirectorySeparatorChar;
            // 判断目标目录是否存在如果不存在则新建之
            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法 string[] fileList = Directory.GetFiles(srcPath);
            string[] fileList = Directory.GetFileSystemEntries(srcPath);

            //遍历所有的文件和目录
            foreach (string file in fileList)
                if (Directory.Exists(file))
                    CopyDir(file, destPath + Path.GetFileName(file));
                //否则直接Copy文件
                else
                    File.Copy(file, destPath + Path.GetFileName(file), true);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceDir">要复制的文件的路径已经全名(包括后缀)</param>
        /// <param name="destDir">目标位置,并指定新的文件名</param>
        public static void CopyFile(string sourceDir, string destDir)
        {
            sourceDir = sourceDir.Replace("/", "\\");
            destDir = destDir.Replace("/", "\\");
            if (File.Exists(sourceDir))
                File.Copy(sourceDir, destDir, true);
        }

        /// <summary>
        /// 复制文件夹(递归)
        /// </summary>
        /// <param name="fromDirectory">源文件夹路径</param>
        /// <param name="toDirectory">目标文件夹路径</param>
        public static void CopyFolder(string fromDirectory, string toDirectory)
        {
            Directory.CreateDirectory(toDirectory);

            if (!Directory.Exists(fromDirectory))
                return;

            string[] directories = Directory.GetDirectories(fromDirectory);

            if (directories.Length > 0)
                foreach (string d in directories)
                    CopyFolder(d, toDirectory + d.Substring(d.LastIndexOf("\\", StringComparison.Ordinal)));
            string[] files = Directory.GetFiles(fromDirectory);
            if (files.Length > 0)
                foreach (string s in files)
                    File.Copy(s, toDirectory + s.Substring(s.LastIndexOf("\\", StringComparison.Ordinal)), true);
        }

        /// <summary>
        /// 判断文件路径是否是文件夹。
        /// </summary>
        /// <param name="filePath">文件或文件夹路径。</param>
        /// <returns></returns>
        public static bool IsDirectory(string filePath)
        {
            // 判断是文件还是文件夹。
            string extension = Path.GetExtension(filePath);
            return string.IsNullOrWhiteSpace(extension);
        }

        /// <summary>
        /// 创建目录，如果是文件路径则创建文件的父目录。
        /// </summary>
        /// <param name="filePath">文件或文件夹路径。</param>
        public static void CreateDirectory(string filePath)
        {
            string dirPath = filePath;
            if (!IsDirectory(filePath))
            {
                dirPath = Path.GetDirectoryName(filePath);
            }

            // 检查目录是否存在。
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath ?? throw new InvalidOperationException());
        }

        /// <summary>
        /// 创建一个文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            // 创建目录
            CreateDirectory(filePath);
            //创建文件
            using (new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
            }
        }

        /// <summary>
        /// 创建一个文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="isOverwritten">是否覆盖，默认不覆盖</param>
        public static void CreateFile(string filePath, bool isOverwritten = false)
        {
            if (isOverwritten)
            {
                //创建文件
                CreateFile(filePath);
            }
            else
            {
                //如果文件不存在则创建该文件
                if (File.Exists(filePath))
                    return;

                //创建文件
                CreateFile(filePath);
            }
        }

        /// <summary>
        /// 创建一个文件,并将字节流写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="buffer">二进制流数据</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            //如果文件不存在则创建该文件
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                //写入二进制流
                fs.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file">要删除的文件路径和名称</param>
        public static void DeleteFile(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
        }

        /// <summary>
        /// 递归删除文件夹目录及文件
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
        }

        /// <summary>
        /// 删除指定文件夹对应其他文件夹里的文件
        /// </summary>
        /// <param name="fromDirectory">指定文件夹路径</param>
        /// <param name="toDirectory">对应其他文件夹路径</param>
        public static void DeleteFolderFiles(string fromDirectory, string toDirectory)
        {
            Directory.CreateDirectory(toDirectory);

            if (!Directory.Exists(fromDirectory))
                return;

            string[] directories = Directory.GetDirectories(fromDirectory);

            if (directories.Length > 0)
                foreach (string d in directories)
                    DeleteFolderFiles(d, toDirectory + d.Substring(d.LastIndexOf("\\", StringComparison.Ordinal)));

            string[] files = Directory.GetFiles(fromDirectory);

            if (files.Length > 0)
                foreach (string s in files)
                    File.Delete(toDirectory + s.Substring(s.LastIndexOf("\\", StringComparison.Ordinal)));
        }

        /// <summary>
        /// 检查文件,如果文件不存在则创建
        /// </summary>
        /// <param name="path">路径,包括文件名</param>
        public static void ExistsFile(string path)
        {
            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                fs.Close();
            }
        }

        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">The searchPattern <see cref="string" /></param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns>The <see cref="string[]" /></returns>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            if (isSearchChild)
                return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
            return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;
            var di = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in di.GetFiles())
                len += fi.Length;
            var dis = di.GetDirectories();
            if (dis.Length > 0)
                foreach (DirectoryInfo t in dis)
                    len += GetDirectoryLength(t.FullName);

            return len;
        }

        /// <summary>
        /// 获取目录名称
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>目录名称</returns>
        public static string GetDirectoryName(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                return string.Empty;
            return new DirectoryInfo(directoryPath).Name;
        }

        /// <summary>
        /// 获取指定目录及子目录中所有子目录名称列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">The searchPattern <see cref="string" /></param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns>The <see cref="IEnumerable{string}" /></returns>
        public static IEnumerable<string> GetDirectoryNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            var dirInfo = new DirectoryInfo(directoryPath);
            var dirInfos = dirInfo.GetDirectories(searchPattern, isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            return dirInfos.Select(x => x.Name);
        }

        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <returns>The <see cref="string[]" /></returns>
        public static string[] GetFileFullNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常
            if (!Directory.Exists(directoryPath))
                throw new FileNotFoundException("未能找到目录:" + directoryPath);

            //获取文件列表
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">The searchPattern <see cref="string" /></param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns>The <see cref="string[]" /></returns>
        public static string[] GetFileFullNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常
            if (!Directory.Exists(directoryPath))
                throw new FileNotFoundException("未能找到目录:" + directoryPath);

            if (isSearchChild)
                return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
            return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 获取一个文件的长度(GB MB KB)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns>The <see cref="int" /></returns>
        public static string GetFileFullSize(string filePath)
        {
            if (File.Exists(filePath))
            {
                //创建一个文件对象
                var fi = new FileInfo(filePath);
                //获取文件的大小
                long lenght = fi.Length;

                if (lenght / 1048576.00 > 1024)
                    return Math.Round(lenght / 1073741824.00, 2) + "GB";

                if (lenght / 1024.00 > 1024)
                    return Math.Round(lenght / 1048576.00, 2) + "MB";

                return Math.Round(lenght / 1024.00, 2) + "KB";
            }

            throw new FileNotFoundException("未能找到文件:" + filePath);
        }

        /// <summary>
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns>The <see cref="string" /></returns>
        public static string GetFileNameNoExtension(string filePath)
        {
            //获取文件的名称
            var fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }

        /// <summary>
        /// 获取一个文件的长度,单位为mb
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns>The <see cref="int" /></returns>
        public static int GetFileSize(string filePath)
        {
            if (File.Exists(filePath))
            {
                //创建一个文件对象
                var fi = new FileInfo(filePath);
                //获取文件的大小
                var lenght = (int)fi.Length;
                return lenght / 1048576;
            }

            throw new FileNotFoundException("未能找到文件:" + filePath);
        }

        /// <summary>
        /// 获取文本文件的行数
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns>The <see cref="int" /></returns>
        public static int GetLineCount(string filePath)
        {
            try
            {
                //将文本文件的各行读到一个字符串数组中
                string[] rows = File.ReadAllLines(filePath);

                //返回行数
                return rows.Length;
            }
            catch (Exception)
            {
                throw new FileNotFoundException("未能找到文件:" + filePath);
            }
        }

        /// <summary>
        /// 获取父目录路径
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>父目录路径</returns>
        public static string GetParentDirectoryPath(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                return string.Empty;
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath).Parent;
            if (directoryInfo != null)
                return directoryInfo.FullName;
            return string.Empty;
        }

        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool IsEmptyDirectory(string directoryPath)
        {
            //判断是否存在文件
            string[] fileNames = GetFileFullNames(directoryPath);
            if (fileNames.Length > 0)
                return false;

            //判断是否存在文件夹
            string[] directoryNames = Directory.GetDirectories(directoryPath);
            if (directoryNames.Length > 0)
                return false;

            return true;
        }

        /// <summary>
        /// 将文件移动到指定目录
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="destDirectoryPath">移动到的目录的绝对路径</param>
        public static void MoveFile(string sourceFilePath, string destDirectoryPath)
        {
            //获取源文件的名称
            string sourceFileName = Path.GetFileName(sourceFilePath);

            if (Directory.Exists(destDirectoryPath))
            {
                //如果目标中存在同名文件,则删除
                if (File.Exists(destDirectoryPath + "\\" + sourceFileName))
                    DeleteFile(destDirectoryPath + "\\" + sourceFileName);
                //将文件移动到指定目录
                File.Move(sourceFilePath, destDirectoryPath + "\\" + sourceFileName);
            }
        }

        /// <summary>
        /// 读取所有文本
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="encoding">读取文件的编码。</param>
        /// <returns>所有文本</returns>
        public static string ReadAllText(string filePath, Encoding encoding)
        {
            string str;
            if (File.Exists(filePath))
                using (var reader = new StreamReader(filePath, encoding))
                {
                    str = reader.ReadToEnd();
                }
            else
                throw new FileNotFoundException("未能找到文件:" + filePath);

            return str;
        }

        /// <summary>
        /// 读取所有文本
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startLine">读取文本的起始行。</param>
        /// <param name="encoding">读取文件的编码。</param>
        /// <returns>从指定行开始的所有文本</returns>
        public static string ReadAllText(string filePath, int startLine, Encoding encoding)
        {
            var stringBuilder = new StringBuilder();
            if (File.Exists(filePath))
            {
                var num = 0;
                using (var reader = new StreamReader(filePath, encoding))
                {
                    string str;
                    while ((str = reader.ReadLine()) != null)
                    {
                        if (num < startLine)
                        {
                            num++;
                            continue;
                        }

                        stringBuilder.AppendLine(str);
                        num++;
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("未能找到文件:" + filePath);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 读取所有文本
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startLine">开始读取文本的起始行。</param>
        /// <param name="lineLenght">读取行数量</param>
        /// <param name="encoding">读取文件的编码。</param>
        /// <returns>从指定行开始的所有文本</returns>
        public static string ReadAllText(string filePath, int startLine, int lineLenght, Encoding encoding)
        {
            var stringBuilder = new StringBuilder();
            if (File.Exists(filePath))
            {

                var totalNum = 0;
                int count = GetLineCount(filePath);
                using (var reader = new StreamReader(filePath, encoding))
                {
                    for (var i = 0; i < count; i++)
                    {
                        if (i < startLine)
                        {
                            reader.ReadLine();
                            continue;
                        }

                        if (totalNum >= lineLenght)
                            break;
                        stringBuilder.AppendLine(reader.ReadLine());
                        totalNum++;
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("未能找到文件:" + filePath);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 从文件的指定行开始读取文本。
        /// </summary>
        /// <param name="filePath">文件路径。</param>
        /// <param name="encoding">读取文件的编码。</param>
        /// <param name="isIgnoreWhiteSpace">是否忽略空白行，默认保留。</param>
        /// <returns>读取的所有行。</returns>
        public static List<string> ReadLines(string filePath, Encoding encoding, bool isIgnoreWhiteSpace = false)
        {
            var lines = new List<string>();
            if (File.Exists(filePath))
                using (var reader = new StreamReader(filePath, encoding))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                        if (isIgnoreWhiteSpace)
                        {
                            if (!string.IsNullOrWhiteSpace(line))
                                lines.Add(line);
                        }
                        else
                        {
                            lines.Add(line);
                        }
                }
            else
                throw new FileNotFoundException("未能找到文件:" + filePath);

            return lines;
        }

        /// <summary>
        /// 从文件的指定行开始读取文本。
        /// </summary>
        /// <param name="filePath">文件路径。</param>
        /// <param name="startLine">读取文本的起始行，索引从1开始。</param>
        /// <param name="encoding">读取文件的编码。</param>
        /// <param name="isIgnoreWhiteSpace">是否忽略空白行，默认保留。</param>
        /// <returns>读取的所有行。</returns>
        public static List<string> ReadLines(string filePath, int startLine, Encoding encoding, bool isIgnoreWhiteSpace = false)
        {
            var lines = new List<string>();
            if (File.Exists(filePath))
            {
                var num = 0;
                using (var reader = new StreamReader(filePath, encoding))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        num++;
                        if (num < startLine)
                            continue;

                        if (isIgnoreWhiteSpace)
                        {
                            if (!string.IsNullOrWhiteSpace(line))
                                lines.Add(line);
                        }
                        else
                        {
                            lines.Add(line);
                        }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("未能找到文件:" + filePath);
            }

            return lines;
        }

        /// <summary>
        /// 从文件的指定行开始读取指定行数的文本。
        /// </summary>
        /// <param name="filePath">文件路径。</param>
        /// <param name="startLine">读取文本的起始行，索引从1开始。</param>
        /// <param name="lineLenght">读取文本的行数。</param>
        /// <param name="encoding">读取文件的编码。</param>
        /// <param name="isIgnoreWhiteSpace">是否忽略空白行，默认保留。</param>
        /// <returns>读取的所有行。</returns>
        public static List<string> ReadLines(string filePath, int startLine, int lineLenght, Encoding encoding, bool isIgnoreWhiteSpace = false)
        {
            var lines = new List<string>();
            if (File.Exists(filePath))
            {
                var num = 0;
                var totalNum = 0;
                using (var reader = new StreamReader(filePath, encoding))
                {
                    string line;
                    while (totalNum < lineLenght && (line = reader.ReadLine()) != null)
                    {
                        num++;
                        if (num < startLine)
                            continue;

                        if (isIgnoreWhiteSpace)
                        {
                            if (!string.IsNullOrWhiteSpace(line))
                                lines.Add(line);
                        }
                        else
                        {
                            lines.Add(line);
                        }
                        totalNum++;
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("未能找到文件:" + filePath);
            }

            return lines;
        }

        /// <summary>
        /// 创建一个新文件，向其中写入指定的字符串，然后关闭文件。 如果目标文件已存在，则覆盖该文件。
        /// </summary>
        /// <param name="filePath">要写入的文件路径。</param>
        /// <param name="content">要写入文件的字符串。</param>
        public static void WriteAllText(string filePath, string content)
        {
            // 创建目录
            CreateDirectory(filePath);

            // 写入文本内容
            using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            using var fs = new StreamWriter(fileStream, Encoding.UTF8);
            fs.Write(content);
        }

        /// <summary>
        /// 创建一个新文件，向其中写入一个字符串类型的二维集合的集合，然后关闭该文件。
        /// </summary>
        /// <param name="filePath">要写入的文件路径。</param>
        /// <param name="textLineFields">要写入文件的字符串类型的二维集合。</param>
        /// <param name="delimiter">分隔符。</param>
        /// <returns>是否完成写入操作。</returns>
        public static void WriteAllLines(string filePath, List<List<string>> textLineFields, string delimiter)
        {
            // 创建目录
            CreateDirectory(filePath);

            // 写入文本内容
            using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            using var fs = new StreamWriter(fileStream, Encoding.Default);
            foreach (string strLine in textLineFields.Select(textLine => string.Join(delimiter, textLine)))
            {
                fs.WriteLine(strLine);
            }
        }
    }
}
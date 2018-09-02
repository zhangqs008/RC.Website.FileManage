using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RC.FileManage
{
    /// <summary>
    ///     文件辅助类
    /// </summary>
    public class FileHelper
    {
        private const double KBCount = 1024;
        private const double MBCount = KBCount * 1024;
        private const double GBCount = MBCount * 1024;
        private const double TBCount = GBCount * 1024;

        /// <summary>
        ///     编码方式
        /// </summary>
        private static readonly Encoding Encoding = Encoding.UTF8;

        /// <summary>
        ///     递归取得文件夹下文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="list"></param>
        public static void GetFiles(string dir, List<string> list)
        {
            GetFiles(dir, list, new List<string>());
        }

        /// <summary>
        ///     递归取得文件夹下文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="list"></param>
        /// <param name="fileExtsions"></param>
        public static void GetFiles(string dir, List<string> list, List<string> fileExtsions)
        {
            //添加文件 
            var files = Directory.GetFiles(dir);
            if (fileExtsions.Count > 0)
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file);
                    if (extension != null && fileExtsions.Contains(extension)) list.Add(file);
                }
            else
                list.AddRange(files);

            //如果是目录，则递归
            var directories = new DirectoryInfo(dir).GetDirectories();
            foreach (var item in directories) GetFiles(item.FullName, list, fileExtsions);
        }

        /// <summary>
        ///     写入文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        public static void WriteFile(string filePath, string content, Encoding encoding)
        {
            try
            {
                var fs = new FileStream(filePath, FileMode.Create);
                var encode = encoding;
                //获得字节数组
                var data = encode.GetBytes(content);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        ///     写入文件(默认UTF-8)
        /// </summary>
        /// <param name="filePath">文件名</param>
        /// <param name="content">文件内容</param>
        public static void WriteFile(string filePath, string content)
        {
            try
            {
                var fs = new FileStream(filePath, FileMode.Create);
                var encode = Encoding;
                //获得字节数组
                var data = encode.GetBytes(content);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        ///     读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding);
        }

        /// <summary>
        ///     读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath, Encoding encoding)
        {
            using (var sr = new StreamReader(filePath, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        ///     读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> ReadFileLines(string filePath)
        {
            var str = new List<string>();
            using (var sr = new StreamReader(filePath, Encoding))
            {
                string input;
                while ((input = sr.ReadLine()) != null) str.Add(input);
            }

            return str;
        }

        /// <summary>
        ///     复制文件夹（及文件夹下所有子文件夹和文件）
        /// </summary>
        /// <param name="sourcePath">待复制的文件夹路径</param>
        /// <param name="destinationPath">目标路径</param>
        public static void CopyDirectory(string sourcePath, string destinationPath)
        {
            var info = new DirectoryInfo(sourcePath);
            if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);
            foreach (var fsi in info.GetFileSystemInfos())
            {
                var destName = Path.Combine(destinationPath, fsi.Name);

                if (fsi is FileInfo) //如果是文件，复制文件
                {
                    File.Copy(fsi.FullName, destName, true);
                }
                else //如果是文件夹，新建文件夹，递归
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }

        /// <summary>
        ///     删除文件夹（及文件夹下所有子文件夹和文件）
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void DeleteFolder(string directoryPath)
        {
            try
            {
                foreach (var d in Directory.GetFileSystemEntries(directoryPath))
                    if (File.Exists(d))
                    {
                        var fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                            fi.Attributes = FileAttributes.Normal;
                        File.Delete(d); //删除文件   
                    }
                    else
                    {
                        DeleteFolder(d); //删除文件夹
                    }

                Directory.Delete(directoryPath); //删除空文件夹
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///     清空文件夹（及文件夹下所有子文件夹和文件）
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void ClearFolder(string directoryPath)
        {
            foreach (var d in Directory.GetFileSystemEntries(directoryPath))
                if (File.Exists(d))
                {
                    var fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d); //删除文件   
                }
                else
                {
                    DeleteFolder(d); //删除文件夹
                }
        }

        /// <summary>
        ///     得到适应的大小
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetFileSize(string filepath)
        {
            return GetFileSize(filepath, 2);
        }
        public static string GetFileSize(double size)
        {
            var round = 2;
            if (KBCount > size) return Math.Round(size, round) + "B";
            if (MBCount > size) return Math.Round(size / KBCount, round) + "KB";
            if (GBCount > size) return Math.Round(size / MBCount, round) + "MB";
            if (TBCount > size) return Math.Round(size / GBCount, round) + "GB";
            return Math.Round(size / TBCount, round) + "TB";
        }
        /// <summary>
        ///     得到适应的大小
        /// </summary>
        /// <param name="filepath">文件路径 </param>
        /// <param name="round">小数位 </param>
        /// <returns>string</returns>
        public static string GetFileSize(string filepath, int round)
        {
            if (File.Exists(filepath))
            {
                var file = new FileInfo(filepath);
                double size = file.Length;
                if (KBCount > size) return Math.Round(size, round) + "B";
                if (MBCount > size) return Math.Round(size / KBCount, round) + "KB";
                if (GBCount > size) return Math.Round(size / MBCount, round) + "MB";
                if (TBCount > size) return Math.Round(size / GBCount, round) + "GB";
                return Math.Round(size / TBCount, round) + "TB";
            }

            return string.Empty;
        }

        /// <summary>
        ///     自适应编码读取文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetTxt(string path)
        {
            var fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite);
            var buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            fileStream.Close();
            fileStream.Dispose();
            return GetTxt(buffer, GetEncode(buffer));
        }

        /// <summary>
        ///     取得文件编码方式
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Encoding GetEncode(byte[] buffer)
        {
            if (buffer.Length <= 0 || buffer[0] < 239)
                return Encoding.Default;
            if (buffer[0] == 239 && buffer[1] == 187 && buffer[2] == 191)
                return Encoding.UTF8;
            if (buffer[0] == 254 && buffer[1] == byte.MaxValue)
                return Encoding.BigEndianUnicode;
            if (buffer[0] == byte.MaxValue && buffer[1] == 254)
                return Encoding.Unicode;
            return Encoding.Default;
        }

        /// <summary>
        ///     按指定编码方式读取文本
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetTxt(byte[] buffer, Encoding encoding)
        {
            if (Equals(encoding, Encoding.UTF8))
                return encoding.GetString(buffer, 3, buffer.Length - 3);
            if (Equals(encoding, Encoding.BigEndianUnicode) || Equals(encoding, Encoding.Unicode))
                return encoding.GetString(buffer, 2, buffer.Length - 2);
            return encoding.GetString(buffer);
        }

        /// <summary>
        ///     读取文本（自适应编码方式）
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string GetTxt(byte[] buffer)
        {
            return GetTxt(buffer, GetEncode(buffer));
        }

        /// <summary>
        ///     写入文本
        /// </summary>
        /// <param name="filepath">写入文件</param>
        /// <param name="body">写入内容</param>
        /// <param name="encoding">编码方式</param>
        public static void WriteTxt(string filepath, string body, Encoding encoding)
        {
            if (File.Exists(filepath))
                try
                {
                    File.Delete(filepath);
                }
                catch (Exception ex)
                {
                }

            var bytes = encoding.GetBytes(body);
            var fileStream = File.Open(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);
            if (Equals(encoding, Encoding.UTF8))
            {
                fileStream.WriteByte(239);
                fileStream.WriteByte(187);
                fileStream.WriteByte(191);
            }
            else if (Equals(encoding, Encoding.BigEndianUnicode))
            {
                fileStream.WriteByte(254);
                fileStream.WriteByte(byte.MaxValue);
            }
            else if (Equals(encoding, Encoding.Unicode))
            {
                fileStream.WriteByte(byte.MaxValue);
                fileStream.WriteByte(254);
            }

            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();
        }
    }

    public class NativeMethods
    {
        /// Return Type: UINT->unsigned int
        /// lpszFile: LPCWSTR->WCHAR*
        /// nIconIndex: int
        /// phiconLarge: HICON*
        /// phiconSmall: HICON*
        /// nIcons: UINT->unsigned int
        [DllImport("shell32.dll", EntryPoint = "ExtractIconExW", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ExtractIconExW([In] [MarshalAs(UnmanagedType.LPWStr)] string lpszFile, int nIconIndex,
            ref IntPtr phiconLarge, ref IntPtr phiconSmall, uint nIcons);
    }
}
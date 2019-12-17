using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LjhTools.Utils
{
    public class TextHelper
    {

        /// <summary>
        ///  保存为Text文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void SaveToFile(string content, string filePath,bool appand)
        {
            FileStream fs;
            if (File.Exists(filePath) && appand)
            {
                fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            }
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(content);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        /// <summary>
        ///  保存为Text文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void SaveToFile(List<string> list, string path)
        {
            StringBuilder sbr = new StringBuilder();
            int i = 0;
            foreach (string line in list)
            {
                if (i > 0)
                {
                    sbr.Append("\r\n");
                }
                else
                {
                    i++;
                }
                sbr.Append(line);
            }

            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(sbr.ToString());
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        /// <summary>
        ///  保存为Text文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void AppendToFile(string content, string filePath)
        {
            FileStream fs;
            if (File.Exists(filePath))
            {
                fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            }
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(content);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        /// <summary>
        /// 保存为Text文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void SaveToFile(string content, string filePath)
        {
            SaveToFile(content, filePath, FileMode.Create);
        }
        /// <summary>
        /// 保存为Text文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void SaveToFile(string content, string filePath, FileMode mode)
        {
            SaveToFile(content, filePath, mode, Encoding.Default);
        }
        /// <summary>
        /// 保存为Text文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void SaveToFile(string content, string filePath, FileMode mode, Encoding encode)
        {
            FileStream fs = new FileStream(filePath, mode, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, encode);
            sw.Write(content);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        /// <summary>
        /// 读取Text文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetFileText(string filePath,Encoding encode)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs, Encoding.Default);
            string s = sw.ReadToEnd();
            sw.Close();
            fs.Close();
            return s;
        }
        /// <summary>
        /// 读取Text文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetFileTextList(string filePath, Encoding encode)
        {
            List<string> linelist = new List<string>();
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs, Encoding.Default);
            while(sw.Peek()>0)
            {
                linelist.Add(sw.ReadLine());
            }
            sw.Close();
            fs.Close();
            return linelist;
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileContent">文本内容</param>
        /// <param name="encode">编码格式</param>
        /// <returns></returns>
        public static string WriteFile(string filePath, string fileContent,Encoding encode)
        {
            string str;
            FileInfo info = new FileInfo(filePath);
            if (!Directory.Exists(info.DirectoryName))
            {
                Directory.CreateDirectory(info.DirectoryName);
            }
            FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream, encode);
            try
            {
                writer.Write(fileContent);
                str = fileContent;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
            finally
            {
                writer.Flush();
                stream.Flush();
                writer.Close();
                stream.Close();
            }
            return str;
        }
    }
}

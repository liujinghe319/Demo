using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LjhTools.Utils
{
    public class StringHelper
    {
        /// <summary>
        /// 截取字符串，半角按半个长度计算
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="len">截取长度，按全角算一个长度</param>
        /// <returns></returns>
        public static String SubString(String str, int len, string tail)
        {
            // 判断字符串是否为空
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            char[] charArray = str.ToCharArray();
            StringBuilder sbr = new StringBuilder();
            int i = 0;
            foreach (char c in charArray)
            {
                i += System.Text.Encoding.Default.GetByteCount(c.ToString());
                if (i > len * 2)
                {
                    sbr.Append(tail);
                    break;
                }
                sbr.Append(c);
            }
            return sbr.ToString();
        }

        // using System.Security.Cryptography;  
        public static string GetMd5Hash(String input)
        {
            if (input == null)
            {
                return null;
            }

            MD5 md5Hash = MD5.Create();

            // 将输入字符串转换为字节数组并计算哈希数据  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回十六进制字符串  
            return sBuilder.ToString();
        }
        // using System.Security.Cryptography;  
        public static string GetMd5Hash(List<long> bmnoList)
        {
            bmnoList.Sort();
            return GetMd5Hash(string.Join(",", bmnoList));
        }
        /// <summary>
        /// 分割同时去两端空格，空元素
        /// </summary>
        /// <param name="code"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<string> Split(char code, string text)
        {
            List<string> list = new List<string>();
            string[] attr = text.Split(code);
            foreach (string str in attr)
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    list.Add(str.Trim());
                }
            }
            return list;
        }
    }
}

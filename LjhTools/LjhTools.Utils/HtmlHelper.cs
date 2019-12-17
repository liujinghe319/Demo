using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace LjhTools.Utils
{
    public static class HtmlHelper
    {                
        /// <summary>
        /// 获得元素中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static string GetFirstInnerText(string html, string elem)
        {
            string inner = "";
            int st = 0;
            int ed = 0;
            int l = elem.Length;
            st = html.IndexOf(string.Format("<{0}", elem), st);
            st = html.IndexOf(">", st + l + 1);
            ed = html.IndexOf(string.Format("</{0}>", elem), st);
            inner = html.Substring(st + 1, ed - st - 1);
            return inner;
        }


        /// <summary>
        /// 根据属性获得值,取多个
        /// </summary>
        /// <param name="html">源文件</param>
        /// <param name="elem">元素名</param>
        /// <param name="n">第几个</param>
        /// <returns></returns>
        public static string GetInnerText(string html, string elem, int n)
        {
            int j = 0;
            int st = 0;
            int ed = 0;
            int l = elem.Length;
            string inner = "";
            while (true)
            {
                st = html.IndexOf(string.Format("<{0}", elem), st);
                if (st < 0) break;
                st = html.IndexOf(">", st + l + 1);
                ed = html.IndexOf(string.Format("</{0}>", elem), st);
                j++;              
                if (j == n)
                {
                    inner = html.Substring(st + 1, ed - st - 1);
                }
                st = ed + l + 1;
            }
            return inner;
        }

        /// <summary>
        /// 获得元素中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static List<string> GetInnerTextList(string html, string elem)
        {
            List<string> list = new List<string>();
            int st = 0;
            int ed = 0;
            int l = elem.Length;
            while (true)
            {
                st = html.IndexOf(string.Format("<{0}", elem), st);
                if (st < 0) break;
                st = html.IndexOf(">", st + l + 1);
                ed = html.IndexOf(string.Format("</{0}>", elem), st);
                string inner = html.Substring(st + 1, ed - st - 1);
                list.Add(inner);
                st = ed + l + 1;
            }
            return list;
        }
        /// <summary>
        /// 获得标签中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="key1">开始关键字</param>
        /// <param name="key2">结束关键字</param>
        /// <returns></returns>

        public static List<string> GetInnerTextList(string html, string key1, string key2)
        {
            List<string> list = new List<string>();
            int st = 0;
            int ed = 0;
            //int ll = key1.Length;
            while (true)
            {
                st = html.IndexOf(key1, st);
                if (st < 0) break;
                ed = html.IndexOf(key2, st);
                //string inner = html.Substring(st + ll, ed - st - ll);
                string inner = html.Substring(st, ed - st + key2.Length);
                list.Add(inner);
                //st = ed + ll;
                st = ed + key2.Length;
            }
            return list;
        }
        /// <summary>
        /// 获得标签中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static string GetInnerText(string html, string key1, string key2,ref int st)
        {
            string inner = "";
            int ed = 0;
            st = html.IndexOf(key1, st);
            if (st >= 0)
            {
                ed = html.IndexOf(key2, st);
                if (ed >= 0)
                {
                    inner = html.Substring(st, ed - st);
                    st = ed;
                }
                else
                {
                    inner = "没有找到结束标签";
                }
            }
            else
            {
                inner = "没有找到开始标签";
            }
            return inner;
        }
        /// <summary>
        /// 获得标签中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static string GetInnerText(string html, string key1,string key2)
        {
            int ed=0;
            return GetInnerText(html, key1, key2,ref ed);
        }
        /// <summary>
        /// 去除元素，保留中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static string RemoveElement(string html, string elem)
        {
            List<string> list = new List<string>();
            int st = 0;
            int ed = 0;
            int l = elem.Length;
            string result = html;
            while (true)
            {
                st = result.IndexOf(string.Format("<{0}", elem));
                if (st < 0)
                {
                    break;
                }
                ed = result.IndexOf(">", st + l + 1);
                result = result.Remove(st, ed - st + 1);
                result = result.Replace(string.Format("</{0}>", elem), "");
                st = ed + 1;
            }
            return result;
        }
        /// <summary>
        /// 根据ID获得Table
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetTableById(string html, string id)
        {
            int st = html.IndexOf(string.Format(" id=\"{0}\"", id), 0);
            if (st == -1) st = html.IndexOf(string.Format(" id={0}", id), 0);
            if (st < 0) return "";
            st = html.IndexOf(">", st);
            int ed = html.IndexOf("</table>", st);
            if (ed == -1) ed = html.IndexOf("</TABLE>", st);
            return html.Substring(st + 1, ed - st);
        }
        /// <summary>
        /// 根据ID获得Table
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetTableById(string html, string id, ref int start)
        {
            int st = html.IndexOf(string.Format(" id=\"{0}\"", id), start);
            if (st == -1) st = html.IndexOf(string.Format(" id={0}", id), start);
            if (st < 0) return "";
            int st0 = html.IndexOf("<TBODY>", st);
            if (st0 == -1)
            {
                st = html.IndexOf("<tbody>", st);
            }
            else
            {
                st = st0;
            }
            int ed = html.IndexOf("</TBODY>", st);
            if (ed == -1) ed = html.IndexOf("</TBODY>", st);
            start = ed + 8;
            return html.Substring(st + 7, ed - st - 7);
        }

        /// <summary>
        /// 根据属性获得值，只取第一个
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetFirstPropertyValue(string html, string property)
        {
            int st = html.IndexOf(property, 0);
            if (st < 0) return "";
            st = html.IndexOf("=", st + property.Length);
            int ed = html.IndexOf(">", st + 1);
            int ed1 = html.IndexOf("\"", st + 2);
            if(ed1<0)
            {
                ed1 = html.IndexOf(" ", st + 1);
            }
            if (ed1 < ed) ed = ed1;
            return html.Substring(st + 1, ed - st - 1).Replace("\"", "");
        }

        /// <summary>
        /// 根据属性获得值,取多个
        /// </summary>
        /// <param name="html"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static List<string> GetPropertyValue(string html, string property)
        {
            List<string> list = new List<string>();
            int st = 0;
            while (true)
            {
                int s = st;
                st = html.IndexOf(string.Format(" {0}=",property), st);
                if (st < 0)
                {
                    st = html.IndexOf(string.Format(" {0} =", property), s);
                    if(st<0) break;
                }
                st = html.IndexOf("\"", st + property.Length);
                int ed = html.IndexOf("\"", st + 1);
                list.Add(html.Substring(st + 1, ed - st - 1));
                st = ed + 1;
            }
            return list;
        }
    }
  
}

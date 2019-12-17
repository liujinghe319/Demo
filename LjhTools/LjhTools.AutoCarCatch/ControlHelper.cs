using LjhTools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LjhTools.AutoCarCatch
{
    public enum FileType
    {
        Text,
        Excel,
        Html
    }
    public static class ControlHelper
    {
        /// <summary>
        ///  选择保存路径和文件名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSavePath(FileType type)
        {
            SaveFileDialog fbd = new SaveFileDialog();
            fbd.Title = "请选择保存文件的文件夹！";
            if (type == FileType.Text)
            {
                fbd.Filter = "Text Document(*.txt)|*.txt";
            }
            else if(type == FileType.Excel)
            {
                fbd.Filter = "Excel (*.xls;*.xlsx)|*.xls;*.xlsx";
            }
            else if (type == FileType.Html)
            {
                fbd.Filter = "Html Document(*.html;*.htm)|*.htm;*.html";
            }
            fbd.FileName = "文本";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return fbd.FileName;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///  选择文件夹路径
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetFolderPath()
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return fbd.SelectedPath;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        ///  选择文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SelectFile(FileType type,string title)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            fbd.Title = title;
            if (type == FileType.Text)
            {
                fbd.Filter = "Text Document(*.txt)|*.txt";
            }
            else if (type == FileType.Excel)
            {
                fbd.Filter = "Excel (*.xls;*.xlsx)|*.xls;*.xlsx";
            }
            else if (type == FileType.Html)
            {
                fbd.Filter = "Html Document(*.html;*.htm)|*.htm;*.html";
            }

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return fbd.FileName;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        ///  选择文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SelectFile(FileType type)
        {
            return SelectFile(type, "选择文件");
        }
        /// <summary>
        ///  选择文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SelectFile()
        {
            OpenFileDialog fbd = new OpenFileDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return fbd.FileName;
            }
            else
            {
                return "";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Threading;
using System.Data.OleDb;
using LjhTools.Utils;

namespace LjhTools.TextShow
{
    public partial class Form1 : Form
    {
        private string _content1 = "";
        private string _content2 = "";
        private string _content3 = "";
        private string _content4 = "";
        private string _fileName = "";
        private string _filePath = "D:\\";
        private string _folderPath = "D:\\";
        private string _savePath = "D:\\";
        private List<string> _fileList = new List<string>();
        private List<string> _filePathList = new List<string>();
        private string[] _txtArray;
        BackgroundWorker _work;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                txtFile.Text = path;
                _fileName = path.Substring(path.LastIndexOf("\\")+1).Replace(".txt", "");
                StreamReader reader = new StreamReader(path, Encoding.Default);
                _content1 = reader.ReadToEnd();
                reader.Close();
                _content2 = _content1.Replace("\t", "\\t").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\\r\\n", "\\r\\n\r\n");
                _content3 = _content1.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\\r\\n", "\\r\\n\r\n"); ;
                _content4 = _content1.Replace("\t", "\\t").Replace("\r\n", "\\r\\n").Replace("\n", "\\n").Replace("\\r\\n","\r\n"); 

                if (cbxTable.Checked && cbxWrap.Checked)
                {
                    txtContent.Text = _content2;
                }
                else if (!cbxTable.Checked && !cbxWrap.Checked)
                {
                    txtContent.Text = _content1;
                }
                else if (cbxTable.Checked && !cbxWrap.Checked)
                {
                    txtContent.Text = _content4;
                }
                else
                {
                    txtContent.Text = _content3;
                }
            }
        }

        private void btnWrap_Click(object sender, EventArgs e)
        {

        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxTable.Checked && cbxWrap.Checked)
            {
                txtContent.Text = _content2;
            }
            else if (!cbxTable.Checked && !cbxWrap.Checked)
            {
                txtContent.Text = _content1;
            }
            else if (cbxTable.Checked && !cbxWrap.Checked)
            {
                txtContent.Text = _content4;
            }
            else
            {
                txtContent.Text = _content3;
            }
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtContent.Text.Trim()))
            {
                MessageBox.Show("内容不能为空！");
                return;
            }
            string num = txtLine.Text.Trim();
            int line = 0;
            if (string.IsNullOrEmpty(num))
            {
                MessageBox.Show("行数不能为空！");
            }
            else if (!int.TryParse(num, out line))
            {
                MessageBox.Show("行数必须为整数！");
            }
            else
            {
                if (line <= 0)
                {
                    MessageBox.Show("行数必须大于0！");
                }
                else
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "请选择保存拆分文件的文件夹！";
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        _filePath = fbd.SelectedPath;
                    }
                    else
                    {
                        return;
                    }
                    BackgroundWorker work = new BackgroundWorker();
                    int count = 0;
                    List<string> lineList = new List<string>();
                    work.DoWork += (s, args) =>
                    {
                        StreamReader reader = new StreamReader(txtFile.Text, Encoding.Default);
                        while (reader.Peek() > -1)
                        {
                            string str = reader.ReadLine();
                            lineList.Add(str);
                        }
                        reader.Close();
                        if (cbxRepeat.Checked)
                        {
                            lineList = lineList.Distinct().ToList();
                            lineList.RemoveAll(IsNullOrEmpty);
                        }
                        string content = "";
                        int i = 1;
                        foreach (string str in lineList)
                        {
                            if (content != "")
                            {
                                content += "\r\n";
                            }
                            content += str;
                            if (i % line == 0 || i == lineList.Count)
                            {
                                count = i / line;
                                if (i % line > 0)
                                {
                                    count++;
                                }
                                string path = Path.Combine(_filePath,(_fileName+count+".txt"));
                                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                                sw.Write(content);
                                sw.Flush();
                                sw.Close();
                                fs.Close();
                                content = "";
                            }
                            i++;
                        }

                    };
                    work.RunWorkerCompleted += (s, args) =>
                    {
                        string message = "拆分完成,共拆分为" + count + "个文件！";
                        if (cbxRepeat.Checked)
                        {
                            message += "不重复数据" + lineList.Count + "行!";
                        }
                        MessageBox.Show(message);
                    };
                    work.RunWorkerAsync();
                }
            }
        }
        private bool IsNullOrEmpty(string s)
        {
            if (s.Trim() == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private int GetStrDisplayCount(string str, string search)
        {
            int count = 0; //计数器 
            for (int i = 0; i < str.Length - search.Length; i++)
            {
                if (str.Substring(i, search.Length) == search)
                {
                    count++;
                }
            }
            return count;
        }

        private void cbxRepeat_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (Directory.Exists(txtPath.Text))
            {
                folderDialog.SelectedPath = txtPath.Text;
            }
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                _folderPath = folderDialog.SelectedPath;
                if (radExcel.Checked) AnalysisFolder(".xls|.xlsx");
                if (radTxt.Checked) AnalysisFolder(".txt");
            }
        }

        private void AnalysisFolder(string filetype)
        {
            _fileList.Clear();
            _filePathList.Clear();
            txtPath.Text = _folderPath;
            txtNum.Text = "0";
            txtProcess.Text = "0";
            if (Directory.Exists(_folderPath)) //如果存在这个文件夹，执行删除操作
            {
                _filePathList = Directory.GetFiles(_folderPath).ToList<string>();
                foreach (string path in _filePathList)
                {
                    foreach(string file in filetype.Split('|'))
                    {
                        if (file!="" && path.EndsWith(file))
                        {
                            int start = path.LastIndexOf('\\') + 1;
                            _fileList.Add(path.Substring(start));
                        }
                    }
                }
                lbxDataFile.DataSource = null;
                lbxDataFile.DataSource = _fileList;
                txtNum.Text = _fileList.Count.ToString();
            }
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            if (txtPath.Text.Trim() == "")
            {
                MessageBox.Show("请选择数据文件夹！");
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (radExcel.Checked)
                {
                    sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    sfd.DefaultExt = ".xlsx";
                }
                else
                {
                    sfd.Filter = "Text Files (*.txt)|*.txt";
                    sfd.DefaultExt = ".txt";
                }
                sfd.CheckPathExists = true;
                sfd.CheckFileExists = false;
                sfd.Title = "请选择文件保存路径";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    _savePath = sfd.FileName;
                    if (radExcel.Checked)
                    {
                        MergeExcel();
                    }
                    else
                    {
                        _work = new BackgroundWorker();
                        _work.WorkerReportsProgress = true;
                        _work.DoWork += _work_DoWork;
                        _work.RunWorkerCompleted += work_RunWorkerCompleted;
                        _work.ProgressChanged += work_ProgressChanged;
                        _work.RunWorkerAsync();
                    }
                }
                else
                {
                    return;
                }
            }
        }
        void work_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            txtProcess.Text = e.ProgressPercentage.ToString();
        }

        void work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("数据合并完成！");
        }

        void _work_DoWork(object sender, DoWorkEventArgs e)
        {
            if(radTxt.Checked)
            {
                MergeText();
            }
        }

        private void MergeExcel()
        {
            Thread thread = new Thread(() =>
            {
                int i = 0;
                List<DataTable> dtList = new List<DataTable>();
                List<string> columnlist = new List<string>();
                foreach (string path in _filePathList)
                {
                    i++;
                    if (File.Exists(path) && (path.EndsWith(".xls", StringComparison.CurrentCultureIgnoreCase) || path.EndsWith(".xlsx", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        DataTable ds = ExcelHelper.ExcelToTable(path);
                        dtList.Add(ds);
                        foreach (var clum in ds.Columns)
                        {
                            string cname = clum.ToString();
                            if (!columnlist.Contains(cname))
                            {
                                columnlist.Add(cname);
                            }
                        }
                    }
                    lblTip.Text = string.Format("正在读取第{0}个文件", i);
                }
                i = 0;
                lblTip.Text = "";
                DataTable table = new DataTable();
                foreach (string clumn in columnlist)
                {
                    table.Columns.Add(new DataColumn(clumn, typeof(string)));
                }
                foreach (DataTable dt in dtList)
                {
                    i++;
                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow newrow = table.NewRow();
                        foreach (DataColumn colum in dt.Columns)
                        {
                            string name = colum.ColumnName;
                            newrow[name] = row[name];
                        }
                        table.Rows.Add(newrow);
                    }
                    txtProcess.Text = i.ToString();
                }
                ExcelHelper.TableToExcel(table, _savePath);
                MessageBox.Show("数据合并完成！");
            });
            thread.Start();
        }
        private void MergeText()
        {
            int num = 0;
            string content = "";
            int i = 0;
            foreach (string path in _filePathList)
            {
                if (File.Exists(path) && path.EndsWith(".txt",StringComparison.CurrentCultureIgnoreCase))
                {
                    StreamReader reader = new StreamReader(path, Encoding.Default);
                    string stream = "";
                    if (cbxFirst.Checked)
                    {
                        if (i == 0)
                        {
                            reader.ReadLine();
                        }
                        stream = reader.ReadToEnd();
                    }
                    else
                    {
                        stream = reader.ReadToEnd();
                    }
                    reader.Close();
                    content += stream;
                    num++;
                    _work.ReportProgress(num);
                }
            }
            FileStream fs = new FileStream(_savePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            if (cbxRemove1.Checked)
            {
                content = content.Replace("\r\n", "%~").Replace("\n", "").Replace("%~", "\r\n");
            }
            sw.Write(content);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        private void btnSelect3_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string file = ofd.FileName;
                txtFile3.Text = file;
                string path = file.Substring(0, file.LastIndexOf("\\"));
                StreamReader reader = new StreamReader(file, Encoding.Default);
                string content = reader.ReadToEnd();
                reader.Close();
                _txtArray = content.Replace("\r\n", "~").Split('~');
                txtContent3.Text = content;
                //foreach (string str in _txtArray)
                //{
                //    if (str.Trim() != "")
                //    {
                //        if(File.Exists(str))
                //        {
                //            _filePath3List.Add(str);
                //        }
                //        else
                //        {
                //            MessageBox.Show("文件不存在：" + str);
                //        }
                //    }
                //}
                txtNum3.Text = _txtArray.Length.ToString();
            }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            //_fileList.Clear();
            //_filePathList.Clear();
            //ResetText();
        }

        private void btnCopy3_Click(object sender, EventArgs e)
        {
            FileOperate(0);
        }
        private void FileOperate(int type)
        {
            if(string.IsNullOrWhiteSpace(txtFileSource.Text))
            {
                MessageBox.Show("请选择文件搜索路径！");
                return;
            }
            if(type!=2)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择复制或移动文件的目的文件夹！";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    _filePath = fbd.SelectedPath;
                }
                else
                {
                    return;
                }
            }
            Thread thread = new Thread(() =>
            {
                _txtArray = _txtArray.Distinct().ToArray<string>();
                int total = _txtArray.Count();
                int process = 0;
                int success = 0;
                txtNum3.Text = total.ToString();
                Dictionary<string, List<string>> pathDic = new Dictionary<string, List<string>>();
                FindPathByFileName(txtFileSource.Text,pathDic);
                foreach (string fileName in _txtArray)
                {
                    process++;
                    string file = fileName;
                    string foldername = "";
                    if(fileName.LastIndexOf("\\")>=0)
                    {
                        file = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        if(type==3)
                        {
                            string[] folders = fileName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                            foldername = folders[folders.Length - 2];
                        }
                    }
                    else if (fileName.LastIndexOf("/") >= 0)
                    {
                        file = fileName.Substring(fileName.LastIndexOf("/") + 1);
                        if (type == 3)
                        {
                            string[] folders = fileName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                            foldername = folders[folders.Length - 2];
                        }
                    }
                    if(file!="")
                    {
                        if(pathDic.ContainsKey(file))
                        {
                            int i = 0;
                            foreach(string findpath in pathDic[file])
                            {
                                i++;
                                string newname = file;
                                if(i>1)
                                {
                                    newname=file.Insert(file.LastIndexOf("."), string.Format("_{0}", i));
                                }
                                if (type == 0)//复制
                                {
                                    File.Copy(findpath, Path.Combine(_filePath, newname),true);
                                }
                                if (type == 1)//移动
                                {
                                    File.Move(findpath, Path.Combine(_filePath, newname));
                                }
                                if (type == 2)//删除
                                {
                                    File.Delete(findpath);
                                }
                                if (type == 3)//分文件夹
                                {
                                    string path = Path.Combine(_filePath, foldername);
                                    if(!Directory.Exists(path))
                                    {
                                        Directory.CreateDirectory(path);
                                    }
                                    File.Move(findpath, Path.Combine(path, newname));
                                }
                            }
                            success++;

                        }
                    }
                    txtProcess3.Text = process.ToString();
                }
                MessageBox.Show(string.Format("文件操作完成，共处理{0}个文件！",success));
            });
            thread.Start();
        }

        private void btnSelect4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                txtFileSource.Text = folderDialog.SelectedPath;
            }
        }

        private void btnDelete3_Click(object sender, EventArgs e)
        {
            FileOperate(2);
        }

        private void btnMove3_Click(object sender, EventArgs e)
        {
            FileOperate(1);
        }
        /// <summary>
        /// 搜索文件
        /// </summary>
        /// <param name="path">搜索路径</param>
        /// <param name="findfile">文件名称</param>
        /// <returns></returns>
        private void FindPathByFileName(string path,Dictionary<string,List<string>> fileDic)
        {
            if(Directory.Exists(path))
            {
                string[] filearray=Directory.GetFiles(path);
                string[] patharray = Directory.GetDirectories(path);
                foreach(string file in filearray)
                {
                    string filename = file.Substring(file.LastIndexOf('\\')+1);
                    if (fileDic.ContainsKey(filename))
                    {
                        fileDic[filename].Add(file);
                    }
                    else
                    {
                        fileDic.Add(filename, new List<string> { file });
                    }
                }
                foreach(string pth in patharray)
                {
                    FindPathByFileName(pth, fileDic);
                }
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtContent3.Text))
            {
                MessageBox.Show("请导入改名文件！");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtFileSource.Text))
            {
                MessageBox.Show("请选择文件夹路径！");
                return;
            }
            Thread thread = new Thread(() =>
            {
                _txtArray = _txtArray.Distinct().ToArray<string>();
                int total = _txtArray.Count();
                txtNum3.Text = total.ToString();
                int process = 0;
                int success = 0;;
                foreach (string fileName in _txtArray)
                {
                    process++;
                    if (fileName != "")
                    {
                        string[] arr = fileName.Split('	');
                        string oldname = arr[0];
                        string newname = arr[1];
                        string path = Path.Combine(txtFileSource.Text, oldname);
                        if (oldname != newname && File.Exists(path))
                        {
                            File.Move(path, Path.Combine(txtFileSource.Text, newname));
                            success++;
                        }
                    }
                    txtProcess3.Text = process.ToString();
                }
                MessageBox.Show(string.Format("文件操作完成，共处理{0}个文件！", success));
            });
            thread.Start();
        }

        private void radExcel_CheckedChanged(object sender, EventArgs e)
        {
            cbxRemove1.Enabled = false;
            cbxFirst.Enabled = false;
        }

        private void radTxt_CheckedChanged(object sender, EventArgs e)
        {
            cbxRemove1.Enabled = true;
            cbxFirst.Enabled = true;
        }

        private void btnCopyPath_Click(object sender, EventArgs e)
        {
            string path = txtFileSource.Text;
            if(path.Trim()=="")
            {
                MessageBox.Show("请选择搜索目录");
            }
            else
            {
                Thread thread = new Thread(() =>
                {
                    List<string> pathList=new List<string>();
                    AnalysisFolder(path, pathList);
                    if(pathList.Count>0)
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Text Files (*.txt)|*.txt";
                        sfd.DefaultExt = ".txt";
                        sfd.CheckPathExists = true;
                        sfd.CheckFileExists = false;
                        sfd.Title = "请选择文件保存路径";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            string fileName = sfd.FileName;
                            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                            sw.Write(string.Join("\r\n", pathList));
                            sw.Flush();
                            sw.Close();
                            fs.Close();
                            System.Diagnostics.Process.Start("notepad.exe", fileName);
                        }
                    }
                    else
                    {
                        MessageBox.Show("该目录没有文件！");
                    }
                });
                thread.ApartmentState = ApartmentState.STA;
                thread.Start();
            }
        }
        private void AnalysisFolder(string filepath,List<string> pathList)
        {
            string[] directories = Directory.GetDirectories(filepath);
            foreach (string directory in directories)
            {
                AnalysisFolder(directory,pathList);
            }

            DirectoryInfo directoryinfo = new DirectoryInfo(filepath);

            FileInfo[] files = directoryinfo.GetFiles().OrderBy(ff => ff.CreationTime).ToArray();
            foreach (FileInfo file in files)
            {
                pathList.Add(file.FullName);
            }
        }



        private void FileNameTransform(string filepath,bool tolower,ref int count)
        {
            int num = count;
            string[] directories = Directory.GetDirectories(filepath);            
            foreach (string directory in directories)
            {
                FileNameTransform(directory, tolower,ref count);
            }
            string[] files = Directory.GetFiles(filepath);
            foreach (string file in files)
            {
                num++;
                int n= file.LastIndexOf('\\');
                string filename = file.Substring(n);
                string newfilename = tolower ? filename.ToLower() : filename.ToUpper();
                if(filename!=newfilename)
                {
                    //FileInfo fi = new FileInfo(file);
                    File.Move(file,file.Substring(0,n+1)+newfilename);
                    count++;
                }
                txtNum3.Text = count.ToString();
            }
        }

        private void btnToLower_Click(object sender, EventArgs e)
        {
            string path = txtFileSource.Text;
            if(path.Trim()=="")
            {
                MessageBox.Show("请选择搜索目录");
            }
            else{
                CheckForIllegalCrossThreadCalls = false;
                Thread thread = new Thread(() =>
                {
                    int n = 0;
                    FileNameTransform(path, true, ref n);
                    MessageBox.Show(string.Format("完成,共更改{0}个文件名", n));
                });
                thread.Start();
            }
        }

        private void btnToUpper_Click(object sender, EventArgs e)
        {
            string path = txtFileSource.Text;
            if (path.Trim() == "")
            {
                MessageBox.Show("请选择搜索目录");
            }
            else
            {
                Thread thread = new Thread(() =>
                {
                    CheckForIllegalCrossThreadCalls = false;
                    int n=0;
                    FileNameTransform(path, false,ref n);
                    MessageBox.Show(string.Format("完成,共更改{0}个文件名",n));
                });
                thread.Start();
            }
        }

        private void btmSearch_Click(object sender, EventArgs e)
        {
            List<string> keyList = txtContent.Lines.Where(ll => !string.IsNullOrWhiteSpace(ll)).ToList();
            if(keyList.Count==0)
            {
                MessageBox.Show("请输入搜索的关键词！");
                return;
            }
            List<string> resultlist = new List<string>();
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string path = folderDialog.SelectedPath;

                Thread thread = new Thread(() =>
                {
                    List<string> files = new List<string>();
                    AnalysisFolder(path, files);
                    int total = files.Count();
                    int i = 0;
                    foreach(string file in files)
                    {
                        i++;
                        int n = 0;
                        StreamReader reader = new StreamReader(file, Encoding.Default);
                        while(reader.Peek()>0)
                        {
                            n++;
                            string line=reader.ReadLine();
                            foreach(string key in keyList)
                            {
                                if(line.IndexOf(key)>=0)
                                {
                                    resultlist.Add(key + "	" + file);
                                    continue;
                                }
                            }
                        }
                        reader.Close();
                        lblTip1.Text = string.Format("进度：{0}/{1}", total, i);
                    }
                    if(resultlist.Count>0)
                    {
                        SaveFileDialog sfd=new SaveFileDialog();
                        sfd.Filter = "txt files(*.txt)|*.txt";
                        if(sfd.ShowDialog()==DialogResult.OK)
                        {
                            string filepath=sfd.FileName;

                            FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                            string content = string.Join("\r\n",resultlist.Distinct());
                            sw.Write(content);
                            sw.Flush();
                            sw.Close();
                            fs.Close();
                            System.Diagnostics.Process.Start("notepad.exe", filepath);
                        }
                    }
                    else{
                        MessageBox.Show("没有找到包含这些关键字的文件！");
                    }
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtContent.Text.Trim()))
            {
                MessageBox.Show("内容不能为空！");
                return;            
            }
            Thread thread = new Thread(() =>
            {
                Dictionary<string, List<string>> lineDictionary = new Dictionary<string, List<string>>();
                int i = 0;
                bool error = false;
                foreach (string str in txtContent.Lines)
                {
                    i++;
                    string line = str.Replace("\"", "").Trim();
                    if (line!= "")
                    {
                        string[] args = line.Trim().Split(new char[]{'	'},StringSplitOptions.RemoveEmptyEntries);
                        if(args.Length<2)
                        {
                            MessageBox.Show(string.Format("第{0}行有错误！", i));
                            error = true;
                            break;
                        }
                        if(lineDictionary.ContainsKey(args[0]))
                        {
                            lineDictionary[args[0]].AddRange(args[1].Split(','));
                        }
                        else
                        {
                            lineDictionary.Add(args[0],args[1].Split(',').ToList());
                        }
                    }
                }
                if (!error)
                {
                    StringBuilder sbr = new StringBuilder();
                    foreach (var obj in lineDictionary)
                    {
                        sbr.Append(obj.Key);
                        sbr.Append("	");
                        sbr.AppendLine(string.Join(",", obj.Value.Distinct()));
                    }
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "txt files(*.txt)|*.txt";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string filepath = sfd.FileName;

                        FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                        string content = string.Join("\r\n", sbr.ToString());
                        sw.Write(content);
                        sw.Flush();
                        sw.Close();
                        fs.Close();
                        System.Diagnostics.Process.Start("notepad.exe", filepath);
                    }
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void btnFindFile_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string path = ofd.FileName;
                    txtFile.Text = path;
                    List<string> pathList = new List<string>();
                    _fileName = path.Substring(path.LastIndexOf("\\") + 1).Replace(".txt", "");
                    StreamReader reader = new StreamReader(path, Encoding.Default);
                    while(reader.Peek()>0)
                    {
                        string line = reader.ReadLine();
                        if(line.Trim()!="")
                        {
                            pathList.Add(line);
                        }
                    }
                    reader.Close();
                    StringBuilder sbr = new StringBuilder();
                    int total = pathList.Count();
                    txtNum3.Text = total.ToString();
                    int process = 0;
                    int error = 0;
                    foreach (string file in pathList)
                    {
                        process++;
                        try
                        {
                           if(!File.Exists(file))
                           {
                               sbr.AppendLine(file);
                               error++;
                           }
                        }
                        catch
                        {
                            error++;
                            sbr.AppendLine(file);
                        }
                        txtProcess3.Text = process.ToString();
                        //lblTip.Text = string.Format("总数：{0}，进度：{1}", total, process);
                    }
                    if(sbr.Length>0)
                    {
                        txtContent3.Text ="未找到"+error+"个文件\r\n"+ sbr.ToString();
                    }
                    else
                    {
                        txtContent3.Text = "所有文件都存在！";
                    }
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            FileOperate(3);
        }
    }
}

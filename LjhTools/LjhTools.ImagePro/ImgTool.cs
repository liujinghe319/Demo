using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using XS.Core.FSO;
//using XS.Core.Strings;
using System.Text.RegularExpressions;
using LjhTools.Utils;

namespace LjhTools.ImagePro
{
    public partial class ImgTool : Form
    {
        private string _path = "";
        private string _watermarkpath; //水印图片
        private List<string> _fileList = new List<string>();//文件名列表
        private List<string> _filePathList = new List<string>();//文件路径列表
        private List<int[]> _sizes; //生成图片尺寸
        private string _tail; //生成图片后缀
        public ImgTool()
        {
            InitializeComponent();
        }

        private void ImgTool_Load(object sender, EventArgs e)
        {
            this.cboImgFormat.SelectedIndex = 0;
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                _path = folderDialog.SelectedPath;
                AnalysisFolder();
            }
        }
        private void AnalysisFolder()
        {
            _fileList.Clear();
            _filePathList.Clear();
            txtPath.Text = _path;
            if (Directory.Exists(_path)) //如果存在这个文件夹，执行删除操作
            {
                _filePathList = Directory.GetFileSystemEntries(_path).ToList<string>();
                foreach (string path in _filePathList)
                {
                    int start = path.LastIndexOf('\\') + 1;
                    _fileList.Add(path.Substring(start));
                }
                lbxDataFile.DataSource = null;
                lbxDataFile.DataSource = _fileList;
                lblCount.Text = "数量："+_fileList.Count;
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (txtPath.Text == "")
            {
                MessageBox.Show("请选择要处理的图片路径！");
                return;
            }
            if (txtSize.Text.Trim() == "" && !cbkMark.Checked)
            {
                MessageBox.Show("请填写新生成图片尺寸，格式：800*600|300*400");
                return;
            }
            _tail = txtTail.Text.Trim();
            _sizes = new List<int[]>();
            if (txtSize.Text.Trim()!="")
            {
                foreach (string size in txtSize.Text.Split('|'))
                {
                    _sizes.Add(size.Split('*').Select(ss => Convert.ToInt32(ss)).ToArray());
                }
            }
            CheckForIllegalCrossThreadCalls = false;

            Thread tt = new Thread(() =>
            {
                int n = 0;
                btnStart.Enabled = false;
                if (MessageBox.Show("是否更改文件保存路径？", "文件路径", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string savepath = folderDialog.SelectedPath;
                        AnalysisFolder(txtPath.Text,savepath, ref n);
                    }
                }
                else
                {
                    AnalysisFolder(txtPath.Text, ref n);
                }
                MessageBox.Show(this,"生成成功！");
                btnStart.Enabled = true;
            });
            tt.SetApartmentState(ApartmentState.STA);
            tt.Start();
        }
        
        private void AnalysisFolder(string filepath,ref int n)
        {
             AnalysisFolder(filepath, filepath, ref n);
        }
        private void AnalysisFolder(string filepath, string savepath, ref int n)
        {
            string[] directories = Directory.GetDirectories(filepath);
            foreach (string directory in directories)
            {
                string newPath = savepath + directory.Replace(filepath, "");
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                AnalysisFolder(directory,savepath,ref n);
            }
            string[] files = Directory.GetFiles(filepath);
            foreach (string file in files)
            {
                string tail = file.Substring(file.LastIndexOf(".")).ToLower();
                if (tail == ".jpg" || tail == ".png" || tail == ".jpeg" || tail == ".gif")
                {
                    n++;
                    lblTip.Text = n.ToString();
                    CreateImage(file, savepath);
                }
            }
        }
        private void AnalysisFolder(string filepath, string savepath,string format, ref int n)
        {
            string[] directories = Directory.GetDirectories(filepath);
            foreach (string directory in directories)
            {
                string newPath = savepath + directory.Replace(filepath, "");
                if(!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                AnalysisFolder(directory, newPath, format, ref n);
            }
            string[] files = Directory.GetFiles(filepath);
            foreach (string file in files)
            {
                string tail = file.Substring(file.LastIndexOf(".")).ToLower();
                if (tail == ".jpg" || tail == ".png" || tail == ".jpeg" || tail == ".gif")
                {
                    n++;
                    lblTip.Text = n.ToString();
                    SaveImageTo(file, savepath,format);
                }
            }
        }
        private void CopyImage(string sourcePath, string copyTo, int w, int h, double scale, ref int n)
        {
            string[] directories = Directory.GetDirectories(sourcePath);
            foreach (string directory in directories)
            {
                CopyImage(directory, copyTo,w,h,scale,ref n);
            }
            string[] files = Directory.GetFiles(sourcePath);
            foreach (string file in files)
            {
                string tail = file.Substring(file.LastIndexOf(".")).ToLower();
                if (tail == ".jpg" || tail == ".png" || tail == ".jpeg" || tail == ".gif")
                {
                    n++;
                    lblTip.Text = n.ToString();
                    Bitmap img = new Bitmap(file);
                    double height = img.Height;
                    double width = img.Width;
                    if ((width < w && height < h) && ((width / height < scale) || (height/width < scale)))
                    {
                        string newpath = copyTo + "\\不合格\\" + file.Substring(file.LastIndexOf("\\"));
                        File.Copy(file, newpath, true);
                    }
                    else
                    {
                        string newpath=copyTo+ "\\合格\\"+file.Substring(file.LastIndexOf("\\"));
                        File.Copy(file, newpath, true);
                    }
                }
            }
        }


        private void SaveImageTo(string sourceimg, string savepath,string format)
        {
            string srcimg = sourceimg;
            int n1 = sourceimg.LastIndexOf('.');
            string[] arr = sourceimg.Split('\\');
            string newpath = savepath.EndsWith("\\") ? savepath + arr.Last() : savepath + "\\" + arr.Last();
            int n2 = newpath.LastIndexOf('.');
            string newimg = newpath.Substring(0,n2+1);
            if(sourceimg.EndsWith("JPG")&&format=="jpg")
            {
                newimg += "JPG";
            }
            else if(sourceimg.EndsWith("PNG") && format == "png")
            {
                newimg += "PNG";
            }
            else
            {
                newimg += format;
            }
            ImageHelper.GenThumbnail(srcimg, newimg, Convert.ToInt32(txtQuality.Text));
        }
        private void CreateImage(string sourceimg,string savepath)
        {
            bool noZoom = cbxNoZoom.Checked;
            string srcimg = sourceimg;
            int n1 = sourceimg.LastIndexOf('.');
            string[] arr=sourceimg.Split('\\');
            string newpath = savepath.EndsWith("\\") ? savepath + arr.Last() : savepath +"\\"+ arr.Last();
            int n2 = newpath.LastIndexOf('.');


            if (cbkMark.Checked)
            {
                Image img = Image.FromFile(sourceimg);
                Image watermark = Image.FromFile(_watermarkpath);
                int positon = Convert.ToInt32(txtPosition.Text);
                int quality = int.Parse(txtQuality.Text);
                int trans = int.Parse(txtTrans.Text);

                ImageHelper.AddImageSignPic(img, newpath, _watermarkpath, positon, quality, trans, _sizes, cboAddSize.Checked, _tail, cboAddIndex.Checked, noZoom);
            }
            else
            {
                int i = 1;
                foreach (int[] size in _sizes)
                {
                    string newimg = "";
                    StringBuilder tail = new StringBuilder();
                    if (cboAddSize.Checked)
                    {
                        tail.AppendFormat("_{0}x{1}", size[0], size[1]);
                    }
                    tail.AppendFormat(_tail);
                    if (cboAddIndex.Checked)
                    {
                        tail.Append(i);
                    }
                    newimg = newpath.Insert(n2, tail.ToString());
                    ImageHelper.GenThumbnail(srcimg, newimg, size[0], size[1], noZoom);
                    i++;
                }
            }
        }


        private void RemoveImageTitle(string sourceimg,string savePath)
        {
            Bitmap m_Bmp = new Bitmap(800, 800);
            Graphics m_Graphics = Graphics.FromImage(m_Bmp);
            Bitmap m_Img = new Bitmap(sourceimg);
            GraphicsUnit m_Units = GraphicsUnit.Pixel;
            //*******保持质量的函数***************
            ImageAttributes m_ImgAtt = new ImageAttributes();
            m_ImgAtt.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
            //**********************
            m_Graphics.DrawImage(m_Img, new Rectangle(0,0,800,720), 0, 0, 800, 720, m_Units, m_ImgAtt);
            m_Graphics.Dispose();
            //m_Bmp.MakeTransparent(System.Drawing.Color.Transparent);
            m_Bmp.Save(savePath+sourceimg.Substring(sourceimg.LastIndexOf("\\")), m_Img.RawFormat);

        }

        private void cbkMark_CheckedChanged(object sender, EventArgs e)
        {
            if (cbkMark.Checked)
            {
                groupBox1.Enabled = true;
                OpenFileDialog filedialog = new OpenFileDialog();
                filedialog.Filter = "图片(*.jpg;*.png;*.gif;*jpeg)|*.jpg;*.png;*.gif;*jpeg";
                if (filedialog.ShowDialog() == DialogResult.OK)
                {
                    _watermarkpath = filedialog.FileName;
                }
                else
                {
                    cbkMark.Checked = false;
                }
            }
            else
            {
                groupBox1.Enabled = false;
                _path = null;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            string error = "";
            if (Regex.IsMatch(txtHeight.Text, "^[^0-9]+$") || Regex.IsMatch(txtWidth.Text, "^[^0-9]+$"))
            {
                error = "长宽,";
            }
            if (Regex.IsMatch(txtSize1.Text, "^[^0-9]+$") || Regex.IsMatch(txtSize2.Text, "^[^0-9]+$"))
            {
                error = "比例";
            }
            if(error!="")
            {
                MessageBox.Show(error+"必须为正整数！");
                return;
            }
            int w = Convert.ToInt32(txtWidth.Text);
            int h = Convert.ToInt32(txtHeight.Text);
            double scale = Convert.ToInt32(txtSize1.Text)*1.0/Convert.ToInt32(txtSize2.Text);
            if (txtPath.Text != "")
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.RootFolder=Environment.SpecialFolder.MyComputer;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string path = fbd.SelectedPath;
                    string path1= path+"\\合格";
                    string path2=path+"\\不合格";
                    if(!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(path1);
                    }
                    if (!Directory.Exists(path2))
                    {
                        Directory.CreateDirectory(path2);
                    }
                    CheckForIllegalCrossThreadCalls = false;
                    Thread thread=new Thread(() =>
                    {
                        int n = 0;
                        CopyImage(txtPath.Text, path, w, h, scale, ref n);
                    });
                    thread.Start();
                }
            }
        }
        private void btnDeleteTitle_Click(object sender, EventArgs e)
        {
            if (txtPath.Text != "")
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string path = fbd.SelectedPath;
                    CheckForIllegalCrossThreadCalls = false;
                    Thread thread = new Thread(() =>
                    {
                        int n = 0;
                        string[] files = Directory.GetFiles(txtPath.Text);
                        foreach (string file in files)
                        {
                            string tail = file.Substring(file.LastIndexOf(".")).ToLower();
                            if (tail == ".jpg" || tail == ".png" || tail == ".jpeg" || tail == ".gif")
                            {
                                n++;
                                lblTip.Text = n.ToString();
                                if (!file.Contains("small"))
                                {
                                    RemoveImageTitle(file, path);
                                }
                            }
                        }
                        MessageBox.Show("生成成功！");
                    });
                    thread.Start();
                }
            }
        }

        private void radBmnoSize_CheckedChanged(object sender, EventArgs e)
        {
            if(radBmnoSize.Checked)
            {
                txtSize.Text = "72*54|400*300|800*600|218*164|100*75|240*180|80*60|200*150";
            }
        }

        private void radCloudSize_CheckedChanged(object sender, EventArgs e)
        {
            if (radCloudSize.Checked)
            {
                txtSize.Text = "800*800|400*400|240*240|100*100";
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            Label.CheckForIllegalCrossThreadCalls = false;
          
            Thread thread = new Thread(() =>
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "(文本文件)|*.txt";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    FolderBrowserDialog pathDialog = new FolderBrowserDialog();
                    if (pathDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        StreamReader reader = new StreamReader(dialog.FileName, Encoding.Default);
                        string line = "";
                        int i = 0;
                        while ((line = reader.ReadLine()) != null)
                        {
                            i++;
                            DownLoadImg(line, pathDialog.SelectedPath);
                            lblTip.Text = string.Format("{0}", i);
                        }
                        lblTail.Text = "下载完毕";
                    }
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }


        private void DownLoadImg(string url, string path)
        {
            using (WebClient client = new WebClient())
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    Uri uri = new Uri(url);
                    if (uri != null)
                    {
                        string filename = Path.GetFileName(uri.LocalPath);
                        string[] args = uri.LocalPath.Split('/');
                        string directory = Path.Combine(path, args[args.Length - 2]);
                        if(!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        try
                        {
                            client.DownloadFile(uri, Path.Combine(directory, filename));
                        }
                        catch
                        {
                            TextHelper.SaveToFile(url+"\r\n", "D:\\图片\\error.txt", FileMode.Append);
                        }
                    }
                }
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (lbxDataFile.Text == "")
            {
                MessageBox.Show("请选择要处理的图片路径！");
                return;
            }
            string savepath = txtPath.Text;
            if (MessageBox.Show("是否更改文件保存路径？", "文件路径", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    savepath = folderDialog.SelectedPath;
                }
            }
            Label.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(() => {
                int total = _filePathList.Count;
                int process = 0;
                foreach (string imgPath in _filePathList)
                {
                    process++;
                    Bitmap bmp = new Bitmap(imgPath);
                    int w = bmp.Width;
                    int h = bmp.Height;
                    int colorH1 = 0;//从上到下出现其他非白色的y标
                    int colorH2 = 0;//从下到上出现其他非白色的y标
                    int clipW = 20;//截取的图片宽度
                    for (int i = 0; i < w; i++)
                    {
                        for (int j = 0; j < h; j++)
                        {
                            Color color = bmp.GetPixel(i, j);
                            if (colorH1 == 0)
                            {
                                if (color.R < 250 && color.G < 250 && color.B < 250)
                                {
                                    colorH1 = j;
                                }
                            }
                        }
                        for (int j = h - 1; j >= 0; j--)
                        {
                            Color color = bmp.GetPixel(i, j);
                            if (colorH2 == 0)
                            {
                                if (color.R < 250 && color.G < 250 && color.B < 250)
                                {
                                    colorH2 = j;
                                }
                            }
                        }
                    }

                    //原图片文件
                    //Image fromImage = new Bitmap(@"C:\Users\liujinghe\Desktop\新建文件夹\69084.jpg");
                    //创建新图位图，存储要从源图上截取的图片部分
                    Bitmap originalImage1 = new Bitmap(bmp.Width, clipW);
                    Bitmap originalImage2 = new Bitmap(bmp.Width, clipW);
                    //创建作图区域
                    Graphics graphic1 = Graphics.FromImage(originalImage1);
                    Graphics graphic2 = Graphics.FromImage(originalImage2);
                    //截取原图相应区域写入作图区 
                    graphic1.DrawImage(bmp, 0, 0, new Rectangle(0, colorH1, bmp.Width, clipW), GraphicsUnit.Pixel);
                    graphic2.DrawImage(bmp, 0, 0, new Rectangle(0, colorH2 - clipW, bmp.Width, clipW), GraphicsUnit.Pixel);

                    //originalImage2.Save(@"C:\Users\liujinghe\Desktop\1.png");

                    //从作图区生成新图

                    int towidth1 = bmp.Width;//截取部分拉伸宽度
                    int toheight1 = colorH1 + clipW;//截取部分拉伸高度
                    int towidth2 = bmp.Width;//截取部分拉伸宽度
                    int toheight2 = h - colorH2 + clipW;//截取部分拉伸高度

                    int ow1 = originalImage1.Width;
                    int oh1 = originalImage1.Height;
                    int ow2 = originalImage2.Width;
                    int oh2 = originalImage2.Height;
                    //新建一个bmp图片 
                    Image zoomImage1 = new System.Drawing.Bitmap(towidth1, toheight1);
                    Image zoomImage2 = new System.Drawing.Bitmap(towidth2, toheight2);
                    //新建一个画板 
                    Graphics g1 = System.Drawing.Graphics.FromImage(zoomImage1);
                    Graphics g2 = System.Drawing.Graphics.FromImage(zoomImage2);
                    //g1.Clear(Color.Transparent);
                    //在指定位置并且按指定大小绘制原图片的指定部分 
                    g1.DrawImage(originalImage1, new Rectangle(0, 0, towidth1, toheight1), new Rectangle(0, 0, ow1, oh1), GraphicsUnit.Pixel);
                    g2.DrawImage(originalImage2, new Rectangle(0, 0, towidth2, toheight2), new Rectangle(0, 0, ow2, oh2), GraphicsUnit.Pixel);

                    //zoomImage2.Save(@"C:\Users\liujinghe\Desktop\2.png");

                    //第三步，请拉伸后图片
                    Graphics graphic = Graphics.FromImage(bmp);
                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //graphic.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel);
                    graphic.DrawImage(zoomImage1, new Rectangle(0, 0, bmp.Width, toheight1), 0, 0, zoomImage1.Width, zoomImage1.Height, GraphicsUnit.Pixel);
                    graphic.DrawImage(zoomImage2, new Rectangle(0, colorH2 - clipW, bmp.Width, toheight2+2), 0, 0, zoomImage2.Width, zoomImage2.Height, GraphicsUnit.Pixel);
                    bmp.Save(Path.Combine(savepath, imgPath.Substring(imgPath.LastIndexOf('\\')+1)));

                    //释放1资源   
                    bmp.Dispose();
                    graphic.Dispose();

                    //释放1资源   
                    originalImage1.Dispose();
                    originalImage2.Dispose();
                    g1.Dispose();
                    g2.Dispose();
                    graphic2.Dispose();
                    zoomImage1.Dispose();
                    zoomImage2.Dispose();
                    originalImage1.Dispose();
                    originalImage2.Dispose();
                    lblTip.Text = string.Format("总数：{0}，进度：{1}", total, process);
                }
            });
            thread.Start();
        }

        #region 
        /**/
        /// <summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径（物理路径）</param> 
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式</param> 
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形） 
                    break;
                case "W"://指定宽，高按比例 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形） 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片 
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
            new Rectangle(x, y, ow, oh),
            GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        /// <summary>
        /// 从大图中截取一部分图片
        /// </summary>
        /// <param name="fromImagePath">来源图片地址</param>        
        /// <param name="offsetX">从偏移X坐标位置开始截取</param>
        /// <param name="offsetY">从偏移Y坐标位置开始截取</param>
        /// <param name="toImagePath">保存图片地址</param>
        /// <param name="width">保存图片的宽度</param>
        /// <param name="height">保存图片的高度</param>
        /// <returns></returns>
        public void CaptureImage(string fromImagePath, int offsetX, int offsetY, string toImagePath, int width, int height)
        {
            //原图片文件
            Image fromImage = Image.FromFile(fromImagePath);
            //创建新图位图
            Bitmap bitmap = new Bitmap(width, height);
            //创建作图区域
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            graphic.DrawImage(fromImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
            //从作图区生成新图
            Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
            //保存图片
            saveImage.Save(toImagePath, ImageFormat.Png);
            //释放资源   
            saveImage.Dispose();
            graphic.Dispose();
            bitmap.Dispose();
        }
        #endregion

        private void btnSaveTo_Click(object sender, EventArgs e)
        {
            string imgFormat= cboImgFormat.Text;
            if (txtPath.Text == "")
            {
                MessageBox.Show("请选择要处理的图片路径！");
                return;
            }
            if (imgFormat == "")
            {
                MessageBox.Show("请选择要保存的图片格式!");
                return;
            }
            CheckForIllegalCrossThreadCalls = false;

            Thread tt = new Thread(() =>
            {
                int n = 0;
                btnStart.Enabled = false;
                if (MessageBox.Show("是否更改文件保存路径？", "文件路径", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string savepath = folderDialog.SelectedPath;
                        AnalysisFolder(txtPath.Text, savepath,imgFormat, ref n);
                    }
                }
                else
                {
                    AnalysisFolder(txtPath.Text, ref n);
                }
                MessageBox.Show(this, "生成成功！");
                btnStart.Enabled = true;
            });
            tt.SetApartmentState(ApartmentState.STA);
            tt.Start();
        }

        private void btnToFolder_Click(object sender, EventArgs e)
        {
            Label.CheckForIllegalCrossThreadCalls = false;
            string[] files= Directory.GetFiles(_path);
            int total = files.Count();
            int process = 0;
            Thread thread = new Thread(() => {
                foreach(string file in files)
                {
                    process++;
                    string[] args=file.Split('.');
                    List<string> paths = args[0].Split('\\').ToList();
                    string filename = paths.Last();
                    int standid = Convert.ToInt32(filename.Split('_')[0]);
                    string folder = "201910" + (standid % 10 + 1).ToString().PadLeft(2,'0');
                    string directory = Path.Combine(_path, folder);
                    if(!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    paths.Insert(paths.Count - 1, folder);
                    string newpath = string.Join("\\", paths) + "." + args[1];
                    File.Move(file, newpath);
                    lblTip.Text = string.Format("{0}/{1}", total, process);
                }
            });
            thread.Start();
        }
    }
}

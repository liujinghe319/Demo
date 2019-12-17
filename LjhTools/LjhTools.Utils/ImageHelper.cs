using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Drawing.Drawing2D;
using System.Net;
using System.IO;

namespace LjhTools.Utils
{
    public class ImageHelper
    {
        public Image ResourceImage;
        private int ImageWidth;
        private int ImageHeight;
        public string ErrMessage;
        public ImageHelper()
        {
            
        }
        /// <summary>
        /// 类的构造函数
        /// </summary>
        /// <param name="ImageFileName">图片文件的全路径名称</param>
        public ImageHelper(string ImageFileName)
        {
            ResourceImage = Image.FromFile(ImageFileName);
            ErrMessage = "";
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// 生成缩略图重载方法1，返回缩略图的Image对象
        /// </summary>
        /// <param name="Width">缩略图的宽度</param>
        /// <param name="Height">缩略图的高度</param>
        /// <returns>缩略图的Image对象</returns>
        public Image GetReducedImage(int Width, int Height)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                ReducedImage = ResourceImage.GetThumbnailImage(Width, Height, callb, IntPtr.Zero);

                return ReducedImage;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法2，将缩略图文件保存到指定的路径
        /// </summary>
        /// <param name="Width">缩略图的宽度</param>
        /// <param name="Height">缩略图的高度</param>
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:Imagesfilename.jpg</param>
        /// <returns>成功返回true，否则返回false</returns>
        public bool GetReducedImage(int Width, int Height, string targetFilePath)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                ReducedImage = ResourceImage.GetThumbnailImage(Width, Height, callb, IntPtr.Zero);
                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);
                ReducedImage.Dispose();

                return true;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法3，返回缩略图的Image对象
        /// </summary>
        /// <param name="Percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>  
        /// <returns>缩略图的Image对象</returns>
        public Image GetReducedImage(double Percent)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                ImageWidth = Convert.ToInt32(ResourceImage.Width*Percent);
                ImageHeight = Convert.ToInt32(ResourceImage.Width*Percent);

                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);

                return ReducedImage;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法4，返回缩略图的Image对象
        /// </summary>
        /// <param name="Percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>  
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:Imagesfilename.jpg</param>
        /// <returns>成功返回true,否则返回false</returns>
        public bool GetReducedImage(double Percent, string targetFilePath)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                ImageWidth = Convert.ToInt32(ResourceImage.Width*Percent);
                ImageHeight = Convert.ToInt32(ResourceImage.Width*Percent);

                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);
                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);

                ReducedImage.Dispose();

                return true;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return false;
            }
        }

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

            ///<summary>  
            /// 生成缩略图  ,空白填充
            /// </summary>  
            /// <param name="originalImagePath">源图路径（物理路径，含文件名）</param>  
            /// <param name="thumbnailPath">缩略图路径（物理路径，含文件名）</param>  
            /// <param name="width">缩略图宽度</param>  
            /// <param name="height">缩略图高度</param>  
            /// <param name="outthumbnailPath">返回缩略图路径</param>  
            public static  void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, out string outthumbnailPath)
            {
                System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

                int towidth = width;
                int toheight = height;

                int x = 0; //缩略图在画布上的X放向起始点  
                int y = 0; //缩略图在画布上的Y放向起始点  
                int ow = originalImage.Width;
                int oh = originalImage.Height;
                int dw = 0;
                int dh = 0;

                if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                {
                    //宽比高大，以宽为准  
                    dw = originalImage.Width * towidth / originalImage.Width;
                    dh = originalImage.Height * toheight / originalImage.Width;
                    x = 0;
                    y = (toheight - dh) / 2;
                }
                else
                {
                    //高比宽大，以高为准  
                    dw = originalImage.Width * towidth / originalImage.Height;
                    dh = originalImage.Height * toheight / originalImage.Height;
                    x = (towidth - dw) / 2;
                    y = 0;
                }

                //新建一个bmp图片  
                System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

                //新建一个画板  
                Graphics g = System.Drawing.Graphics.FromImage(bitmap);

                //设置高质量插值法  
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //设置高质量,低速度呈现平滑程度  
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充  
                g.Clear(Color.Transparent);

                //在指定位置并且按指定大小绘制原图片的指定部分  
                g.DrawImage(originalImage, new Rectangle(x, y, dw, dh),
                 new Rectangle(0, 0, ow, oh),
                 GraphicsUnit.Pixel);

                try
                {
                    //以Jpeg格式保存缩略图(KB最小)  
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    outthumbnailPath = thumbnailPath;
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
            /// 北迈自带
            /// </summary>
            /// <param name="pathImageFrom">原图路径</param>
            /// <param name="pathImageTo">缩略图路径</param>
            /// <param name="width">宽度</param>
            /// <param name="height">高度</param>
            public static void GenThumbnail(string pathImageFrom, string pathImageTo, int width, int height)
            {
                Image image = null;
                try
                {
                    image = Image.FromFile(pathImageFrom);
                }
                catch
                {
                }
                if (image != null)
                {
                    int num = image.Width;
                    int num2 = image.Height;
                    int num3 = width;
                    int num4 = height;
                    int x = 0;
                    int y = 0;
                    if ((num4 * num) > (num3 * num2))
                    {
                        num4 = (num2 * width) / num;
                        y = (height - num4) / 2;
                    }
                    else
                    {
                        num3 = (num * height) / num2;
                        x = (width - num3) / 2;
                    }
                    Bitmap bitmap = new Bitmap(width, height);
                    Graphics graphics = Graphics.FromImage(bitmap);
                    if(pathImageTo.ToLower().EndsWith("png"))
                    {
                        graphics.Clear(Color.Transparent);
                    }
                    else
                    {
                        graphics.Clear(Color.White);
                    }
                    graphics.InterpolationMode =InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;//.HighQuality;
                    graphics.DrawImage(image, new Rectangle(x, y, num3, num4), new Rectangle(0, 0, num, num2), GraphicsUnit.Pixel);
                    try
                    {
                        bitmap.Save(pathImageTo, image.RawFormat);//ImageFormat.Png);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        image.Dispose();
                        bitmap.Dispose();
                        graphics.Dispose();
                    }
                }
            }
            /// <summary>
            /// 北迈自带
            /// </summary>
            /// <param name="pathImageFrom">原图路径</param>
            /// <param name="pathImageTo">缩略图路径</param>
            /// <param name="width">宽度</param>
            /// <param name="height">高度</param>
            /// <param name="noZoom">图片不放大，只置于图片的中央</param>
            public static void GenThumbnail(string pathImageFrom, string pathImageTo, int width, int height, bool noZoom)
            {
                Image image = null;
                try
                {
                    image = Image.FromFile(pathImageFrom);
                }
                catch
                {
                }
                if (image != null)
                {
                    int num = image.Width;
                    int num2 = image.Height;
                    int num3 = width;
                    int num4 = height;
                    int x = 0;
                    int y = 0;
                    if ((num4 * num) > (num3 * num2))
                    {
                        num4 = (num2 * width) / num;
                        y = (height - num4) / 2;
                    }
                    else
                    {
                        num3 = (num * height) / num2;
                        x = (width - num3) / 2;
                    }
                    Bitmap bitmap = new Bitmap(width, height);
                    Graphics graphics = Graphics.FromImage(bitmap);
                    if (pathImageTo.ToLower().EndsWith("png"))
                    {
                        graphics.Clear(Color.Transparent);//透明背景
                    }
                    else
                    {
                        graphics.Clear(Color.White);
                    }
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;//.HighQuality;
                    if(num4>num2 && num3>num && noZoom)
                    {
                        graphics.DrawImage(image, new Rectangle((width - num) / 2, (height - num2) / 2, num, num2), new Rectangle(0, 0, num, num2), GraphicsUnit.Pixel);
                    }
                    else
                    {
                        graphics.DrawImage(image, new Rectangle(x, y, num3, num4), new Rectangle(0, 0, num, num2), GraphicsUnit.Pixel);
                    }
                    try
                    {
                        bitmap.Save(pathImageTo, image.RawFormat);//ImageFormat.Png);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        image.Dispose();
                        bitmap.Dispose();
                        graphics.Dispose();
                    }
                }
            }
        /// <summary>
        /// 北迈自带
        /// </summary>
        /// <param name="pathImageFrom">原图路径</param>
        /// <param name="pathImageTo">另存为图路径</param>
        public static void GenThumbnail(string pathImageFrom, string pathImageTo, int qualityParam)
        {
            Image image = null;
            try
            {
                image = Image.FromFile(pathImageFrom);
            }
            catch
            {
            }
            if (image != null)
            {
                int width = image.Width;
                int height = image.Height;
                Bitmap bitmap = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap);
                if (pathImageTo.ToLower().EndsWith("png"))
                {
                    graphics.Clear(Color.Transparent);//透明背景
                }
                else
                {
                    graphics.Clear(Color.White);
                }
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;//.HighQuality;
                graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
                try
                {
                    if (pathImageTo.EndsWith("png"))
                    {
                        bitmap.Save(pathImageTo, image.RawFormat);
                    }
                    else
                    {
                        EncoderParameters encoderParams = new EncoderParameters();
                        EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
                        encoderParams.Param[0] = encoderParam;
                        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                        ImageCodecInfo ici = null;
                        foreach (ImageCodecInfo codec in codecs)
                        {
                            if (codec.MimeType.IndexOf("jpeg") > -1)
                                ici = codec;
                        }
                        bitmap.Save(pathImageTo, ici, encoderParams);
                    }
                }
                catch
                {
                }
                finally
                {
                    image.Dispose();
                    bitmap.Dispose();
                    graphics.Dispose();
                }
            }
        }

            /// <summary>
            /// 北迈自带
            /// </summary>
            /// <param name="image">原图</param>
            /// <param name="pathImageTo">缩略图路径</param>
            /// <param name="width">宽度</param>
            /// <param name="height">高度</param>
            /// <param name="noZoom">图片不放大，只置于图片的中央</param>
            public static void GenThumbnail(Image image, string pathImageTo, int width, int height, bool noZoom)
            {
                if (image != null)
                {
                    int num = image.Width;
                    int num2 = image.Height;
                    int num3 = width;
                    int num4 = height;
                    int x = 0;
                    int y = 0;
                    if ((num4 * num) > (num3 * num2))
                    {
                        num4 = (num2 * width) / num;
                        y = (height - num4) / 2;
                    }
                    else
                    {
                        num3 = (num * height) / num2;
                        x = (width - num3) / 2;
                    }
                    Bitmap bitmap = new Bitmap(width, height);
                    Graphics graphics = Graphics.FromImage(bitmap);
                    if (pathImageTo.ToLower().EndsWith("png"))
                    {
                        graphics.Clear(Color.Transparent);//透明背景
                    }
                    else
                    {
                        graphics.Clear(Color.White);
                    }
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;//.HighQuality;
                    if (num4 > num2 && num3 > num && noZoom)
                    {
                        graphics.DrawImage(image, new Rectangle((width - num) / 2, (height - num2) / 2, num, num2), new Rectangle(0, 0, num, num2), GraphicsUnit.Pixel);
                    }
                    else
                    {
                        graphics.DrawImage(image, new Rectangle(x, y, num3, num4), new Rectangle(0, 0, num, num2), GraphicsUnit.Pixel);
                    }
                    try
                    {
                        bitmap.Save(pathImageTo, image.RawFormat);//ImageFormat.Png);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        //image.Dispose();
                        bitmap.Dispose();
                        graphics.Dispose();
                    }
                }
            }

            /// <summary>
            /// 给图片加水印
            /// </summary>
            /// <param name="img"></param>
            /// <param name="filename">保存目录及文件名</param>
            /// <param name="watermarkFilename">水印图片路径</param>
            /// <param name="watermarkStatus">图片附件添加水印 0=不使用 1=左上 2=中上 3=右上 4=左中 ... 9=右下</param>
            /// <param name="quality">附件图片质量　取值范围 1是　0不是</param>
            /// <param name="watermarkTransparency">图片水印透明度 取值范围1--10 (10为不透明)</param>
            public static void AddImageSignPic(Image img, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
            {
                Graphics g = null;
                //如果原图片是索引像素格式之列的，则需要转换
                if (IsPixelFormatIndexed(img.PixelFormat))
                {
                    Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                    g = Graphics.FromImage(bmp);
                }
                else
                {
                    g=Graphics.FromImage(img);
                }
                //设置高质量插值法
                //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                Image watermark = new Bitmap(watermarkFilename);

                if (watermark.Height > img.Height || watermark.Width > img.Width)
                    return;

                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();

                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };

                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                float transparency = 0.5F;
                if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                    transparency = (watermarkTransparency / 10.0F);


                float[][] colorMatrixElements = {
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                int xpos = 0;
                int ypos = 0;

                switch (watermarkStatus)
                {
                    case 1:
                        xpos = (int)(img.Width * (float).00);//.01)
                        ypos = (int)(img.Height * (float).00);//.01)
                        break;
                    case 2:
                        xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                        ypos = (int)(img.Height * (float).00);//.01)
                        break;
                    case 3:
                        //xpos = (int)((img.Width * (float).99) - (watermark.Width));
                        xpos = (int)((img.Width * (float)1.00) - (watermark.Width));
                        ypos = (int)(img.Height * (float).00);//.01)
                        break;
                    case 4:
                        xpos = (int)(img.Width * (float).00);//.01)
                        ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                        break;
                    case 5:
                        xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                        ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                        break;
                    case 6:
                        xpos = (int)((img.Width * (float).99) - (watermark.Width));
                        ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                        break;
                    case 7:
                        xpos = (int)(img.Width * (float).00);//.01)
                        ypos = (int)((img.Height * (float).99) - watermark.Height);
                        break;
                    case 8:
                        xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                        ypos = (int)((img.Height * (float).99) - watermark.Height);
                        break;
                    case 9:
                        xpos = (int)((img.Width * (float).99) - (watermark.Width));
                        ypos = (int)((img.Height * (float).99) - watermark.Height);
                        break;
                }

                g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType.IndexOf("jpeg") > -1)
                        ici = codec;
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityParam = new long[1];
                if (quality < 0 || quality > 100)
                    quality = 80;

                qualityParam[0] = quality;

                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
                encoderParams.Param[0] = encoderParam;
                Bitmap map = new Bitmap(img);
                //if (ici != null)
                map.Save(filename, ici, encoderParams);
                //else
                //map.Save(filename);

                g.Dispose();
                img.Dispose();
                watermark.Dispose();
                imageAttributes.Dispose();
            }


            /// <summary>
            /// 给图片加水印
            /// </summary>
            /// <param name="img"></param>
            /// <param name="filename">保存目录及文件名</param>
            /// <param name="watermarkFilename">水印图片路径</param>
            /// <param name="watermarkStatus">图片附件添加水印 0=不使用 1=左上 2=中上 3=右上 4=左中 ... 9=右下</param>
            /// <param name="quality">附件图片质量　取值范围 1是　0不是</param>
            /// <param name="watermarkTransparency">图片水印透明度 取值范围1--10 (10为不透明)</param>
            /// <param name="sizeTail">名称是否加尺寸后缀</param>
            /// <param name="tail">图片名称后缀</param>
            /// <param name="tail">图片名称后缀加不加序号</param>
            /// <param name="nozoom">图片是否不放大只置于新图片中央</param>
            public static void AddImageSignPic(Image img, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency, List<int[]> sizes,bool sizeTail,string tail,bool tailIndex,bool noZoom)
            {
                Graphics g = null;
                //如果原图片是索引像素格式之列的，则需要转换
                if (IsPixelFormatIndexed(img.PixelFormat))
                {
                    Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                    g = Graphics.FromImage(bmp);
                }
                else
                {
                    g = Graphics.FromImage(img);
                }
                //设置高质量插值法
                //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                Image watermark = new Bitmap(watermarkFilename);

                if (watermark.Height > img.Height || watermark.Width > img.Width)
                    return;

                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();

                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };

                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                float transparency = 0.5F;
                if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                    transparency = (watermarkTransparency / 10.0F);


                float[][] colorMatrixElements = {
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                int xpos = 0;
                int ypos = 0;

                switch (watermarkStatus)
                {
                    case 1:
                        xpos = (int)(img.Width * (float).00);//.01)
                        ypos = (int)(img.Height * (float).00);//.01)
                        break;
                    case 2:
                        xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                        ypos = (int)(img.Height * (float).00);//.01)
                        break;
                    case 3:
                        //xpos = (int)((img.Width * (float).99) - (watermark.Width));
                        xpos = (int)((img.Width * (float)1.00) - (watermark.Width));
                        ypos = (int)(img.Height * (float).00);//.01)
                        break;
                    case 4:
                        xpos = (int)(img.Width * (float).00);//.01)
                        ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                        break;
                    case 5:
                        xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                        ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                        break;
                    case 6:
                        xpos = (int)((img.Width * (float).99) - (watermark.Width));
                        ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                        break;
                    case 7:
                        xpos = (int)(img.Width * (float).00);//.01)
                        ypos = (int)((img.Height * (float).99) - watermark.Height);
                        break;
                    case 8:
                        xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                        ypos = (int)((img.Height * (float).99) - watermark.Height);
                        break;
                    case 9:
                        xpos = (int)((img.Width * (float).99) - (watermark.Width));
                        ypos = (int)((img.Height * (float).99) - watermark.Height);
                        break;
                }

                g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType.IndexOf("jpeg") > -1)
                        ici = codec;
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityParam = new long[1];
                if (quality < 0 || quality > 100)
                    quality = 80;

                qualityParam[0] = quality;

                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
                encoderParams.Param[0] = encoderParam;
                //Bitmap map = new Bitmap(img);
                //if (ici != null)
                //map.Save(filename, ici, encoderParams);
                //else
                //map.Save(filename);
                int i = 1;
                int n2 = filename.LastIndexOf('.');
                foreach (int[] size in sizes)
                {
                    string newimg = "";
                    StringBuilder tailSbr = new StringBuilder();
                    if (sizeTail)
                    {
                        tailSbr.AppendFormat("_{0}x{1}", size[0], size[1]);
                    }
                    tailSbr.AppendFormat(tail);
                    if (tailIndex)
                    {
                        tailSbr.Append(i);
                    }
                    newimg = filename.Insert(n2, tailSbr.ToString());
                    ImageHelper.GenThumbnail(img, newimg, size[0], size[1], noZoom);
                    i++;
                }
                g.Dispose();
                img.Dispose();
                watermark.Dispose();
                imageAttributes.Dispose();
            }

            /// <summary>
            /// 加图片水印
            /// </summary>
            /// <param name="filename">文件名</param>
            /// <param name="watermarkFilename">水印文件名</param>
            /// <param name="watermarkStatus">图片水印位置 图片附件添加水印 0=不使用 1=左上 2=中上 3=右上 4=左中 ... 9=右下</param>
            /// <param name="quality">附件图片质量　取值范围 0-100</param>
            /// <param name="watermarkTransparency">图片水印透明度 取值范围1--10 (10为不透明)</param>
            public static bool AddImageSignPic(string picurl, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
            {

                string imgpath = picurl;

                HttpWebRequest wrequest;
                HttpWebResponse wresponse;
                Stream s;
                System.Drawing.Image img;
                try
                {
                    wrequest = (HttpWebRequest)HttpWebRequest.Create(imgpath);
                    //wrequest.Timeout = 60000;
                    //wrequest.KeepAlive = false;
                    //wrequest.AllowAutoRedirect = true;

                    wresponse = (HttpWebResponse)wrequest.GetResponse();
                    s = wresponse.GetResponseStream();
                    img = System.Drawing.Image.FromStream(s);
                }
                catch
                {
                    return false;
                    //throw new Exception(string.Concat(e.Message, "请确认此地址是否可以访问:", imgpath));
                }


                Graphics g;
                if (IsPixelFormatIndexed(img.PixelFormat))
                {
                    Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                    using (Graphics g2 = Graphics.FromImage(bmp))
                    {
                        g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g2.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g2.DrawImage(img, 0, 0);
                    }
                    g = Graphics.FromImage(bmp);
                }
                else
                {
                    g = Graphics.FromImage(img);
                }
                //设置高质量插值法
                //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                if (!File.Exists(watermarkFilename))
                {
                    throw new Exception("找不到水印图片，请到系统设置里重新设置");
                }
                Image watermark = new Bitmap(watermarkFilename);

                if (watermark.Height >= img.Height || watermark.Width >= img.Width)
                {
                    img.Save(filename);

                    g.Dispose();
                    img.Dispose();
                    watermark.Dispose();
                    if (!Equals(s, null))
                    {
                        s.Close();
                        s.Dispose();
                    }
                    wresponse.Close();
                    wrequest.Abort();

                    wrequest = null;

                    return true;
                }

                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();

                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };

                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                float transparency = 0.5F;
                if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                {
                    transparency = (watermarkTransparency / 10.0F);
                }

                float[][] colorMatrixElements = {
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                int xpos = 0;
                int ypos = 0;

                switch (watermarkStatus)
                {
                    case 1:
                        xpos = (int)(img.Width * (float).01);
                        ypos = (int)(img.Height * (float).01);
                        break;
                    case 2:
                        xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                        ypos = (int)(img.Height * (float).01);
                        break;
                    case 3:
                        xpos = (int)((img.Width * (float).99) - (watermark.Width));
                        ypos = (int)(img.Height * (float).01);
                        break;
                    case 4:
                        xpos = (int)(img.Width * (float).01);
                        ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                        break;
                    case 5:
                        xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                        ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                        break;
                    case 6:
                        xpos = (int)((img.Width * (float).99) - (watermark.Width));
                        ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                        break;
                    case 7:
                        xpos = (int)(img.Width * (float).01);
                        ypos = (int)((img.Height * (float).99) - watermark.Height);
                        break;
                    case 8:
                        xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                        ypos = (int)((img.Height * (float).99) - watermark.Height);
                        break;
                    case 9:
                        xpos = (int)((img.Width * (float).99) - (watermark.Width));
                        ypos = (int)((img.Height * (float).99) - watermark.Height);
                        break;
                }

                g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);
                //g.DrawImage(watermark, new System.Drawing.Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, System.Drawing.GraphicsUnit.Pixel);

                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType.IndexOf("jpeg") > -1)
                    {
                        ici = codec;
                    }
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityParam = new long[1];
                if (quality < 0 || quality > 100)
                {
                    quality = 80;
                }
                qualityParam[0] = quality;

                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
                encoderParams.Param[0] = encoderParam;

                if (ici != null)
                {
                    img.Save(filename, ici, encoderParams);
                }
                else
                {
                    img.Save(filename);
                }

                g.Dispose();
                img.Dispose();
                watermark.Dispose();
                imageAttributes.Dispose();
                if (!Equals(s, null))
                {
                    s.Close();
                    s.Dispose();
                }

                wresponse.Close();

                wrequest.Abort();
                wrequest = null;
                return true;
            }
            /// <summary>
            /// 会产生graphics异常的PixelFormat
            /// </summary>
            private static PixelFormat[] indexedPixelFormats = { PixelFormat.Undefined, PixelFormat.DontCare, PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed, PixelFormat.Format8bppIndexed };

            /// <summary>
            /// 判断图片的PixelFormat 是否在 引发异常的 PixelFormat 之中
            /// 无法从带有索引像素格式的图像创建graphics对象
            /// </summary>
            /// <param name="imgPixelFormat">原图片的PixelFormat</param>
            /// <returns></returns>
            private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
            {
                foreach (PixelFormat pf in indexedPixelFormats)
                {
                    if (pf.Equals(imgPixelFormat)) return true;
                }

                return false;
            }
            #region 从大图中截取一部分图片
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
    }
}

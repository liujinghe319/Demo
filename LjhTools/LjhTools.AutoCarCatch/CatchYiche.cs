using LjhTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LjhTools.AutoCarCatch
{
    public partial class CatchYiche : Form
    {
        Dictionary<string, List<string>> configDic = new Dictionary<string, List<string>>();
        private WebBrowser webBrowser1 = new WebBrowser();
        public CatchYiche()
        {
            InitializeComponent();
            webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
        }
        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            YiCheCatch(sender, e);
        }
        void YiCheCatch(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            configDic = new Dictionary<string, List<string>>();
            int n=0;//记录车型数
            string url = e.Url.ToString();
            string url2 = webBrowser1.Url.ToString();
            WebBrowserReadyState state = webBrowser1.ReadyState;
            if (state != WebBrowserReadyState.Complete || url != url2) return;
            //if (e.Url.ToString() != webBrowser1.Url.ToString()) return;
            btnCatch.Enabled = true;
            List<List<string>> allList = new List<List<string>>();
            string html = webBrowser1.Document.Body.InnerHtml;
            #region 获取主机厂车型
            int st = html.IndexOf("<div class=\"crumbs-txt\">", 0);
            int ed = html.IndexOf("</div>", st + 3);
            string caption = html.Substring(st, ed - st + 1);
            List<string> captionlist = GetInnerTextList(caption, "a");
            captionlist.Remove("易车");
            captionlist.Remove("车型");
            captionlist.Remove("参数");
            string brand = "";
            string factory = "";
            string carmodel = "";
            if(captionlist.Count==2)
            {
                brand = captionlist[0];
                factory = captionlist[0];
                carmodel = captionlist[1];
            }
            else if(captionlist.Count==3)
            {
                brand = captionlist[0];
                factory = captionlist[1];
                carmodel = captionlist[2];
            }
            #endregion
            st = html.IndexOf("id=\"CarCompareContent\"");
            st = html.IndexOf("<table", st);
            ed = html.IndexOf("</table>", st);
            string table = html.Substring(st, ed - st + 8);//获得Table表内容
            List<string> trList = GetInnerTextList(table, "tr");
            int r = 0;
            foreach(string tr in trList)
            {
                r++;
                List<string> innerlist = new List<string>();
                if(r==1)
                {
                    string title = "车型名称";
                    //第一列
                    List<string> tdList = GetInnerTextList(tr, "td");
                    foreach(string td in tdList)
                    {
                        List<string> ddlist = GetInnerTextList(td, "dd");
                        if(ddlist.Count==0)
                        {
                            break;
                        }
                        string car = GetInnerText(ddlist[0],"a");
                        innerlist.Add(car);
                    }
                    configDic.Add(title, innerlist);
                    n = innerlist.Count;//列数
                    lblTip.Text = title;
                }
                else
                {
                    if(tr.IndexOf("<th")>=0)
                    {
                        string title = GetInnerText(tr, "th");
                        title = RemoveElement(title, "em");
                        title = GetInnerText(title, "a").Trim();

                        if (title == "定速巡航")
                        {
                            int fjl = 2;
                        }
                        List<string> tdList=new List<string>();
                        //if (title == "限滑差速器/差速锁" || title == "定速巡航")
                        //{
                        //    tdList = GetInnerTextList2(tr, "td");
                        //}
                        //else
                        //{
                        tdList = GetInnerTextList2(tr, "td", n);
                        //}
                        foreach(string td in tdList)
                        {
                            string txt = "";
                            if (title == "厂商指导价")
                            {
                                txt = GetInnerText(td, "b");
                            }
                            else if (title == "商家报价" || title == "北京参考价")
                            {
                                if (td == "无")
                                {
                                    txt = td;
                                }
                                else
                                {
                                    List<string> txtList = GetInnerTextList(td, "a");
                                    txt = txtList[0];
                                }
                            }
                            else if(title=="车身颜色" || title=="外观颜色")
                            {
                                List<string> txtList = GetInnerTextList(td, "li");
                                StringBuilder sbr = new StringBuilder();
                                foreach(string li in txtList)
                                {
                                    sbr.Append(GetFirstPropertyValue(li, "title"));
                                    sbr.Append(";");
                                }
                                txt = sbr.ToString();
                            }
                            else
                            {
                                txt = RemoveElementTag(td, "div");
                                txt = RemoveElementTag(txt, "strong");
                                txt = RemoveElementTag(txt, "i");
                                txt = RemoveElementTag(txt, "span");
                                txt = txt.Replace("&nbsp;","").Trim();
                            }
                            innerlist.Add(txt);
                        }
                        #if DEBUG
                            //Console.WriteLine(title);
                        #endif
                        if(configDic.ContainsKey(title))
                        {
                            configDic.Add(title+2, innerlist);
                        }
                        else
                        {
                            configDic.Add(title, innerlist);
                        }
                        lblTip.Text = title;
                    }
                }

            }
            #region 匹配数据库字段
            Dictionary<string, List<string>> newconfigDic = new Dictionary<string, List<string>>();
            foreach(var obj in configDic)
            {
                if(obj.Key=="限滑差速器/差速锁")
                {
                    List<string> frontList = new List<string>();
                    List<string> afterList = new List<string>();
                    List<string> centerList = new List<string>();
                    foreach(string str in obj.Value)
                    {
                        if(str.Trim()=="-")
                        {
                            frontList.Add("-");
                            centerList.Add("-");
                            afterList.Add("-");
                        }
                        else if (str.IndexOf("前")>0)
                        {
                            frontList.Add("●");
                            centerList.Add("");
                            afterList.Add("");
                        }
                        else if (str.IndexOf("后") > 0)
                        {
                            frontList.Add("");
                            centerList.Add("");
                            afterList.Add("●");
                        }
                        else if (str.IndexOf("中央") > 0)
                        {
                            frontList.Add("");
                            centerList.Add("●");
                            afterList.Add("");
                        }
                        else
                        {
                            frontList.Add("");
                            centerList.Add("");
                            afterList.Add("");
                        }
                    }
                    newconfigDic.Add("前桥限滑差速器/差速锁", frontList);
                    newconfigDic.Add("后桥限滑差速器/差速锁", afterList);
                    newconfigDic.Add("中央差速器结构", centerList);
                }
                else if (obj.Key == "变速箱类型")
                {
                    newconfigDic.Add("变速箱", obj.Value);
                }
                else if (obj.Key == "变速箱类型2")
                {
                    newconfigDic.Add("变速箱类型", obj.Value);
                }
                else if (matchDic.ContainsKey(obj.Key))
                {
                    newconfigDic.Add(matchDic[obj.Key], obj.Value);
                    if (obj.Key == "长×宽×高[mm]")
                    {
                        //5250x1902x1498
                        List<string> lengthList = new List<string>();
                        List<string> widthList = new List<string>();
                        List<string> hightList = new List<string>();
                        foreach (string str in obj.Value)
                        {
                            string[] attr = str.Split('x');
                            if (attr.Count() >= 3)
                            {
                                lengthList.Add(attr[0]);
                                widthList.Add(attr[1]);
                                hightList.Add(attr[2]);
                            }
                            else
                            {
                                lengthList.Add("");
                                widthList.Add("");
                                hightList.Add("");
                            }
                        }
                        newconfigDic.Add("长度(mm)", lengthList);
                        newconfigDic.Add("宽度(mm)", widthList);
                        newconfigDic.Add("高度(mm)", hightList);
                    }
                }
                else if (matchDic2.ContainsKey(obj.Key))
                {
                    List<string> newList = matchDic2[obj.Key];
                    if (!newconfigDic.ContainsKey(newList[0]))
                    {
                        List<string> list1 = null;
                        List<string> list2 = null;
                        if (configDic.ContainsKey(newList[1]))
                        {
                            list1 = configDic[newList[1]];
                        }
                        if (configDic.ContainsKey(newList[2]))
                        {
                            list2 = configDic[newList[2]];
                        }
                        List<string> list = new List<string>();
                        for (int j = 0; j < obj.Value.Count; j++)
                        {
                            StringBuilder sbr = new StringBuilder();
                            sbr.Append(list1 == null ? "" : list1[j]);
                            sbr.Append(" / ");
                            sbr.Append(list2 == null ? "" : list2[j]);
                            list.Add(sbr.ToString());
                        }
                        newconfigDic.Add(newList[0], list);
                    }
                }
                else
                {
                    newconfigDic.Add(obj.Key, obj.Value);
                }
            }
            #endregion
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("品牌", typeof(string)));
            dt.Columns.Add(new DataColumn("厂商", typeof(string)));
            dt.Columns.Add(new DataColumn("车系", typeof(string)));
            foreach (string head in newconfigDic.Keys)//匹配后，原样输出用configDic
            {
                dt.Columns.Add(new DataColumn(head,typeof(string)));
            }

            for(int i=0;i<n;i++)
            {
                DataRow row = dt.NewRow();
                row["品牌"] = brand;
                row["厂商"] = factory;
                row["车系"] = carmodel;
                foreach (var config in newconfigDic)//匹配后，原样输出用configDic
                {
                    string val = config.Value[i];
                    int n1 = val.IndexOf("选配");
                    if (n1 >= 0)
                        val = val.Substring(0, n1);
                    row[config.Key] = val;
                }
                dt.Rows.Add(row);
            }
            savefile(carmodel, dt);
        }
        private void btnCatch_Click(object sender, EventArgs e)
        {
            btnCatch.Enabled = false;
            webBrowser1.Navigate(txtUrl.Text);            
        }

        /// <summary>
        /// 获得元素中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        private List<string> GetInnerTextList(string html, string elem)
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
        /// 获得元素中间的内容,特别针对 限滑差速器/差速锁， 有td开始，无td结尾
        /// </summary>
        /// <param name="html">源文件</param>
        /// <param name="elem">标签</param>
        /// <param name="n">次数</param>
        /// <returns></returns>
        private List<string> GetInnerTextList2(string html, string elem,int n=9999)
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
                ed = html.IndexOf(string.Format("<{0}", elem), st);
                if(ed<0)
                {
                    ed = html.LastIndexOf(">");
                    string inner = html.Substring(st + 1, ed - st);
                    inner=inner.Replace(string.Format("</{0}>", elem), "");
                    list.Add(inner);
                    break;
                }
                else
                {
                    string inner = html.Substring(st + 1, ed - st - 1);
                    inner = inner.Replace(string.Format("</{0}>", elem), "");
                    list.Add(inner);
                    if (list.Count == n) break;
                }
                st = ed;
            }
            return list;
        }

        /// <summary>
        /// 获得元素中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        private string GetInnerText(string html, string elem)
        {
            List<string> list = new List<string>();
            int st = 0;
            int ed = 0;
            int l = elem.Length;
            st = html.IndexOf(string.Format("<{0}", elem), st);
            if (st < 0) return html;
            st = html.IndexOf(">", st + l + 1);
            ed = html.IndexOf(string.Format("</{0}>", elem), st);
            string inner = html.Substring(st + 1, ed - st - 1);
            return inner;
        }
        /// <summary>
        /// 根据属性获得值，只取第一个
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetFirstPropertyValue(string html, string property)
        {
            int st = html.IndexOf(property, 0);
            if (st < 0) return "";
            st = html.IndexOf("\"", st + property.Length);
            int ed = html.IndexOf("\"", st + 1);
            return html.Substring(st + 1, ed - st - 1);
        }
        /// <summary>
        /// 去除元素，保留中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        private string RemoveElementTag(string html, string elem)
        {
            List<string> list = new List<string>();
            int st = 0;
            int ed = 0;
            int len = elem.Length;
            string result = html;
            while (true)
            {
                st = result.IndexOf(string.Format("<{0}", elem));
                if (st < 0)
                {
                    break;
                }
                ed = result.IndexOf(">", st+1);
                result = result.Remove(st, ed - st + 1);
                st = ed + 1;
            }
            result=result.Replace(string.Format("</{0}>",elem),"");
            return result;
        }
        /// <summary>
        /// 去除元素及内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        private string RemoveElement(string html, string elem)
        {
            List<string> list = new List<string>();
            int st = 0;
            int ed = 0;
            int len = elem.Length;
            string result = html;
            while (true)
            {
                st = result.IndexOf(string.Format("<{0}", elem));
                if (st < 0)
                {
                    break;
                }
                ed = result.IndexOf(string.Format("</{0}>", elem), st + len + 1);
                result = result.Remove(st, ed - st +len+3);
                st = ed + 1;
            }
            return result;
        }
        private string downloadHtml(string url)
        {
            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            string html = "";
            try
            {
                html = wc.DownloadString(url);
                html = Regex.Replace(html, @">\s+<", "><");
                html = Regex.Replace(html, @"\r\n", "");
                html = Regex.Replace(html, @"\r", "");
                html = Regex.Replace(html, @"\n", "");
                html = Regex.Replace(html, @"\t", "");
            }
            catch (Exception)
            {
                goto aaa;
            }
        aaa: return html;
        }

        private void savefile(string filename, DataTable dt)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\"+string.Format("{0}.xls",filename);
            ExcelHelper.Export(dt, "", path);
            MessageBox.Show("采集完成！");
        }
        //易车对照
        private Dictionary<string, string> matchDic = new Dictionary<string, string> {
        {"厂商指导价","厂商指导价(元)"},
        {"保修政策","整车质保"},
        {"排量[L]","排量(L)"},
        {"排量","排量(ml)"},
        {"车身型式","车身结构"},
        {"车型级别","级别"},
        {"动力类型","燃料形式"},
        {"最高车速[km/h]","最高车速(km/h)"},
        {"长×宽×高[mm]","长*宽*高(mm)"},
        {"轴距[mm]","轴距(mm)"},
        {"整备质量[kg]","整备质量(kg)"},
        {"座位数[个]","座位数(个)"},
        {"油箱容积[L]","油箱容积(L)"},
        {"备胎","备胎规格"},
        {"最小离地间隙[mm]","最小离地间隙(mm)"},
        {"车身颜色","外观颜色"},
        {"排气量","排量(mL)"},
        {"最大功率[kW]","最大功率(kW)"},
        {"最大马力[Ps]","最大马力(Ps)"},
        {"最大功率转速[rpm]","最大功率转速(rpm)"},
        {"最大扭矩[N.m]","最大扭矩(N·m)"},
        {"最大扭矩转速[rpm]","最大扭矩转速(rpm)"},
        {"缸体形式","气缸排列形式"},
        {"气缸数[缸]","气缸数(个)"},
        {"发动机启停","发动机启停技术"},
        {"混合工况油耗[L/100km]","工信部综合油耗(L/100km)"},
        {"可调悬架","可变悬架"},
        {"前轮制动器类型","前制动器类型"},
        {"后轮制动器类型","后制动器类型"},
        {"限滑差速器/差速锁","前桥限滑差速器/差速锁"},
        {"防抱死制动(ABS)","ABS防抱死"},
        {"制动辅助(BA/EBA/BAS等)","刹车辅助(EBA/BAS/BA等)"},
        {"牵引力控制(ARS/TCS/TRC等)","牵引力控制(ASR/TCS/TRC等)"},
        {"车身稳定控制(ESP/DSC/VSC等)","车身稳定控制(ESC/ESP/DSC等)"},
        {"侧安全气帘","前/后排头部气囊(气帘)"},
        {"胎压监测","胎压监测装置"},
        {"零胎压续行轮胎","零胎压继续行驶"},
        {"后排儿童座椅接口(ISO FIX/LATCH)","ISOFIX儿童座椅接口"},
        {"车道保持","车道偏离预警系统"},
        {"碰撞报警/主动刹车","主动刹车/主动安全系统"},
        {"自动泊车","自动泊车入位"},
        {"自动驾驶辅助","整体主动转向系统"},
        {"倒车影像","倒车视频影像"},
        {"前大灯","近光灯"},
        {"LED日间行车灯","日间行车灯"},
        {"自动大灯","自动头灯"},
        {"大灯功能","大灯高度可调"},
        {"天窗类型","全景天窗"},
        {"外后视镜电动调节","后视镜电动调节"},
        {"隐私玻璃","后排侧隐私玻璃"},
        {"后遮阳帘","后风挡遮阳帘"},
        {"前雨刷器","感应雨刷"},
        {"后雨刷器","后雨刷"},
        {"电吸门","电动吸合门"},
        {"电动侧滑门","侧滑门"},
        {"电动行李厢","电动后备厢"},
        {"中控锁","车内中控锁"},
        {"智能钥匙","无钥匙启动系统"},
        {"方向盘材质","真皮方向盘"},
        {"前排空调","空调控制方式"},
        {"后排空调","后排独立空调"},
        {"空气净化","车内空气调节/花粉过滤"},
        {"主座椅调节方式","座椅高低调节"},
        {"第二排座椅调节方式","第二排座椅移动"},
        {"后排座椅功能","后排座椅电动调节"},
        {"座椅放倒方式","后排座椅放倒方式"},
        {"中控彩色液晶屏","中控台彩色大屏"},
        {"HUD平视显示","HUD抬头数字显示"},
        {"GPS导航","GPS导航系统"},
        {"智能互联定位","定位互动服务"},
        {"CD/DVD","CD支持MP3/WMA"},
        {"蓝牙/WIFI连接","蓝牙/车载电话"},
        {"外接接口","外接音源接口"},
        {"音响品牌","扬声器品牌"},
        {"扬声器数量[个]","扬声器数量"},
        {"后排液晶屏/娱乐系统","后排液晶屏"},
        {"车载220V电源","220V/230V电源"},
        {"行李厢容积[L]","行李厢容积(L)"},
        {"0-100km/h加速时间[s]","官方0-100km/h加速(s)"},
        {"电动机总功率[kW]","电动机总功率(kW)"},
        {"电动机总扭矩[N.m]","电动机总扭矩(N·m)"},
        {"最大续航里程[km]","工信部续航里程(km)"}};
        Dictionary<string, List<string>> matchDic2 = new Dictionary<string, List<string>> { 
        {"主驾驶安全气囊",new List<string>{"主/副驾驶座安全气囊","主驾驶安全气囊","副驾驶安全气囊"}},
        {"副驾驶安全气囊",new List<string>{"主/副驾驶座安全气囊","主驾驶安全气囊","副驾驶安全气囊"}},
        {"前侧气囊",new List<string>{"前/后排侧气囊","前侧气囊","后侧气囊"}},
        {"后侧气囊",new List<string>{"前/后排侧气囊","前侧气囊","后侧气囊"}},
        {"前倒车雷达",new List<string>{"前/后驻车雷达","前倒车雷达","后倒车雷达"}},
        {"后倒车雷达",new List<string>{"前/后驻车雷达","前倒车雷达","后倒车雷达"}},
        {"前电动车窗",new List<string>{"前/后电动车窗","前电动车窗","后电动车窗"}},
        {"后电动车窗",new List<string>{"前/后电动车窗","前电动车窗","后电动车窗"}},
        {"内后视镜自动防眩目",new List<string>{"内/外后视镜自动防眩目","内后视镜自动防眩目","外后视镜自动防眩目"}},
        {"外后视镜自动防眩目",new List<string>{"内/外后视镜自动防眩目","内后视镜自动防眩目","外后视镜自动防眩目"}},
        {"主座椅电动调节",new List<string>{"主/副驾驶座电动调节","主座椅电动调节","副座椅电动调节"}},
        {"副座椅电动调节",new List<string>{"主/副驾驶座电动调节","主座椅电动调节","副座椅电动调节"}},
        {"前排中央扶手",new List<string>{"前/后中央扶手","前排中央扶手","后排中央扶手"}},
        {"后排中央扶手",new List<string>{"前/后中央扶手","前排中央扶手","后排中央扶手"}}};

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTip_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

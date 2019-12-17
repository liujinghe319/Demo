using LjhTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LjhTools.AutoCarCatch
{
    public partial class Catch360Che : Form
    {
        public Catch360Che()
        {
            InitializeComponent();
            System.Windows.Forms.Label.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnCatchConfig_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                Dictionary<string, List<string>> configDictionary = new Dictionary<string, List<string>>();
                List<string> linklist = txtLinks.Lines.Where(ln => ln.Trim() != "").ToList();
                if (linklist.Count == 0)
                {
                    string filename = ControlHelper.SelectFile(FileType.Text);
                    if (filename != "")
                    {
                        List<string> txtList = TextHelper.GetFileTextList(filename, Encoding.Default);
                        txtLinks.Lines = txtList.ToArray();
                        linklist=txtList.Where(ln => ln.Trim() != "").ToList();
                    }
                }
                if (linklist.Count>0)
                {
                    int total=linklist.Count;
                    int process=0;
                    foreach (string link in linklist)
                    {
                        configDictionary.Clear();
                        process++;
                        List<string> columnList = new List<string>();
                        string html = "";
                        int errorCount = 0;
                        while(true)
                        {
                            try
                            {
                                html = RequestHelper.GetWebRequest(link, System.Text.Encoding.GetEncoding("utf-8"));
                                break;
                            }
                            catch
                            {
                                errorCount++;
                                if (errorCount == 3) break;
                                Thread.Sleep(2000);
                            }
                        }
                        if (errorCount == 3) continue;
                        string carseries = HtmlHelper.GetInnerText(html, "class=\"inner", "</div>");
                        string caption = HtmlHelper.GetFirstInnerText(carseries, "a");
                        string inner = HtmlHelper.GetInnerText(html, "class=\"parameter-detail", "<p class=\"notes");
                        #region 固定头，前二行，第三行不要
                        string thead = HtmlHelper.GetInnerText(inner, "<thead", "</thead");
                        List<string> theadList = HtmlHelper.GetInnerTextList(thead, "tr");
                        int n = 0;
                        foreach (string tr in theadList)
                        {
                            n++;
                            if (n == 1)
                            {
                                //车型名称 
                                List<string> thlist = HtmlHelper.GetInnerTextList(tr, "th");
                                string title = "车型名称";
                                List<string> carlist = new List<string>();
                                for (int i = 1; i < thlist.Count; i++)
                                {
                                    string car = HtmlHelper.GetFirstInnerText(thlist[i], "a");
                                    carlist.Add(car);
                                }
                                configDictionary.Add(title, carlist);
                            }
                            else
                            {
                                //车型名称 
                                List<string> thlist = HtmlHelper.GetInnerTextList(tr, "td");
                                string title = thlist[0].Replace("：", "");
                                if (title != "本地最低报价")
                                {
                                    List<string> configlist = new List<string>();
                                    for (int i = 1; i < thlist.Count; i++)
                                    {
                                        string cpnfig = thlist[i].Trim();
                                        configlist.Add(cpnfig);
                                    }
                                    if (!configDictionary.ContainsKey(title))
                                    {
                                        configDictionary.Add(title, configlist);
                                    }
                                }
                            }
                        }
                        #endregion
                        #region 主要内容
                        string tbody = HtmlHelper.GetFirstInnerText(inner, "tbody");
                        List<string> trList = HtmlHelper.GetInnerTextList(tbody, "tr");
                        foreach (string tr in trList)
                        {
                            if (tr.IndexOf("colspan=") >= 0)
                            {
                                continue;//分组名（基本信息）
                            }
                            else
                            {
                                List<string> tdList = HtmlHelper.GetInnerTextList(tr, "td");
                                string title = tdList[0].Replace("：", "");
                                List<string> configlist = new List<string>();
                                for (int i = 1; i < tdList.Count; i++)
                                {
                                    string config = HtmlHelper.GetInnerText(tdList[i], "<div>", "<span");
                                    config = config.Replace("<div>", "").Trim();
                                    configlist.Add(config);
                                }
                                if(!configDictionary.ContainsKey(title))
                                {
                                    configDictionary.Add(title, configlist);
                                }
                            }
                        }
                        #endregion
                        DataTable dt = new DataTable();
                        foreach (string key in configDictionary.Keys)
                        {
                            dt.Columns.Add(new DataColumn(key, typeof(string)));
                        }
                        int rowcount=configDictionary.Values.First().Count;
                        for (int i = 0; i < rowcount; i++)
                        {
                            DataRow row = dt.NewRow();
                            int j = 0;
                            foreach (var data in configDictionary)
                            {
                                row[j] = data.Value[i];
                                j++;
                            }
                            dt.Rows.Add(row);
                        }
                        if(total==1)
                        {
                            string filePath = ControlHelper.GetSavePath(FileType.Excel);
                            ExcelHelper.TableToExcel(dt, filePath);
                        }
                        else
                        {
                            int n1=link.LastIndexOf("/");
                            int n2=link.LastIndexOf(".");
                            string filePath = Path.Combine(Config.SavePath, link.Substring(n1 + 1, n2 - n1 - 1)+".xls");
                            ExcelHelper.TableToExcel(dt, filePath);
                            lblTip.Text = string.Format("总数：{0},进度：{1}",total,process);
                        }
                    }
                    lblTip.Text = "采集完成！";
                }
                else
                {
                    lblTip.Text = "没在要采集的链接！";
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void btnAllBrands_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => {
                DataTable dt = new DataTable();
                dt.Columns.Add("品牌", typeof(string));
                dt.Columns.Add("品牌链接", typeof(string));
                dt.Columns.Add("品牌图片", typeof(string));
                dt.Columns.Add("主机厂", typeof(string));
                dt.Columns.Add("主机厂链接", typeof(string));
                dt.Columns.Add("车型", typeof(string));
                dt.Columns.Add("车型链接", typeof(string));
                string link = "https://product.360che.com/BrandList.html#pvareaid=1010101";
                string html = RequestHelper.GetWebRequest(link, System.Text.Encoding.GetEncoding("utf-8"));
                string key = "class=\"xll_center2_a1_y1\"";//主机厂关键字
                int st = html.IndexOf("class=\"xll_center2_a1_z\"",0);
                int ed = html.IndexOf(key, st);
                string brandhtml = html.Substring(st, ed - st);
                brandhtml = HtmlHelper.GetInnerText(brandhtml, "<dt>", "</dt>");
                string brand = HtmlHelper.GetFirstPropertyValue(brandhtml, "title");
                string brandlink = HtmlHelper.GetFirstPropertyValue(brandhtml, "href");
                string brandimg = HtmlHelper.GetFirstPropertyValue(brandhtml, "src");
                st = ed;
                while (st >= 0)
                {
                    bool nextbrand = false;
                    st = html.IndexOf(key, st);
                    if (st >= 0)
                    {
                        string factory = HtmlHelper.GetInnerText(html, key, "</div>", ref st);
                        if (factory == "") break;
                        string factoryname = HtmlHelper.GetFirstInnerText(factory, "a");
                        lblTip.Text = string.Format("正在采集：{0}", factoryname);
                        string factorylink = HtmlHelper.GetFirstPropertyValue(factory, "href");
                        ed = html.IndexOf(key, st);//寻找下一个主机厂，st上一家主机厂开始位置
                        string carseries = "";
                        if (ed < 0)//没有下一个主机厂
                        {
                            int ed2 = html.IndexOf("<script", st);
                            carseries = html.Substring(st, ed2 - st);
                        }
                        else
                        {
                            int n1 = html.IndexOf("class=\"xll_center2_a1_z\"",st);
                            if (n1 >= 0 && n1<ed)
                            {
                                nextbrand = true;
                                brandhtml = html.Substring(n1, ed - st);
                                brandhtml = HtmlHelper.GetInnerText(brandhtml, "<dt>", "</dt>");
                                /*brand = HtmlHelper.GetFirstPropertyValue(brandhtml, "title");
                                brandlink = HtmlHelper.GetFirstPropertyValue(brandhtml, "href");
                                brandimg = HtmlHelper.GetFirstPropertyValue(brandhtml, "src");*/
                                carseries = html.Substring(st, n1 - st);
                                ed = n1;
                            }
                            else
                            {
                                carseries = html.Substring(st, ed - st);
                            }
                        }
                        List<string> carList = HtmlHelper.GetInnerTextList(carseries, "<dt", "</dt");
                        foreach (string car in carList)
                        {
                            DataRow row = dt.NewRow();
                            row["品牌"] = brand;
                            row["品牌链接"] = brandlink;
                            row["品牌图片"] = brandimg;
                            row["主机厂"] = factoryname;
                            row["主机厂链接"] = factorylink;
                            row["车型"] = HtmlHelper.GetFirstInnerText(car, "a");
                            row["车型链接"] = HtmlHelper.GetFirstPropertyValue(car, "href");
                            dt.Rows.Add(row);
                        }
                        if (nextbrand)
                        {
                            string brand1 = HtmlHelper.GetFirstPropertyValue(brandhtml, "title");
                            if(brand1.Trim()!="")
                            {
                                brand = brand1;
                                brandlink = HtmlHelper.GetFirstPropertyValue(brandhtml, "href");
                                brandimg = HtmlHelper.GetFirstPropertyValue(brandhtml, "src");
                            }
                        }
                        if (ed < 0) break;
                        st = ed+key.Length;
                    }
                }
                string path = ControlHelper.GetSavePath(FileType.Excel);
                if (path.Length > 0)
                {
                    ExcelHelper.TableToExcel(dt, path);
                }
                lblTip.Text = "采集完毕！";
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("车型ID", typeof(string));
                dt.Columns.Add("车型", typeof(string));
                dt.Columns.Add("车系", typeof(string));
                dt.Columns.Add("款数", typeof(string));
                dt.Columns.Add("车系链接", typeof(string));
                string file=ControlHelper.SelectFile(FileType.Excel);
                if(file!="")
                {
                    DataTable carTable = ExcelHelper.ExcelToTable(file);
                    int total = carTable.Rows.Count;
                    int process = 0;
                    try
                    {
                        foreach (DataRow row in carTable.Rows)
                        {
                            process++;
                            string id = row["Id"].ToString();
                            string carseries = row["车型"].ToString();
                            string link = row["车型链接"].ToString();
                            if (!string.IsNullOrEmpty(id))
                            {
                                link = string.Format("https://product.360che.com{0}", link);
                                string html = RequestHelper.GetWebRequest(link, System.Text.Encoding.GetEncoding("utf-8"));
                                int st = html.IndexOf("id=\"filter_content\"");
                                while (st > 0)
                                {
                                    int n1 = html.IndexOf("class=\"tractor-price-content price-wrap\"", st);
                                    if (n1 < 0) break;
                                    int n2 = html.IndexOf("</h3>", st) + 5;
                                    string inner = html.Substring(n1, n2 - n1);
                                    string carlink = HtmlHelper.GetFirstPropertyValue(inner, "href");
                                    string h3 = HtmlHelper.GetFirstInnerText(inner, "h3").Trim();
                                    string carname = h3.Substring(0, h3.IndexOf("<span")).Trim();
                                    string carcount = HtmlHelper.GetFirstInnerText(h3, "em").Trim();
                                    DataRow row1 = dt.NewRow();
                                    row1["车型ID"] = id;
                                    row1["车型"] = carseries;
                                    row1["车系"] = carname;
                                    row1["款数"] = carcount;
                                    row1["车系链接"] = carlink;
                                    dt.Rows.Add(row1);
                                    st = n2;
                                }
                                Thread.Sleep(1000);
                            }
                            lblTip.Text = string.Format("总数：{0}，进度：{1}", total, process);
                        }
                    }
                    catch
                    {
                        lblTip.Text = "采集出错："+process;
                    }
                }
                string path = ControlHelper.GetSavePath(FileType.Excel);
                if (path.Length > 0)
                {
                    ExcelHelper.TableToExcel(dt, path);
                    lblTip.Text = "采集完毕！";
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void btnCarDetail_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("车系ID", typeof(string));
                dt.Columns.Add("车系", typeof(string));
                dt.Columns.Add("配置名称", typeof(string));
                dt.Columns.Add("配置链接", typeof(string));
                dt.Columns.Add("配置", typeof(string));
                dt.Columns.Add("厂商指导价", typeof(string));
                dt.Columns.Add("销售状态", typeof(string));
                string file = ControlHelper.SelectFile(FileType.Excel);
                if (file != "")
                {
                    DataTable carTable = ExcelHelper.ExcelToTable(file);
                    int total = carTable.Rows.Count;
                    int process = 0;
                    //try
                    {
                        foreach (DataRow row in carTable.Rows)
                        {
                            process++;
                            string id = row["Id"].ToString();
                            string carseries = row["车系"].ToString();
                            string link = row["车系链接"].ToString();
                            if (!string.IsNullOrEmpty(id))
                            {
                                link = string.Format("https://product.360che.com{0}", link);
                                string html = "";
                                int errorCount = 0;
                                while(true)
                                {
                                    try
                                    {
                                        html=RequestHelper.GetWebRequest(link, System.Text.Encoding.GetEncoding("utf-8"));
                                        break;
                                    }
                                    catch
                                    {
                                        errorCount++;
                                        Thread.Sleep(2000);
                                        if (errorCount == 3)
                                        {
                                            break;
                                        }
                                    }

                                }
                                if (errorCount == 3) continue;
                                List<string> keyList = new List<string> { "id=\"listprice1\"", "id=\"listprice2\"", "id=\"listprice3\"", "id=\"listprice4\"", "class=\"module" };
                                List<string> statusList = new List<string> { "无报价", "在售", "状态2", "未上市", "停售" };
                                int n=0;
                                string status = statusList[0];
                                int st = html.IndexOf("id=\"listprice0\"");
                                if (st < 0)
                                {
                                    for (int i = 0; i < keyList.Count; i++)
                                    {
                                        st = html.IndexOf(keyList[i]);
                                        n++;
                                        if (st > 0)
                                        {
                                            status = statusList[n];
                                            break;
                                        }
                                    }
                                }
                                while(n<5)
                                {
                                    int ed =html.IndexOf(keyList[n], st);
                                    if(ed==-1)
                                    {
                                        n++;
                                        continue;
                                    }
                                    string zaishou = html.Substring(st, ed - st);
                                    List<string> tbodyList = HtmlHelper.GetInnerTextList(zaishou, "tbody");
                                    foreach(string tbody in tbodyList)
                                    {
                                        List<string> trlist = HtmlHelper.GetInnerTextList(tbody, "tr");
                                        foreach(string tr in trlist)
                                        {
                                            List<string> tdlist = HtmlHelper.GetInnerTextList(tr, "td");
                                            string carname = HtmlHelper.GetFirstInnerText(tdlist[0], "a");
                                            string carlink = HtmlHelper.GetFirstPropertyValue(tdlist[0], "href");
                                            string keys = string.Join(" ", HtmlHelper.GetInnerTextList(tdlist[0], "span"));
                                            string price = HtmlHelper.GetFirstInnerText(tdlist[2], "span");
                                            DataRow row1 = dt.NewRow();
                                            row1["车系ID"] = id;
                                            row1["车系"] = carseries;
                                            row1["配置名称"] = carname;
                                            row1["配置链接"] = carlink;
                                            row1["配置"] = keys;
                                            row1["厂商指导价"] = price;
                                            row1["销售状态"] = status;
                                            dt.Rows.Add(row1);
                                        }
                                    }
                                    n++;
                                    if(n<5)
                                    {
                                        status = statusList[n];
                                    }
                                    st = ed;
                                }
                                Thread.Sleep(1000);
                            }
                            lblTip.Text = string.Format("总数：{0}，进度：{1}", total, process);
                            ExcelHelper.TableToExcel(dt, Path.Combine(Config.SavePath, carseries.Replace("/", "-").Replace("\\", "-")+id + ".xls"));
                            dt.Rows.Clear();
                        }
                    }
                    //catch
                    {
                        lblTip.Text = "采集出错：" + process;
                    }
                }
                lblTip.Text = "采集完毕！";
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                string file = ControlHelper.SelectFile(FileType.Excel);
                if (file != "")
                {
                    DataTable carTable = ExcelHelper.ExcelToTable(file);
                    int total = carTable.Rows.Count;
                    int process = 0;
                    //try
                    {
                        foreach (DataRow row in carTable.Rows)
                        {
                            Dictionary<string, string> tableDictinary = new Dictionary<string, string>();//表格资源字典
                            process++;
                            string id = row["Id"].ToString();
                            string carseries = row["车系"].ToString();
                            string carseriesid = row["车系ID"].ToString();
                            string configname = row["配置名称"].ToString();
                            string price = row["厂商指导价"].ToString();
                            string salestatus = row["销售状态"].ToString();
                            string link = row["配置链接"].ToString();
                            if (!string.IsNullOrEmpty(id))
                            {
                                link = string.Format("https://product.360che.com{0}", link);
                                string html = "";
                                int errorCount = 0;
                                while (true)
                                {
                                    try
                                    {
                                        html = RequestHelper.GetWebRequest(link, System.Text.Encoding.GetEncoding("utf-8"));
                                        break;
                                    }
                                    catch
                                    {
                                        errorCount++;
                                        Thread.Sleep(2000);
                                        if (errorCount == 3)
                                        {
                                            break;
                                        }
                                    }

                                }
                                if (errorCount == 3) continue;
                                int st = html.IndexOf("参数说明：");
                                if (st >0)
                                {
                                    int ed = html.IndexOf("注：不符合中", st);
                                    if(ed>0)
                                    {
                                        html = html.Substring(st, ed - st);
                                        List<string> trList = HtmlHelper.GetInnerTextList(html, "tr");
                                        foreach(string tr in trList)
                                        {
                                            List<string> tdList = HtmlHelper.GetInnerTextList(tr, "td");
                                            string td0 = tdList[0].Replace("<div>", "").Replace("</div>", "").Replace("：", "").Trim();
                                            string td2 = tdList[2].Replace("<div>", "").Replace("</div>", "").Replace("：", "").Trim();
                                            if(td0!="" && !tableDictinary.ContainsKey(td0))
                                            {
                                                tableDictinary.Add(td0, tdList[1].Replace("<div>", "").Replace("</div>", "").Trim());
                                            }
                                            if (td2 != "" && !tableDictinary.ContainsKey(td0))
                                            {
                                                tableDictinary.Add(td2, tdList[3].Replace("<div>", "").Replace("</div>", "").Trim());
                                            }
                                        }
                                        DataTable dt=new DataTable();
                                        dt.Columns.Add(new DataColumn("Id",typeof(string)));
                                        dt.Columns.Add(new DataColumn("车系",typeof(string)));
                                        dt.Columns.Add(new DataColumn("车系ID",typeof(string)));
                                        dt.Columns.Add(new DataColumn("配置名称",typeof(string)));
                                        dt.Columns.Add(new DataColumn("厂商指导价", typeof(string)));
                                        dt.Columns.Add(new DataColumn("销售状态", typeof(string)));
                                        dt.Columns.Add(new DataColumn("配置链接",typeof(string)));
                                        foreach(string key in tableDictinary.Keys)
                                        {
                                            dt.Columns.Add(new DataColumn(key,typeof(string)));
                                        }
                                        DataRow newrow = dt.NewRow();
                                        newrow["Id"]=id;
                                        newrow["车系"]=carseries;
                                        newrow["车系ID"]=carseriesid;
                                        newrow["配置名称"] = configname;
                                        newrow["厂商指导价"] = price;
                                        newrow["销售状态"] = salestatus;
                                        newrow["配置链接"] = link;
                                        foreach(var td in tableDictinary)
                                        {
                                            newrow[td.Key] = td.Value;
                                        }
                                        dt.Rows.Add(newrow);
                                        string[] cartypes=carseries.Split(' ');
                                        string cartype = cartypes.Last();
                                        ExcelHelper.TableToExcel(dt, Path.Combine(Config.SavePath, string.Format("{0}_{1}.xls",cartype.Replace("/", "-"),id)));
                                    }
                                }
                                Thread.Sleep(1000);
                            }

                            lblTip.Text = string.Format("总数：{0}，进度：{1}", total, process);
                        }
                    }
                    //catch
                    {
                        lblTip.Text = "采集出错：" + process;
                    }
                }
                lblTip.Text = "采集完毕！";
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}

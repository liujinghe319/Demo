using LjhTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LjhTools.AutoCarCatch
{
    public partial class AutoHomeDataFrm : Form
    {
        /*
        HtmlDocument doc = webBrowser1.Document;
        HtmlElement inp_product = doc.All["product"];
        HtmlElement inp_chexing = doc.All["chexing"];
        HtmlElement form = doc.Forms[0];
        inp_product.SetAttribute("value", "刹车片");
        inp_chexing.SetAttribute("value", "北京现代i30");
        doc.InvokeScript("alert");
        $('#car .tag_options li')[2].click();while(true){if($('#car1 .tag_options li').length!=0){$('#car1 .tag_options li')[2].click();break;}}while(true){if($('#car2 .tag_options li').length!=0){$('#car2 .tag_options li')[2].click();break;}}$('form')[0].submit();
        HtmlDocument doc = webBrowser1.Document;
        HtmlElement script = doc.CreateElement("script");
        script.SetAttribute("type", "text/javascript");
        script.SetAttribute("text", "$('#car .tag_options li')[2].click();while(true){if($('#car1 .tag_options li').length!=0){$('#car1 .tag_options li')[2].click();break;}}while(true){if($('#car2 .tag_options li').length!=0){$('#car2 .tag_options li')[2].click();break;}}$('form')[0].submit();");
        doc.Body.AppendChild(script);
        */
        private Dictionary<string, string> headerKeyDic = new Dictionary<string, string>();
        private WebBrowser webBrowser1 = new WebBrowser();
        public AutoHomeDataFrm()
        {
            System.Windows.Forms.Label.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            //string url="http://car.autohome.com.cn/config/spec/29788.html#pvareaid=2023149";
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (radAutoHome.Checked)
            {
                AutoHomeCatch(sender, e);
            }
            else if(radPacific.Checked)
            {
                PacificCatch(sender, e);
            }
        }

        void AutoHomeMobileCatch(List<int> caridList)
        {
            Thread thread = new Thread(() => {

                int total = caridList.Count;
                int process = 0;
                int loss = 0;
                StringBuilder sbrError = new StringBuilder();
                foreach (int id in caridList)
                {
                    process++;
                    try
                    {
                        string url = string.Format("https://car.m.autohome.com.cn/ashx/car/GetModelConfigNew2.ashx?seriesId={0}", id);
                        string html = RequestHelper.GetHttpWebRequest(url);
                        html = html.Replace("\\u0027", "'");
                        html = html.Replace("\\u003e", ">");
                        html = html.Replace("\\u003c", "<");
                        int st = html.IndexOf("decodeURIComponent']('");
                        int ed = html.IndexOf("'+$SystemFunction1$");
                        string config = html.Substring(0, html.IndexOf("\\\"search\\\":"));
                        string urlUnicode = "";
                        string position = "";
                        if (st > 0 && ed > st)
                        {
                            urlUnicode = html.Substring(st + 22, ed - st - 22);
                        }
                        st = html.IndexOf("$SystemFunction1$('')+'");
                        ed = html.IndexOf("'),$SystemFunction2$");
                        if (st > 0 && ed > st)
                        {
                            position = html.Substring(st + 23, ed - st - 23);
                        }
                        string keys = System.Web.HttpUtility.UrlDecode(urlUnicode, System.Text.Encoding.GetEncoding("utf-8"));
                        string[] positions = position.Split(';');
                        List<string> keyList = new List<string>();
                        foreach (string pos in positions)
                        {
                            StringBuilder sbr = new StringBuilder();
                            string[] arr = pos.Split(',');
                            foreach (string p in arr)
                            {
                                sbr.Append(keys.Substring(Convert.ToInt32(p), 1));
                            }
                            keyList.Add(sbr.ToString());
                        }
                        st = config.IndexOf("\\\"name\\\"");
                        ed = 0;
                        List<string> columnList = new List<string>();
                        List<List<string>> configList = new List<List<string>>();
                        while (ed >= 0)
                        {
                            string item = "";
                            ed = config.IndexOf("\\\"name\\\"", st + 8);
                            if (ed > 0)
                            {
                                item = config.Substring(st, ed - st - 2);
                            }
                            else
                                item = config.Substring(st);
                            if (item.IndexOf("valueitems") >= 0)
                            {
                                List<string> itemList = new List<string>();
                                int start = item.IndexOf("name\\\":\\\"");
                                int end = item.IndexOf("\\\",\\\"valueitems");
                                string name = item.Substring(start + 9, end - start - 9);
                                name = ReplaceKeyWord(name, keyList);
                                if (columnList.Contains(name))
                                {
                                    for (int j = 0; j < 100; j++)
                                    {
                                        string newname = string.Format("{0}{1}", name, j);
                                        if (!columnList.Contains(newname))
                                        {
                                            name = newname;
                                            break;
                                        }
                                    }
                                }
                                if (name == "皮质方向盘") name = "真皮方向盘";
                                if (name.ToLower() == "led日间行车灯") name = "日间行车灯";
                                if (name == "扬声器个数") name = "扬声器数量";
                                if (name == "防紫外线玻璃") name = "防紫外线/隔热玻璃";
                                if (name == "手机互联/映射功能") name = "手机互联/映射";
                                columnList.Add(name);
                                itemList.Add(name);
                                start = 0;
                                while (start >= 0)
                                {
                                    start = item.IndexOf("value\\\":\\\"", end);
                                    end = item.IndexOf("\\\"}", start + 10);
                                    if (start < 0) break;
                                    string str = item.Substring(start + 10, end - start - 10);
                                    str = ReplaceKeyWord(str, keyList);
                                    itemList.Add(str);
                                }
                                configList.Add(itemList);
                            }
                            st = ed;
                        }
                        DataTable dt = new DataTable();
                        foreach (List<string> list in configList)
                        {
                            dt.Columns.Add(new DataColumn(list[0], typeof(string)));
                        }
                        int n = configList[0].Count;
                        for (int i = 1; i < n; i++)
                        {
                            DataRow row = dt.NewRow();
                            foreach (List<string> list in configList)
                            {
                                row[list[0]] = list[i];
                            }
                            dt.Rows.Add(row);
                        }
                        string tableName = dt.Rows[0][0].ToString();
                        if (tableName.IndexOf(" ") > 0) tableName = tableName.Substring(0, tableName.IndexOf(" "));
                        string path = string.Format("D://导出数据/{0}_{1}.xls", id,tableName);
                        ExcelHelper.Export(dt, "", path);
                    }
                    catch(Exception ex)
                    {
                        sbrError.AppendFormat("{0}\r\n", id);
                        loss++;
                    }
                    lblTip.Text = string.Format("总数：{0}，进度：{1}", total, process);
                }
                if(loss>0)
                {
                    TextHelper.SaveToFile(sbrError.ToString(), Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"/采集失败ID.txt", false);
                }
                MessageBox.Show("采集成功！失败："+loss+"个！");
            });
            thread.Start();
        }
        private string ReplaceKeyWord(string item,List<string> keyList)
        {
            string str = item;
            str = item.Replace("</span>", "").Replace("nbsp;"," ").Replace("\\\\u0026","&").Replace("& /& "," / ");
            int st = str.IndexOf("<span");
            int ed = 0;
            while(st>=0)
            {
                ed = str.IndexOf(">", st + 2);
                string span = str.Substring(st, ed - st+1);
                int st2 = span.IndexOf("class='hs_kw");
                int ed2 = span.IndexOf("_",st2+12);
                int num = Convert.ToInt32(span.Substring(st2 + 12, ed2 - st2 - 12));
                str = str.Replace(span, keyList[num]);
                st = str.IndexOf("<span");
            }
            return str;
        }
        void AutoHomeCatch(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url=e.Url.ToString();
            string url2=webBrowser1.Url.ToString();
            WebBrowserReadyState state = webBrowser1.ReadyState;
            if (state != WebBrowserReadyState.Complete || url!=url2) return;
            //if (e.Url.ToString() != webBrowser1.Url.ToString()) return;
            btnCatch.Enabled = true;
            int rel = KeyDicInitional();
            if(rel==0)
            {
                MessageBox.Show("CSS样式数据不能为空！");
            }
            else if(rel==-1)
            {
                MessageBox.Show("样式数据不正确！");
            }
            else
            {
                List<List<string>> allList = new List<List<string>>();
                string html = webBrowser1.Document.Body.InnerHtml;
                int st = html.IndexOf("id=config_nav");
                if (st == -1) st = html.IndexOf("id=\"config_nav\"");
                int st0 = html.IndexOf("<tbody>", st);
                if (st0 == -1)
                {
                    st = html.IndexOf("<TBODY>", st);
                }
                else
                {
                    st = st0;
                }
                int ed = html.IndexOf("</tbody>", st);
                if (ed == -1) ed=html.IndexOf("</TBODY>", st);
                //获得车型头
                string header = html.Substring(st + 7, ed - st - 7);
                List<string> versionList = GetVersionList(header.ToLower());
                versionList.Insert(0, "车型名称");
                allList.Add(versionList);
                // tab_0,1,2,3,4,5,100,101,102,103,104,105,106,107,108
                st = html.IndexOf("id=\"config_data\"", ed);
                if (st == -1) st = html.IndexOf("id=config_data", ed);
                st0 = html.IndexOf("class=\"tbcs\"", st);
                if (st0 == -1)
                {
                    st = html.IndexOf("class=tbcs", st);
                }
                else
                {
                    st = st0;
                }
                st0 = html.IndexOf("class=\"tbcs\"", st + 10);
                if (st0 == -1)
                {
                    st = html.IndexOf("class=tbcs", st + 10); 
                }
                else
                {
                    st = st0;
                }
                st0 = html.IndexOf("<tbody>", st);
                if (st0 == -1)
                {
                    st = html.IndexOf("<TBODY>", st);
                }
                else
                {
                    st = st0;
                }
                ed = html.IndexOf("</tbody>", st);
                if (ed == -1) ed = html.IndexOf("</TBODY>", st);
                string table = html.Substring(st + 7, ed - st - 7);
                //价格

                List<List<string>> priceList = GetPriceTable(table);//价格
                allList.AddRange(priceList);
                //tab_0 基本参数
                table = GetTableById(html, "tab_0");
                List<List<string>> tabList_0 = GetTable_0(table);//
                allList.AddRange(tabList_0);
                //tab_1
                table = GetTableById(html, "tab_1");
                List<List<string>> tabList_1 = GetTable_0(table);//
                allList.AddRange(tabList_1);
                //tab_2
                table = GetTableById(html, "tab_2");
                List<List<string>> tabList_2 = GetTable_0(table);//
                allList.AddRange(tabList_2);
                //tab_3
                table = GetTableById(html, "tab_3");
                List<List<string>> tabList_3 = GetTable_0(table);//
                allList.AddRange(tabList_3);
                //tab_4
                table = GetTableById(html, "tab_4");
                List<List<string>> tabList_4 = GetTable_0(table);//
                allList.AddRange(tabList_4);
                //tab_5
                table = GetTableById(html, "tab_5");
                List<List<string>> tabList_5 = GetTable_0(table);//
                allList.AddRange(tabList_5);
                //tab_6
                table = GetTableById(html, "tab_6");
                if(!string.IsNullOrEmpty(table))
                {
                    List<List<string>> tabList_6 = GetTable_0(table);//
                    allList.AddRange(tabList_6);
                }
                //tab_100
                table = GetTableById(html, "tab_100");
                List<List<string>> tabList_100 = GetTable_0(table);//
                allList.AddRange(tabList_100);
                //tab_101
                table = GetTableById(html, "tab_101");
                List<List<string>> tabList_101 = GetTable_0(table);//
                allList.AddRange(tabList_101);
                //tab_102
                table = GetTableById(html, "tab_102");
                List<List<string>> tabList_102 = GetTable_0(table);//
                allList.AddRange(tabList_102);
                //tab_103
                table = GetTableById(html, "tab_103");
                List<List<string>> tabList_103 = GetTable_0(table);//
                allList.AddRange(tabList_103);
                //tab_104
                table = GetTableById(html, "tab_104");
                List<List<string>> tabList_104 = GetTable_0(table);//
                allList.AddRange(tabList_104);
                //tab_105
                table = GetTableById(html, "tab_105");
                List<List<string>> tabList_105 = GetTable_0(table);//
                allList.AddRange(tabList_105);
                //tab_106
                table = GetTableById(html, "tab_106");
                List<List<string>> tabList_106 = GetTable_0(table);//
                allList.AddRange(tabList_106);
                //tab_107
                table = GetTableById(html, "tab_107");
                List<List<string>> tabList_107 = GetTable_0(table);//
                allList.AddRange(tabList_107);
                //tab_108
                table = GetTableById(html, "tab_108");
                List<List<string>> tabList_108 = GetTable_0(table);//
                allList.AddRange(tabList_108);
                table = GetTableById(html, "tab_200");
                List<List<string>> tabList_200 = GetTable_200(table);//
                allList.AddRange(tabList_200);
                //导出表
                DataTable dt = new DataTable();
                List<string> columnnames = new List<string>();
                //先加入表头
                foreach (List<string> list in allList)
                {
                    string th = ReplaceHeader(list[0]);
                    if (columnnames.Contains(th))
                    {
                        string newth = "";
                        for(int j=2;j<10;j++)
                        {
                            newth = th + j;
                            if (dt.Columns.Contains(newth)) continue;
                            if (newth == "皮质方向盘") newth = "真皮方向盘";
                            if (newth.ToLower() == "led日间行车灯") newth = "日间行车灯";
                            dt.Columns.Add(newth, typeof(string));
                            columnnames.Add(newth);
                            break;
                        }
                    }
                    else
                    {
                        if (th == "皮质方向盘") th = "真皮方向盘";
                        if (th.ToLower() == "led日间行车灯") th = "日间行车灯";
                        dt.Columns.Add(th, typeof(string));
                        columnnames.Add(th);
                    }
                }
                int n = versionList.Count - 1;//行数
                for (int i = 0; i < n; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
                for (int i = 0; i < n; i++)
                {
                    int col = allList.Count();//列数 190
                    for (int j = 0; j < col; j++)
                    {
                        string td = ReplaceHeader(allList[j][i + 1]);
                        dt.Rows[i][j] = td.ToUpper();
                    }
                }
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel文件(*.xls)|*.xls";
                if(sfd.ShowDialog()==DialogResult.OK)
                {
                    string path = sfd.FileName;
                    ExcelHelper.Export(dt, "", path);
                    MessageBox.Show("导出成功！");
                }
            }            
        }
        void PacificCatch(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = e.Url.ToString();
            string url2 = webBrowser1.Url.ToString();
            WebBrowserReadyState state = webBrowser1.ReadyState;
            if (state != WebBrowserReadyState.Complete || url != url2) return;
            //if (e.Url.ToString() != webBrowser1.Url.ToString()) return;
            btnCatch.Enabled = true;
            List<List<string>> allList = new List<List<string>>();
            Thread.Sleep(1000);
            string html = webBrowser1.Document.Body.InnerHtml;
            int st = html.IndexOf("class=position");
            st = html.IndexOf("<DIV", st + 10);
            int ed = html.IndexOf("</DIV>", st);
            string h1 = html.Substring(st, ed - st + 6);
            List<string> navList = GetInnerText(h1, "A");

            st= html.IndexOf("id=config_nav");
            st = html.IndexOf("<TBODY>", st);
            ed = html.IndexOf("</TBODY>", st);
            //获得车型年款
            string header = html.Substring(st + 7, ed - st - 7);
            List<string> versionList = GetVersionList2(header);
            versionList.Insert(0, "车型名称");
            allList.Add(versionList);
            List<string> priceList = GetPriceList(header);
            priceList.Insert(0, "厂商指导价(元)");
            allList.Add(priceList);
            
            st=html.IndexOf("id=config_data",ed);
            ed=html.IndexOf("id=ctip",st);
            html = html.Substring(st, ed-st);
            List<string> tableList = GetInnerText(html, "TABLE");
            foreach(string table in tableList)
            {
                int n1 = table.IndexOf("class=gbicot");
                if(n1<0)
                {
                    continue;
                    //去掉无关Table，只保留带分组标签的Table
                }
                List<List<string>> trList = GetConfigList(table);
                allList.AddRange(trList);
            }
            //导出表
            DataTable dt = new DataTable();
            List<string> columnnames = new List<string>();
            //先加入表头
            dt.Columns.Add("品牌", typeof(string));
            dt.Columns.Add("车系", typeof(string));
            columnnames.Add("品牌");
            columnnames.Add("车系");
            foreach (List<string> list in allList)
            {
                string th = list[0];
                if(th=="皮质方向盘") th="真皮方向盘";
                if(th.ToLower()=="led日间行车灯") th="日间行车灯";
                th = MathAutoHomeHeader(th);
                if (columnnames.Contains(th))
                {
                    dt.Columns.Add(th + "_2", typeof(string));
                }
                else
                {
                    dt.Columns.Add(th, typeof(string));
                }
                columnnames.Add(th);
            }
            dt.Columns.Add("前/后电动车窗", typeof(string));
            columnnames.Add("前/后电动车窗");
            dt.Columns.Add("排量(L)", typeof(string));
            columnnames.Add("排量(L)");
            dt.Columns.Add("排量", typeof(string));
            columnnames.Add("排量");
            int n = versionList.Count - 1;//行数
            for (int i = 0; i < n; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            for (int i = 0; i < n; i++)
            {
                int col = allList.Count();//列数 190
                dt.Rows[i][0] = navList[2];
                dt.Rows[i][1] = navList[4];
                for (int j = 0; j < col; j++)
                {
                    dt.Rows[i][j+2] = allList[j][i + 1];
                }
                #region 车窗,排量
                StringBuilder sbr=new StringBuilder();
                if (columnnames.Contains("前电动车窗") && dt.Rows[i]["前电动车窗"] != "" && dt.Rows[i]["前电动车窗"] != null)
                {
                    sbr.Append(dt.Rows[i]["前电动车窗"]);
                }
                else{                    
                    sbr.Append("-");
                }
                sbr.Append(" / ");
                if (columnnames.Contains("后电动车窗") && dt.Rows[i]["后电动车窗"] != null && dt.Rows[i]["后电动车窗"] != "")
                {
                    sbr.Append(dt.Rows[i]["后电动车窗"]);
                }
                else{                    
                    sbr.Append("-");
                }
                if (columnnames.Contains("排量(mL)")&& dt.Rows[i]["排量(mL)"] != null&&dt.Rows[i]["排量(mL)"] != "-"&&dt.Rows[i]["排量(mL)"] != "" )
                {
                    try
                    {
                        int vol=Convert.ToInt32(dt.Rows[i]["排量(mL)"]);
                        double result = Math.Round((vol / 1000.0f), 1);
                        if(dt.Rows[i]["进气形式"].ToString()=="涡轮增压")
                        {
                            dt.Rows[i]["排量(L)"] = string.Format("{0}T", result);
                            dt.Rows[i]["排量"] = string.Format("{0}T", result);
                        }
                        else
                        {
                            dt.Rows[i]["排量(L)"] = string.Format("{0}L", result);
                            dt.Rows[i]["排量"] = string.Format("{0}T", result);
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
                #endregion
                dt.Rows[i]["前/后电动车窗"] = sbr.ToString();
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel文件(*.xls)|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                ExcelHelper.Export(dt, "", path);
                MessageBox.Show("导出成功！");
            }
        }

        private void btnCatch_Click(object sender, EventArgs e)
        {
            btnCatch.Enabled = false;
            if (!radPhoneAutoHome.Checked)
            {
                webBrowser1.Navigate(txtUrl.Text);
            }
            else
            {
                List<int> seriesidList = txtKeyWord.Lines.Where(id=>!string.IsNullOrWhiteSpace(id)).Select(id => Convert.ToInt32(id)).ToList();
                if (seriesidList.Count>0)
                {
                    AutoHomeMobileCatch(seriesidList);
                    btnCatch.Enabled = true;
                }
                else
                {
                    MessageBox.Show("车系ID不能为空！");
                }
            }
        }
        private int KeyDicInitional()
        {
            headerKeyDic.Clear();
            string keylist = txtKeyWord.Text.Trim();
            if(keylist=="")
            {
                return 0;//空
            }            
            try
            {
                keylist = keylist.Replace(":before {\r\n    content: \"", "");
                keylist = keylist.Replace("\";\r\n}\r\n", "|");
                keylist = keylist.Replace("\";\r\n}", "|");
                keylist = keylist.Replace(".", "");
                string[] attr = keylist.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in attr)
                {
                    string[] keys = str.Split(':');
                    string key = keys[0];
                    int st=key.IndexOf("_config");
                    if (st > 0) key = key.Substring(0, st + 7);
                    st = key.IndexOf("_baike");
                    if (st > 0) key = key.Substring(0, st + 6);
                    st = key.IndexOf("_option");
                    if (st > 0) key = key.Substring(0, st + 7);
                    key = key.Trim();
                    headerKeyDic.Add("<span class="+key+"[^>]*>", keys[1]);
                }
                return 1;
            }
            catch(Exception ex)
            {
                return -1;//错误
            }           
        }
        #region 汽车之家方法
        private List<string> GetVersionList(string html)
        {
            List<string> list=new List<string>();
            int st = 0;
            while(st>=0)
            {
                int st0 = html.IndexOf("<a href=",st);
                if(st0==-1)
                {
                    st = html.IndexOf("<A href=", st);
                }
                else
                {
                    st = st0;
                }
                if (st < 0) break;
                st=html.IndexOf(">",st+2);
                int ed = html.IndexOf("</a>", st);
                if (ed == -1) ed = html.IndexOf("</A>", st);
                string str = html.Substring(st + 1, ed - st - 1);
                str=str.Replace("<span class=\"", "[");
                str=str.Replace("\"></span>", "]");
                list.Add(str);
                st = ed + 4;
            }
            return list;
        }
        private List<List<string>> GetPriceTable(string html1)
        {
            string html = html1.ToLower();           
            Regex regex1 = new Regex(@"<tr[^>]*>(.*?)</tr>", RegexOptions.Multiline);
            Regex regex2 = new Regex(@"<th[^>]*>(.*?)</th>", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@"<td[^>]*>(.*?)</td>", RegexOptions.IgnoreCase);
            List<List<string>> list = new List<List<string>>();
            List<string> trlist = GetInnerText(html, "tr");
            foreach (string tr1 in trlist)
            {
                string tr = tr1.Replace("\r\n", "");
                List<string> tdList = new List<string>();

                string th = regex2.Match(tr).Groups[0].Value;
                th = Regex.Replace(th, @"</?th[^>]*>", "");//替换th
                th = Regex.Replace(th, @"</?div[^>]*>", "");//替换div
                tdList.Add(th);
                List<string> tds = GetInnerText(tr, "td");
                foreach (string inner in tds)
                {
                    string td = inner;
                    if (tdList[0] == "经销商参考价")
                    {
                        List<string> prices = GetInnerText(inner, "a");
                        td = prices.Count > 0 ? prices[0] : "";
                    }
                    else
                    {
                        int n = td.IndexOf("<div class=\"pop pop02 fn-hide\"", 0);
                        if (n >= 0)
                        {
                            td = td.Remove(n);
                        }
                        td = Regex.Replace(td, @"</?div[^>]*>", "");//替换div
                        td = Regex.Replace(td, @"<a[^>]*>(.*?)</a>", "");//替换a
                        td = Regex.Replace(td, @"<span[^>]*>(.*?)</span>", "");//替换span
                    }
                    tdList.Add(td);
                }
                list.Add(tdList);
            }
            return list;
        }
        private List<List<string>> GetTable_0(string html1)
        {
            string html = html1.ToLower(); 
            Regex regex1 = new Regex(@"<tr[^>]*>(.*?)</tr>", RegexOptions.Multiline);
            Regex regex2 = new Regex(@"<th[^>]*>(.*?)</th>", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@"<td[^>]*>(.*?)</td>", RegexOptions.IgnoreCase);
            List<List<string>> list = new List<List<string>>();
            List<string> trlist = GetInnerText(html, "tr");
            if (trlist.Count == 0) return new List<List<string>>();
            trlist.RemoveAt(0);
            foreach (string tr1 in trlist)
            {
                string tr = tr1.Replace("\r\n", "");
                List<string> tdList = new List<string>();
                List<string> colors = new List<string>();
                string th = regex2.Match(tr).Groups[0].Value;
                th = Regex.Replace(th, @"</?th[^>]*>", "");//替换th
                th = Regex.Replace(th, @"</?div[^>]*>", "");//替换div
                th = Regex.Replace(th, @"</?a[^>]*>", "");//替换span
                th = th.Replace("<span class=\"", "[");
                th = th.Replace("\"></span>", "]");
                th = th.Replace("<span>", "").Replace("</span>", "");
                th = th.Replace("&nbsp;", "");
                tdList.Add(th);
                List<string> tds = GetInnerText(tr, "td");
                if (th == "外观颜色" || th == "内饰颜色")
                {
                    colors.Add(th + "码");
                }
                foreach (string inner in tds)
                {    
                    string td = inner;
                    if (th == "外观颜色" || th == "内饰颜色")
                    {
                        List<string> lilist = GetInnerText(td, "li");
                        StringBuilder sbr = new StringBuilder();
                        StringBuilder colorsbr = new StringBuilder();
                        foreach (string lic in lilist)
                        {
                            string li = lic;
                            sbr.Append(GetFirstPropertyValue(li, "title"));
                            sbr.Append(";");
                            List<string> colorlist = GetPropertyValue(li, " style=");//获得颜色码
                            StringBuilder codesbr = new StringBuilder();
                            foreach (string code in colorlist)
                            {
                                codesbr.Append(", u'#");
                                string color = code.Replace("background-color: rgb(", "").Replace(");", "").Replace(" ", "");
                                color = color.Replace("background-color:#", "").Replace(");", "").Replace(" ", "");
                                codesbr.Append(color);
                                //string[] arr = color.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                //foreach (string rgb in arr)
                                //{
                                //    int d = Convert.ToInt32(rgb);
                                //    codesbr.Append(Convert.ToString(d, 16).ToUpper().PadLeft(2, '0'));
                                //}
                                codesbr.Append("'");
                            }
                            if (colorlist.Count > 0)
                            {
                                codesbr.Remove(0, 2);
                                codesbr.Insert(0, "[");
                                codesbr.Append("];");
                            }
                            colorsbr.Append(codesbr);
                        }
                        td = sbr.ToString();
                        tdList.Add(td);
                        colors.Add(colorsbr.ToString());
                    }
                    else
                    {

                        int n = td.IndexOf("class=\"pop pop02 fn-hide\"", 0);
                        if (n >= 0)
                        {
                            td = td.Remove(n);
                            n = td.LastIndexOf("<div");
                            if(n>=0) td = td.Remove(n);
                        }
                        td = Regex.Replace(td, @"</?div[^>]*>", "");//替换div
                        td = Regex.Replace(td, @"</?p[^>]*>", "");//替换p
                        td = Regex.Replace(td, @"</?i[^>]*>", "");//替换<i>
                        td = Regex.Replace(td, @"</?a[^>]*>", "");//替换span
                        td = td.Replace("<span class=\"", "[");
                        td = td.Replace("\"></span>", "]");
                        td = td.Replace("&nbsp;", "");
                        tdList.Add(td);
                    }
                }
                list.Add(tdList);
                if (colors.Count > 0)
                {
                    list.Add(colors);
                }
            }
            return list;
        }
        private List<List<string>> GetTable_200(string html1)
        {
            string html = html1.ToLower(); 
            Regex regex1 = new Regex(@"<tr[^>]*>(.*?)</tr>", RegexOptions.Multiline);
            Regex regex2 = new Regex(@"<th[^>]*>(.*?)</th>", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@"<td[^>]*>(.*?)</td>", RegexOptions.IgnoreCase);
            List<List<string>> list = new List<List<string>>();
            List<string> trlist = GetInnerText(html, "tr");
            if (trlist.Count == 0) return new List<List<string>>();
            trlist.RemoveAt(0);
            foreach (string tr1 in trlist)
            {
                string tr = tr1.Replace("\r\n", "");
                if (tr.Contains("colspan=")) continue;
                List<string> tdList = new List<string>();
                List<string> colors = new List<string>();
                string th = regex2.Match(tr).Groups[0].Value;
                th = Regex.Replace(th, @"</?th[^>]*>", "");//替换th
                th = Regex.Replace(th, @"</?div[^>]*>", "");//替换div
                th = Regex.Replace(th, @"</?a[^>]*>", "");//替换span
                th = th.Replace("<span class=\"", "[");
                th = th.Replace("\"></span>", "]");
                th = th.Replace("<span>", "").Replace("</span>", "");
                th = th.Replace("&nbsp;", "");
                tdList.Add(th);
                List<string> tds = GetInnerText(tr, "td");
                if (th == "外观颜色" || th == "内饰颜色")
                {
                    colors.Add(th + "码");
                }
                foreach (string inner in tds)
                {
                    string td = inner;
                    if (th == "外观颜色" || th == "内饰颜色")
                    {
                        List<string> lilist = GetInnerText(td, "li");
                        StringBuilder sbr = new StringBuilder();
                        StringBuilder colorsbr = new StringBuilder();
                        foreach (string lic in lilist)
                        {
                            string li = lic;
                            sbr.Append(GetFirstPropertyValue(li, "title"));
                            sbr.Append(";");
                            List<string> colorlist = GetPropertyValue(li, " style=");//获得颜色码
                            StringBuilder codesbr = new StringBuilder();
                            foreach (string code in colorlist)
                            {
                                codesbr.Append(", u'#");
                                string color = code.Replace("background-color: rgb(", "").Replace(");", "").Replace(" ", "");
                                color = color.Replace("background-color:#", "").Replace(");", "").Replace(" ", "");
                                codesbr.Append(color);
                                //string[] arr = color.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                //foreach (string rgb in arr)
                                //{
                                //    int d = Convert.ToInt32(rgb);
                                //    codesbr.Append(Convert.ToString(d, 16).ToUpper().PadLeft(2, '0'));
                                //}
                                codesbr.Append("'");
                            }
                            if (colorlist.Count > 0)
                            {
                                codesbr.Remove(0, 2);
                                codesbr.Insert(0, "[");
                                codesbr.Append("];");
                            }
                            colorsbr.Append(codesbr);
                        }
                        td = sbr.ToString();
                        tdList.Add(td);
                        colors.Add(colorsbr.ToString());
                    }
                    else
                    {
                        int n = td.IndexOf("class=\"pop pop02 fn-hide\"", 0);
                        if (n >= 0)
                        {
                            td = td.Remove(n);
                            n = td.LastIndexOf("<div");
                            if (n >= 0) td = td.Remove(n);
                        }
                        td = Regex.Replace(td, @"</?div[^>]*>", "");//替换div
                        td = Regex.Replace(td, @"</?p[^>]*>", "");//替换p
                        td = Regex.Replace(td, @"</?i[^>]*>", "");//替换<i>
                        td = Regex.Replace(td, @"</?a[^>]*>", "");//替换span
                        td = td.Replace("<span class=\"", "[");
                        td = td.Replace("\"></span>", "]");
                        td = td.Replace("&nbsp;", "");
                        tdList.Add(td);
                    }
                }
                list.Add(tdList);
                if (colors.Count > 0)
                {
                    list.Add(colors);
                }
            }
            return list;
        }
        #endregion
        #region 太平洋汽车
        private List<string> GetVersionList2(string html)
        {
            List<string> list = new List<string>();
            int st = 0;
            st = html.IndexOf("<DIV class=\"carbox carbox-v2\">");
            while (st >= 0)
            {
                st = html.IndexOf("<A href=", st);
                if (st < 0) break;
                st = html.IndexOf(">", st + 2);
                int ed = html.IndexOf("</A>", st);
                //车型版本
                string str = html.Substring(st + 1, ed - st - 1);
                list.Add(str);
                st = ed + 4;
                st = html.IndexOf("<DIV class=\"carbox carbox-v2\">",st);
            }
            return list;
        }
        private List<string> GetPriceList(string html)
        {
            List<string> list = new List<string>();
            int st = 0;
            st = html.IndexOf("<DIV class=\"carbox carbox-v2\">");
            st = html.IndexOf("<SPAN class=sys_ow", st);
            while (st >= 0)
            {
                if (st < 0) break;
                st = html.IndexOf(">", st + 2);
                int ed = html.IndexOf("</SPAN>", st);
                //车型版本
                string str = html.Substring(st + 1, ed - st - 1).Replace("官方价：", "");
                list.Add(str);
                st = ed + 4;
                st = html.IndexOf("<SPAN class=sys_ow", st);
            }
            return list;
        }
        private List<List<string>> GetConfigList(string html)
        {
            Regex regex1 = new Regex(@"<TR[^>]*>(.*?)</TR>", RegexOptions.Multiline);
            Regex regex2 = new Regex(@"<TH[\S\s]*>(.*?)</TH>", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@"<TD[^>]*>(.*?)</TD>", RegexOptions.IgnoreCase);
            List<List<string>> list = new List<List<string>>();
            List<string> trlist = GetInnerText(html, "TR");
            if (trlist.Count == 0) return new List<List<string>>();
            foreach (string tr in trlist)
            {
                if (tr.IndexOf("colSpan=6") > 0)
                {
                    //首行分类标签
                    continue;
                }
                else if (tr.IndexOf("class=gbicot") > 0)
                {
                    //二行参数看图
                    continue;
                }
                List<string> tdList = new List<string>();
                string th = regex2.Match(tr).Groups[0].Value;
                if(th.IndexOf("<A")>-0)
                {
                    th = GetInnerText(th, "A")[0];
                }
                else
                {
                    th = GetInnerText(th, "DIV")[0];
                }
                tdList.Add(th);//标签
                List<string> tds = GetInnerText(tr, "TD");
                if(tr.IndexOf("colorul")>0)
                {
                    foreach (string inner in tds)
                    {
                        string td = inner;
                        td = GetInnerText(td, "DIV")[0];
                        List<string> liList = GetInnerText(td, "LI");
                        StringBuilder sbr = new StringBuilder();
                        foreach(string li in liList)
                        {
                            int s1 = li.IndexOf("<A title=");
                            if(s1>=0)
                            {
                                int ed1 = li.IndexOf("href");
                                sbr.AppendFormat("{0};", li.Substring(s1 + 9, ed1 - s1 - 9).Trim());
                            }
                            else
                            {
                                s1 = li.IndexOf("<SPAN title=");
                                if (s1 >= 0)
                                {
                                    int ed1 = li.IndexOf(">",s1+12);
                                    sbr.AppendFormat("{0};", li.Substring(s1 + 12, ed1 - s1 - 12).Trim());
                                }
                            }
                        }
                        tdList.Add(sbr.ToString());
                    }
                }
                else
                {
                    foreach (string inner in tds)
                    {
                        string td = inner;
                        td = GetInnerText(td, "DIV")[0];
                        td = RemoveElement(td, "STRONG");
                        td = RemoveElement(td, "A");
                        td = td.Replace("&nbsp;", "");
                        td = td.Replace("&amp;", "&");
                        tdList.Add(td);
                    }
                }
                list.Add(tdList);
            }
            return list;
        }

        /// <summary>
        /// 匹配汽车之家标签
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private string MathAutoHomeHeader(string header)
        {
            string title=header;
            switch (header)
            {
                case "长×宽×高(mm)": title = "长*宽*高(mm)";
                    break;
                case "车身类型": title = "车身结构";
                    break;
                case "车重(kg)": title = "整备质量(kg)";
                    break;
                case "后备厢容积(L)": title = "行李厢容积(L)";
                    break;
                case "排放标准": title = "环保标准";
                    break;
                case "前悬挂类型": title = "前悬架类型";
                    break;
                case "后悬挂类型": title = "后悬架类型";
                    break;
                case "转向助力类型": title = "助力类型";
                    break;
                case "防爆轮胎": title = "零胎压继续行驶";
                    break;
                case "车身稳定控制(ESP/DSC/VSC等)": title = "车身稳定控制(ESC/ESP/DSC等)";
                    break;
                case "可变悬挂": title = "可变悬架";
                    break;
                case "空气悬挂": title = "空气悬架";
                    break;
                case "后备厢感应开启": title = "感应后备厢";
                    break;
                case "方向盘调节范围": title = "方向盘调节";
                    break;
                case "前/后雷达": title = "前/后驻车雷达";
                    break;
                case "前/后座中央扶手": title = "前/后中央扶手";
                    break;
                case "外接音源接口(AUX/USB/iPod等)": title = "外接音源接口";
                    break;
                case "自适应远近光灯": title = "自适应远近光";
                    break;
                case "随动转向大灯(AFS)": title = "转向头灯";
                    break;
                case "外后视镜加热": title = "后视镜加热";
                    break;
                case "空调调节方式": title = "空调控制方式";
                    break;
                case "车身颜色": title = "外观颜色";
                    break;
                case "电池支持最高续航里程(km)": title = "工信部续航里程(km)";
                    break;
                case "电池保修时间": title = "电池组质保";
                    break;
                case "驱动模式": title = "驱动方式";
                    break;
            }
            return title;
        }
        #endregion
        /// <summary>
        /// 获得元素中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        private List<string> GetInnerText(string html,string elem)
        {
            List<string> list = new List<string>();
            int st = 0;
            int ed = 0;
            int l = elem.Length;
            while(true)
            {
                st = html.IndexOf(string.Format("<{0}", elem),st);
                if (st < 0) break;
                st = html.IndexOf(">", st + l + 1);
                ed = html.IndexOf(string.Format("</{0}>", elem), st);
                string inner=html.Substring(st+1, ed - st-1);
                list.Add(inner);
                st = ed + l + 1;
            }
            return list;
        }

        /// <summary>
        /// 去除元素，保留中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        private string RemoveElement(string html, string elem)
        {
            List<string> list = new List<string>();
            int st = 0;
            int ed = 0;
            int l = elem.Length;
            string result=html;
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
        private string GetTableById(string html,string id)
        {
            int st = html.IndexOf(string.Format(" id=\"{0}\"", id),0);
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
        private string GetTableById1(string html, string id,ref int start)
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
            start = ed+8;
            return html.Substring(st + 7, ed - st - 7);
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
            st = html.IndexOf("=", st+property.Length);
            int ed = html.IndexOf(">", st+1);
            int ed1 = html.IndexOf(" ", st + 1);
            if (ed1 < ed) ed = ed1;
            return html.Substring(st + 1, ed - st - 1).Replace("\"","");
        }
        /// <summary>
        /// 根据属性获得值,取多个
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<string> GetPropertyValue(string html, string property)
        {
            List<string> list = new List<string>();
            int st = 0;
            while(true)
            {
                st = html.IndexOf(property, st);
                if (st < 0) break;
                st = html.IndexOf("\"", st + property.Length);
                int ed = html.IndexOf("\"", st + 1);
                list.Add(html.Substring(st + 1, ed - st - 1));
                st = ed+1;
            }
            return list;
        }

        private string ReplaceHeader(string header)
        {
            string content = header;
            foreach(var str in headerKeyDic)
            {
                content = Regex.Replace(content, str.Key, str.Value, RegexOptions.IgnoreCase);
            }
            return content.Replace("</span>","").Replace("</SPAN>","");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtKeyWord.Clear();
        }

        private void btn_GetCarSeries_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => {
                StringBuilder linkSbr = new StringBuilder();
                StringBuilder error = new StringBuilder();
                int i = 0;
                int j =0;
               foreach(string url in txtKeyWord.Lines)
               {
                   if (url.Trim() == "") continue;
                   lblTip.Text = string.Format("进度：{0}", j);
                   j++;
                   string html = RequestHelper.GetHttpWebRequest(url,"gb2312");
                   //html = html.Replace("\\u0027", "'");
                   //html = html.Replace("\\u003e", ">");
                   //html = html.Replace("\\u003c", "<");
                   int st = html.IndexOf("class=\"athm-sub-nav__car__name");
                   if(st>0)
                   {
                       int ed = html.IndexOf("</div>", st);
                       string link = html.Substring(st,ed-st);
                       List<string> carurllist = GetPropertyValue(link, "href");
                       if (carurllist.Count<=2)
                       {
                           string carurl = carurllist[0];
                           string[] args = carurl.Split('/');
                           linkSbr.AppendLine(args.First(lk => lk != ""));
                           i++;
                       }
                       else
                       {
                           error.AppendLine(url);
                       }
                   }
                   else
                   {
                       st = html.IndexOf("class=\"car-skip");
                       if (st >= 0)
                       {
                           while (st >= 0)
                           {
                               int ed = html.IndexOf("</div>", st);
                               string link = html.Substring(st, ed - st);
                               i++;
                               List<string> carurllist = GetPropertyValue(link, "href");
                               if (carurllist.Count == 1)
                               {
                                   string carurl = carurllist[0];
                                   string[] args = carurl.Split('/');
                                   linkSbr.AppendLine(args.First(lk =>lk!="" && !lk.Contains(".")));
                               }
                               else
                               {
                                   i--;
                                   error.AppendLine(url);
                               }
                               st = html.IndexOf("class=\"car-skip", ed);
                           }
                       }
                       else
                       {
                           error.AppendLine(url);
                       }
                   }
               }
               if (error.Length > 0)
               {
                   error.Insert(0, "错误：\r\n");
               }
               if (linkSbr.Length > 0)
               {
                   linkSbr.Insert(0, "正确：\r\n");
               }
               string strDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
               TextHelper.SaveToFile(error.ToString() + linkSbr.ToString(), strDesktopPath+"/车系ID.txt",false);
               lblTip.Text = string.Format("一共：{0}条，采集：{1}条，错误：{2}条", j, i, error.Length);
            });
            thread.Start();
        }

        private void btnAllCar_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                List<char> firstCodelist = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("品牌Url", typeof(string)));
                dt.Columns.Add(new DataColumn("品牌", typeof(string)));
                dt.Columns.Add(new DataColumn("主机厂Url", typeof(string)));
                dt.Columns.Add(new DataColumn("主机厂", typeof(string)));
                dt.Columns.Add(new DataColumn("车系Url", typeof(string)));
                dt.Columns.Add(new DataColumn("车系", typeof(string)));
                int i = 0;
                foreach (char code in firstCodelist)
                {
                    string html = RequestHelper.GetWebClient(string.Format("https://www.autohome.com.cn/grade/carhtml/{0}.html", code), Encoding.Default);
                    List<string> brandList = HtmlHelper.GetInnerTextList(html, "dl");
                    foreach (string brandhtml in brandList)
                    {
                        string dthtml = HtmlHelper.GetFirstInnerText(brandhtml, "dt");
                        string brandurl = HtmlHelper.GetFirstPropertyValue(dthtml, "href");
                        string brand = HtmlHelper.GetInnerText(dthtml, "a", 2);
                        string ddhtml = HtmlHelper.GetFirstInnerText(brandhtml, "dd");
                        List<string> factoryList = HtmlHelper.GetInnerTextList(ddhtml, "class=\"h3-tit", "</ul>");
                        foreach (string factoryHtml in factoryList)
                        {
                            string factoryurl = HtmlHelper.GetFirstPropertyValue(factoryHtml, "href");
                            string factory = HtmlHelper.GetFirstInnerText(factoryHtml, "a");
                            List<string> lilist = HtmlHelper.GetInnerTextList(factoryHtml, "li");
                            foreach (string lihtml in lilist)
                            {
                                if (lihtml == "") continue;
                                string carurl = HtmlHelper.GetFirstPropertyValue(lihtml, "href");
                                string carseries = HtmlHelper.GetFirstInnerText(lihtml, "a");
                                DataRow row = dt.NewRow();
                                row["品牌Url"] = "http:" + brandurl;
                                row["品牌"] = brand;
                                row["主机厂Url"] = "http:" + factoryurl;
                                row["主机厂"] = factory;
                                row["车系Url"] = "http:" + carurl;
                                row["车系"] = carseries;
                                dt.Rows.Add(row);
                                i++;
                                lblTip.Text = string.Format("{0} - {1} - {2}", brand, factory, carseries);
                            }
                        }
                    }
                }
                ExcelHelper.TableToExcel(dt, "D://导出数据/汽车之家车系.xlsx");
                lblTip.Text = "完成！";
            });
            thread.Start();
        }

        private void btnAutoCar_Click(object sender, EventArgs e)
        {
            Thread thread=new Thread(() => {
                
                string filepath = ControlHelper.SelectFile(FileType.Text, "请选择车系文件");
                if (filepath != "")
                {
                    List<string> carseriesids = TextHelper.GetFileTextList(filepath, Encoding.Default);
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("车系Id", typeof(string)));
                    dt.Columns.Add(new DataColumn("年款Id", typeof(string)));
                    dt.Columns.Add(new DataColumn("价格", typeof(string)));
                    dt.Columns.Add(new DataColumn("年款", typeof(string)));
                    int total = carseriesids.Count;
                    int process = 0;
                    foreach (string cid in carseriesids)
                    {
                        if (cid == "") continue;
                        string html = RequestHelper.GetWebClient(string.Format("https://car.autohome.com.cn/mtn/series/{0}#pvareaid=3454444", cid), Encoding.UTF8);
                        //采集“查询保养信息”
                        /*
                        string dlhtml = HtmlHelper.GetInnerText(html, "id=\"dl_spec\"", "</dl>");
                        List<string> linkList = HtmlHelper.GetInnerTextList(dlhtml, "href=\"", "</a>");
                        List<string[]> autoyearList = new List<string[]>();
                        foreach (string linkhtml in linkList)
                        {
                            string yearid = HtmlHelper.GetFirstPropertyValue(linkhtml, "data-key");
                            string autoyear = HtmlHelper.GetFirstPropertyValue(linkhtml, "data-text");
                            string price= HtmlHelper.GetFirstInnerText(linkhtml, "span");
                            DataRow row = dt.NewRow();
                            row["车系Id"] = cid;
                            row["年款Id"] = yearid;
                            row["价格"] = price;
                            row["年款"] = autoyear;
                            dt.Rows.Add(row);
                        }*/
                        #region 采集“查看车型保养周期表”                        
                        string linkshtml = HtmlHelper.GetInnerText(html, "class=\"series-tab-item", "class=\"uibox\"");
                        List<string> linkList = HtmlHelper.GetInnerTextList(linkshtml, "<a ", "</a>");
                        List<string[]> autoyearList = new List<string[]>();
                        foreach (string linkhtml in linkList)
                        {
                            string link = HtmlHelper.GetFirstPropertyValue(linkhtml, "href");
                            string autoyear = HtmlHelper.GetFirstInnerText(linkhtml, "a");
                            DataRow row = dt.NewRow();
                            row["车系Id"] = cid;
                            row["年款Url"] = link == "javascript:void(0);" ? "" : link;
                            row["年款"] = autoyear.Replace("(暂无)", "");
                            dt.Rows.Add(row);
                        }
                        #endregion
                        process++;
                        lblTip.Text = string.Format("总数:{0},进度:{1}", total, process);
                        ExcelHelper.TableToExcel(dt, string.Format("D://导出数据//汽车之家年款-{0}.xlsx", cid));
                        dt.Rows.Clear();
                    }
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void btnMtn_Click(object sender, EventArgs e)
        {
            Label.CheckForIllegalCrossThreadCalls = true;
            string filepath = ControlHelper.SelectFile(FileType.Text, "请选择年款文件");
            if (true) // filepath != "")
            {
                List<string> autoyearidlist =  TextHelper.GetFileTextList(filepath, Encoding.Default);
                autoyearidlist.Remove("");
                int total = autoyearidlist.Count;
                int process = 0;
                foreach(string yid in autoyearidlist)
                {
                    process++;
                    ThreadPool.SetMaxThreads(5, 5);
                    ThreadPool.SetMinThreads(1, 1);
                    // 使用CancellationToken来取消任务  取消任务直接返回
                    using (var cts = new CancellationTokenSource())
                    {
                        CancellationToken token = cts.Token;
                        ThreadPool.QueueUserWorkItem(_ =>CatchMaintenanceInfo(yid,total,process, token));
                    }
                }
            }           

        }

        public void CatchMaintenanceInfo(string yid,int total,int process, CancellationToken token)//定义方法thread2
        {
            string html = RequestHelper.GetWebClient(string.Format("https://car.autohome.com.cn/mtn/series/cycle/{0}", yid), Encoding.UTF8);
            //采集“查询保养信息”
            string tableHtml = HtmlHelper.GetInnerText(html, "id=\"tableFixed\"", "</table>");
            string headerhtml = HtmlHelper.GetFirstInnerText(tableHtml, "thead");
            string tbodyhtml = HtmlHelper.GetFirstInnerText(tableHtml, "tbody");
            List <string> tdlist= HtmlHelper.GetInnerTextList(headerhtml, "td");
            List<string> columnList = new List<string>();
            DataTable dt = new DataTable();
            foreach(string tdhtml in tdlist)
            {
                string text = HtmlHelper.GetFirstInnerText(tdhtml, "div");
                if(text.IndexOf("<p")>-1)
                {
                    List<string> plist = HtmlHelper.GetInnerTextList(text, "p");
                    text = $"{plist[0]}({plist[1]})";
                }
                dt.Columns.Add(new DataColumn (text,typeof(string)));
            }

            List<string> trList = HtmlHelper.GetInnerTextList(tbodyhtml, "tr");
            foreach(string tr in trList)
            {
                DataRow row=dt.NewRow();
                List<string> tdlist2 = HtmlHelper.GetInnerTextList(tr, "td");
                int i = 0;
                foreach(string td in tdlist2)
                {
                    if(i==0)
                    {
                        string inner = HtmlHelper.GetFirstInnerText(td, "div");
                        if(inner.IndexOf("<a")>-1)
                        {
                            inner = HtmlHelper.GetFirstInnerText(inner, "a");
                        }
                        row[0] = inner;
                    }
                    else
                    {
                        string inner = HtmlHelper.GetFirstInnerText(td, "p");
                        row[i] = inner.Replace("&nbsp;","");
                    }
                    i++;
                }
                dt.Rows.Add(row);
            }
            ExcelHelper.TableToExcel(dt, $"D://导出数据/保养{yid}.xls");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            SetText(string.Format("总数：{0},进度：{1}", total, process));
        }

        delegate void SafeSetText(string strMsg);
        private void SetText(string strMsg)
        {
            SafeSetText objSet = delegate (string str)
            {
                lblTip.Text = str;
            };
            lblTip.Invoke(objSet, new object[] { strMsg });
        }
    }
}

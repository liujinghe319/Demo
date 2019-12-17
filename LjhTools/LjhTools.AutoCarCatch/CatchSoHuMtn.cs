using LjhTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LjhTools.AutoCarCatch
{
    public partial class CatchSoHuMtn : Form
    {
        List<string> carSeriesList = new List<string>();
        public CatchSoHuMtn()
        {
            InitializeComponent();
            Label.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnMtn_Click(object sender, EventArgs e)
        {
            string filepath = ControlHelper.SelectFile(FileType.Text);
            if (!string.IsNullOrEmpty(filepath))
            {
                carSeriesList = TextHelper.GetFileTextList(filepath, Encoding.Default);
                Thread thread = new Thread(() => {
                    int total = carSeriesList.Count;
                    int process = 0;
                    int error = 0;
                    foreach(string id in carSeriesList)
                    {
                        if(id.Trim()!="")
                        {
                            string link = string.Format("http://db.auto.sohu.com/model_{0}/Maintenance.shtml", id.Trim());
                            string html = RequestHelper.GetHtmlCode(link, Encoding.UTF8);
                            string daohang = HtmlHelper.GetInnerText(html,"class=\"daohang","</span>");
                            List<string> daohanglist = HtmlHelper.GetInnerTextList(daohang, "a");
                            int l = daohanglist.Count;
                            string brand = daohanglist[l - 3];
                            string factory = daohanglist[l - 2];
                            string carseries = daohanglist[l -1];
                            string yearhtml = "";
                            try
                            {
                                yearhtml=HtmlHelper.GetInnerText(html, "id=\"modelid", "</ul");
                                yearhtml = yearhtml.Substring(yearhtml.IndexOf("ul"));
                            }
                            catch(Exception ex)
                            {
                                process++;
                                TextHelper.SaveToFile(id+"\r\n","D://导出数据//error.txt", true);
                                error++;
                                continue;
                            }
                            List<string> yidlist = HtmlHelper.GetPropertyValue(yearhtml, "id");
                            List<string> yearlist = HtmlHelper.GetPropertyValue(yearhtml, "data-year");
                            List<string> lilist = HtmlHelper.GetInnerTextList(yearhtml, "li");
                            Dictionary<string, string> yearDic = new Dictionary<string, string>();
                            for(int i=0;i<yidlist.Count;i++)
                            {
                                yearDic.Add(yidlist[i], string.Format("{0}款 {1}",yearlist[i],lilist[i].Trim()));
                            }
                            string maintenance=HtmlHelper.GetInnerText(html,"id=\"type_tb_L","id=\"special");
                            foreach(var car in yearDic)
                            {
                                string cardiv=HtmlHelper.GetInnerText(maintenance,string.Format("id=\"{0}_L",car.Key),"div");
                                string tablehtml = HtmlHelper.GetInnerText(cardiv, "class=\"tabel1", "</table>");
                                List<string> trlist = HtmlHelper.GetInnerTextList(tablehtml, "tr");
                                List<List<string>> tdList = new List<List<string>>();
                                DataTable dt = new DataTable();
                                dt.Columns.Add(new DataColumn("品牌", typeof(string)));
                                dt.Columns.Add(new DataColumn("主机厂", typeof(string)));
                                dt.Columns.Add(new DataColumn("车系", typeof(string)));
                                dt.Columns.Add(new DataColumn("年款", typeof(string)));                               
                                for(int j=0;j<trlist.Count;j++)
                                {
                                    List<string> tds = HtmlHelper.GetInnerTextList(trlist[j], j==0?"th":"td");
                                    tdList.Add(tds);
                                    dt.Columns.Add(new DataColumn(tds[0],typeof(string)));
                                }
                                for (int i = 1; i < tdList[0].Count-1;i++ )
                                {
                                    DataRow row = dt.NewRow();
                                    row["品牌"] = brand;
                                    row["主机厂"] = factory;
                                    row["车系"] = carseries;
                                    row["年款"] = car.Value;
                                    for (int j=0;j<tdList.Count;j++)
                                    {
                                        row[4+j] = tdList[j][i].Replace("<br />", "");
                                    }
                                    dt.Rows.Add(row);
                                }
                                string filename=string.Format("{0}-{1}-{2}-{3}", brand, factory, carseries, car.Value);
                                if(filename.Contains("\\"))
                                {
                                    filename=filename.Replace("\\", "&");
                                }
                                if (filename.Contains("/"))
                                {
                                    filename=filename.Replace("/", "&");
                                }
                                ExcelHelper.TableToExcel(dt, string.Format("D://导出数据//{0}.xlsx", filename));
                            }
                            process++;
                            lblTip.Text = string.Format("总数：{0}，进度：{1},错误：{2}", total, process,error);
                            Thread.Sleep(800);
                        }
                    }                    
                });
                thread.Start();
            }
        }
    }
}

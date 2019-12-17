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
    public partial class CatchQuanna : Form
    {
        Dictionary<string, string> columnDic = new Dictionary<string, string> { 
        {"款式名称","车型名称"},
        {"出厂时间","上市时间"},
        {"排放标准","环保标准"},
        {"车体形式","车身结构"},
        {"长/宽/高(mm)","长*宽*高(mm)"},
        {"最大总质量(kg)","最大载重质量(kg)"},
        {"行李箱容积(l)","行李厢容积(L)"},
        {"车门数(含后车门)","车门数(个)"},
        {"乘员数(含驾驶员)","座位数(个)"},
        {"发动机重要技术","发动机特有技术"},
        {"发动机描述","发动机"},
        {"升功率(kw/l)","最大功率(kW)"},
        {"综合油耗(L/100km)","工信部综合油耗(L/100km)"},
        {"工信部综合油耗(L)","工信部综合油耗(L/100km)"},
        {"每缸气门数","每缸气门数(个)"},
        {"最大功率(kw(ps)/rpm)","最大功率转速(rpm)"},
        {"最大扭矩(n.m/rpm)","最大扭矩(N·m)"},
        {"燃料类型标号","燃油标号"},
        {"排气量(ml)","排量(mL)"},
        {"汽缸容积(cc)","排量(mL)"},
        {"进气方式","进气形式"},
        {"变速器形式","变速箱"},
        {"转向系统","整体主动转向系统"},
        {"前悬架","前悬架类型"},
        {"后悬架","后悬架类型"},
        {"前制动","前制动器类型"},
        {"后制动","后制动器类型"},
        {"变速器名称","变速箱类型"},
        {"0-100km/h加速时间(s)","官方0-100km/h加速(s)"},
        {"保修期","整车质保"},
        {"100km/h-0制动距离(m)","实测100-0km/h制动(m)"}};
        public CatchQuanna()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private Thread th;
        private void button3_Click(object sender, EventArgs e)
        {
            th = new Thread(() =>
            {
                if (!string.IsNullOrEmpty(textBox1.Text.Trim()))
                {
                    catchQuanna();
                }
                else
                {
                    MessageBox.Show("url不能为空");
                }
            }
                   );
            th.Start();
        }
        private void catchQuanna()
        {
            int count = 0;
            string[] urls = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
            for (int i = 0; i < urls.Length; i++)
            {
                if (Regex.IsMatch(urls[i], "http://.+"))
                {
                    string pageHtml = downloadHtml(urls[i].Trim(),Encoding.UTF8);

                    string carbrand = "请手动填写";
                    string carseries = "请手动填写";
                    MatchCollection mc1 = Regex.Matches(pageHtml, "汽车档案</a>.+?<a.+?>(.+?)品牌</a>.+?<a.+?>(.+?)系列</a>");
                    if (mc1 != null && mc1.Count > 0)
                    {
                        carbrand = mc1[0].Groups[1].ToString();
                        carbrand = Regex.Replace(carbrand, "\\s+", "");

                        carseries = mc1[0].Groups[2].ToString();
                        carseries = Regex.Replace(carseries, "\\s+", "");
                    }

                    int indexStart = pageHtml.IndexOf("<table id=\'table1\'");
                    if (indexStart > -1)
                    {
                        pageHtml = pageHtml.Substring(indexStart, pageHtml.IndexOf("</tr></table></div><script") - indexStart + 13);
                        pageHtml = pageHtml.Substring(pageHtml.IndexOf("</tr>") + 5, pageHtml.LastIndexOf("</tr>") - pageHtml.IndexOf("</tr>") - 5);
                        string[] trs = pageHtml.Split(new string[] { "</tr>" }, StringSplitOptions.RemoveEmptyEntries);
                        Dictionary<string, List<string>> tableDic = new Dictionary<string, List<string>>();
                        foreach(string tr in trs)
                        {
                            if (tr.IndexOf("td class='lm_left'") > 0)
                            {
                                continue;
                            }
                            List<string> tdList = GetTdInnerText(tr);
                            string title = tdList[0];
                            do
                            {
                                tdList.Remove(title);
                            }
                            while (tdList.Contains(title));
                            if (title != "车型对比：")
                            {
                                if (title.EndsWith("："))
                                {
                                    title=title.Replace("：", "");
                                }
                                tableDic.Add(title, tdList);
                            }
                        }


                        DataTable newdt = toTable(tableDic, carbrand, carseries);
                        DataTable newdt2 = toTableForAuto(tableDic, carbrand, carseries);
                        if (newdt.Rows.Count > 0)
                        {
                            count++;
                            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string path = desktop + "\\全纳网";
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            Random r = new Random();
                            string filename = DateTime.Now.ToString("yyyy-MM-ddhhmmss") + r.Next(0, 100) + "全纳网：品牌：" + carbrand + "车型：" + carseries + ".xls";
                            ExcelHelper.Export(newdt, "", path + "\\原表" + filename);
                            ExcelHelper.Export(newdt2, "", path + "\\处理表" + filename);
                        }
                        newdt.Clear();
                        newdt2.Clear();
                    }
                }
            }
            label4.Text = "已保存到桌面(共" + count + "个有效链接)";
        }

        /// <summary>
        /// 获得tr中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        private List<string> GetTdInnerText(string html)
        {
            List<string> list = new List<string>();
            int st = 0;
            int ed = 0;
            while (true)
            {
                st = html.IndexOf("<td",ed);
                if (st < 0) break;
                st = html.IndexOf(">", st + 2);
                ed = html.IndexOf("</td>", st);
                string inner = html.Substring(st + 1, ed - st - 1);
                ed = ed + 3;
                if(inner.IndexOf("<a",0)>-1)
                {
                    inner = GetFirstInnerText(inner, "a");
                }
                list.Add(inner);
            }
            return list;
        }

        /// <summary>
        /// 获得第一个元素中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        private string GetFirstInnerText(string html, string elem)
        {
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
                return inner;
            }
            return "";
        }



        private DataTable toTable(Dictionary<string, List<string>> tableDic, string carbrand, string carseries)
        {
            DataTable newdt = new DataTable();
            newdt.Columns.Add(new DataColumn("品牌", typeof(string)));
            newdt.Columns.Add(new DataColumn("车系", typeof(string)));
            foreach (var obj in tableDic)
            {
                newdt.Columns.Add(new DataColumn(obj.Key, typeof(string)));
            }
            int n = tableDic.Values.First().Count();
            for (int j = 0; j < n; j++)
            {
                DataRow row = newdt.NewRow();
                row["品牌"] = carbrand;
                row["车系"] = carseries;
                foreach (var obj in tableDic)
                {
                    row[obj.Key] = tableDic[obj.Key][j];
                }
                newdt.Rows.Add(row);
            }
            return newdt;
        }

        private DataTable toTableForAuto(Dictionary<string,List<string>> tableDic,string carbrand,string carseries)
        {
            if (tableDic == null)
                return null;
            DataTable newdt = new DataTable();

            #region MyRegion

            newdt.Columns.Add("首字母", typeof(string));
            newdt.Columns.Add("品牌", typeof(string));
            newdt.Columns.Add("车系", typeof(string));
            newdt.Columns.Add("销售状态", typeof(string));
            newdt.Columns.Add("Logo", typeof(string));
            newdt.Columns.Add("图片路径", typeof(string));
            newdt.Columns.Add("车型名称", typeof(string));
            newdt.Columns.Add("年款", typeof(string));
            newdt.Columns.Add("厂商指导价(元)", typeof(string));
            newdt.Columns.Add("排量", typeof(string));
            newdt.Columns.Add("厂商", typeof(string));
            newdt.Columns.Add("级别", typeof(string));
            newdt.Columns.Add("发动机", typeof(string));
            newdt.Columns.Add("变速箱", typeof(string));
            newdt.Columns.Add("长*宽*高(mm)", typeof(string));
            newdt.Columns.Add("车身结构", typeof(string));
            newdt.Columns.Add("最高车速(km/h)", typeof(string));
            newdt.Columns.Add("官方0-100km/h加速(s)", typeof(string));
            newdt.Columns.Add("实测0-100km/h加速(s)", typeof(string));
            newdt.Columns.Add("实测100-0km/h制动(m)", typeof(string));
            newdt.Columns.Add("实测油耗(L/100km)", typeof(string));
            newdt.Columns.Add("工信部综合油耗(L/100km)", typeof(string));
            newdt.Columns.Add("实测离地间隙(mm)", typeof(string));
            newdt.Columns.Add("整车质保", typeof(string));
            newdt.Columns.Add("长度(mm)", typeof(string));
            newdt.Columns.Add("宽度(mm)", typeof(string));
            newdt.Columns.Add("高度(mm)", typeof(string));
            newdt.Columns.Add("轴距(mm)", typeof(string));
            newdt.Columns.Add("前轮距(mm)", typeof(string));
            newdt.Columns.Add("后轮距(mm)", typeof(string));
            newdt.Columns.Add("最小离地间隙(mm)", typeof(string));
            newdt.Columns.Add("整备质量(kg)", typeof(string));
            newdt.Columns.Add("车门数(个)", typeof(string));
            newdt.Columns.Add("座位数(个)", typeof(string));
            newdt.Columns.Add("油箱容积(L)", typeof(string));
            newdt.Columns.Add("行李厢容积(L)", typeof(string));
            newdt.Columns.Add("发动机型号", typeof(string));
            newdt.Columns.Add("排量(mL)", typeof(string));
            newdt.Columns.Add("排量(L)", typeof(string));
            newdt.Columns.Add("进气形式", typeof(string));
            newdt.Columns.Add("气缸排列形式", typeof(string));
            newdt.Columns.Add("气缸数(个)", typeof(string));
            newdt.Columns.Add("每缸气门数(个)", typeof(string));
            newdt.Columns.Add("压缩比", typeof(string));
            newdt.Columns.Add("配气机构", typeof(string));
            newdt.Columns.Add("缸径(mm)", typeof(string));
            newdt.Columns.Add("行程(mm)", typeof(string));
            newdt.Columns.Add("最大马力(Ps)", typeof(string));
            newdt.Columns.Add("最大功率(kW)", typeof(string));
            newdt.Columns.Add("最大功率转速(rpm)", typeof(string));
            newdt.Columns.Add("最大扭矩(N·m)", typeof(string));
            newdt.Columns.Add("最大扭矩转速(rpm)", typeof(string));
            newdt.Columns.Add("发动机特有技术", typeof(string));
            newdt.Columns.Add("燃料形式", typeof(string));
            newdt.Columns.Add("燃油标号", typeof(string));
            newdt.Columns.Add("供油方式", typeof(string));
            newdt.Columns.Add("缸盖材料", typeof(string));
            newdt.Columns.Add("缸体材料", typeof(string));
            newdt.Columns.Add("环保标准", typeof(string));
            newdt.Columns.Add("简称", typeof(string));
            newdt.Columns.Add("挡位个数", typeof(string));
            newdt.Columns.Add("变速箱类型", typeof(string));
            newdt.Columns.Add("驱动方式", typeof(string));
            newdt.Columns.Add("四驱形式", typeof(string));
            newdt.Columns.Add("中央差速器结构", typeof(string));
            newdt.Columns.Add("前悬架类型", typeof(string));
            newdt.Columns.Add("后悬架类型", typeof(string));
            newdt.Columns.Add("助力类型", typeof(string));
            newdt.Columns.Add("车体结构", typeof(string));
            newdt.Columns.Add("前制动器类型", typeof(string));
            newdt.Columns.Add("后制动器类型", typeof(string));
            newdt.Columns.Add("驻车制动类型", typeof(string));
            newdt.Columns.Add("前轮胎规格", typeof(string));
            newdt.Columns.Add("后轮胎规格", typeof(string));
            newdt.Columns.Add("备胎规格", typeof(string));
            newdt.Columns.Add("主/副驾驶座安全气囊", typeof(string));
            newdt.Columns.Add("前/后排侧气囊", typeof(string));
            newdt.Columns.Add("前/后排头部气囊(气帘)", typeof(string));
            newdt.Columns.Add("膝部气囊", typeof(string));
            newdt.Columns.Add("胎压监测装置", typeof(string));
            newdt.Columns.Add("零胎压继续行驶", typeof(string));
            newdt.Columns.Add("安全带未系提示", typeof(string));
            newdt.Columns.Add("ISOFIX儿童座椅接口", typeof(string));
            newdt.Columns.Add("发动机电子防盗", typeof(string));
            newdt.Columns.Add("车内中控锁", typeof(string));
            newdt.Columns.Add("无钥匙启动系统", typeof(string));
            newdt.Columns.Add("无钥匙进入系统", typeof(string));
            newdt.Columns.Add("ABS防抱死", typeof(string));
            newdt.Columns.Add("制动力分配(EBD/CBC等)", typeof(string));
            newdt.Columns.Add("刹车辅助(EBA/BAS/BA等)", typeof(string));
            newdt.Columns.Add("牵引力控制(ASR/TCS/TRC等)", typeof(string));
            newdt.Columns.Add("车身稳定控制(ESC/ESP/DSC等)", typeof(string));
            newdt.Columns.Add("上坡辅助", typeof(string));
            newdt.Columns.Add("自动驻车", typeof(string));
            newdt.Columns.Add("陡坡缓降", typeof(string));
            newdt.Columns.Add("可变悬架", typeof(string));
            newdt.Columns.Add("空气悬架", typeof(string));
            newdt.Columns.Add("可变转向比", typeof(string));
            newdt.Columns.Add("前桥限滑差速器/差速锁", typeof(string));
            newdt.Columns.Add("中央差速器锁止功能", typeof(string));
            newdt.Columns.Add("后桥限滑差速器/差速锁", typeof(string));
            newdt.Columns.Add("电动天窗", typeof(string));
            newdt.Columns.Add("全景天窗", typeof(string));
            newdt.Columns.Add("运动外观套件", typeof(string));
            newdt.Columns.Add("铝合金轮圈", typeof(string));
            newdt.Columns.Add("电动吸合门", typeof(string));
            newdt.Columns.Add("侧滑门", typeof(string));
            newdt.Columns.Add("电动后备厢", typeof(string));
            newdt.Columns.Add("感应后备厢", typeof(string));
            newdt.Columns.Add("车顶行李架", typeof(string));
            newdt.Columns.Add("真皮方向盘", typeof(string));
            newdt.Columns.Add("方向盘调节", typeof(string));
            newdt.Columns.Add("方向盘电动调节", typeof(string));
            newdt.Columns.Add("多功能方向盘", typeof(string));
            newdt.Columns.Add("方向盘换挡", typeof(string));
            newdt.Columns.Add("方向盘加热", typeof(string));
            newdt.Columns.Add("方向盘记忆", typeof(string));
            newdt.Columns.Add("定速巡航", typeof(string));
            newdt.Columns.Add("前/后驻车雷达", typeof(string));
            newdt.Columns.Add("倒车视频影像", typeof(string));
            newdt.Columns.Add("行车电脑显示屏", typeof(string));
            newdt.Columns.Add("全液晶仪表盘", typeof(string));
            newdt.Columns.Add("HUD抬头数字显示", typeof(string));
            newdt.Columns.Add("座椅材质", typeof(string));
            newdt.Columns.Add("运动风格座椅", typeof(string));
            newdt.Columns.Add("座椅高低调节", typeof(string));
            newdt.Columns.Add("腰部支撑调节", typeof(string));
            newdt.Columns.Add("肩部支撑调节", typeof(string));
            newdt.Columns.Add("主/副驾驶座电动调节", typeof(string));
            newdt.Columns.Add("第二排靠背角度调节", typeof(string));
            newdt.Columns.Add("第二排座椅移动", typeof(string));
            newdt.Columns.Add("后排座椅电动调节", typeof(string));
            newdt.Columns.Add("电动座椅记忆", typeof(string));
            newdt.Columns.Add("前/后排座椅加热", typeof(string));
            newdt.Columns.Add("前/后排座椅通风", typeof(string));
            newdt.Columns.Add("前/后排座椅按摩", typeof(string));
            newdt.Columns.Add("第三排座椅", typeof(string));
            newdt.Columns.Add("后排座椅放倒方式", typeof(string));
            newdt.Columns.Add("前/后中央扶手", typeof(string));
            newdt.Columns.Add("后排杯架", typeof(string));
            newdt.Columns.Add("GPS导航系统", typeof(string));
            newdt.Columns.Add("定位互动服务", typeof(string));
            newdt.Columns.Add("中控台彩色大屏", typeof(string));
            newdt.Columns.Add("蓝牙/车载电话", typeof(string));
            newdt.Columns.Add("车载电视", typeof(string));
            newdt.Columns.Add("后排液晶屏", typeof(string));
            newdt.Columns.Add("220V/230V电源", typeof(string));
            newdt.Columns.Add("外接音源接口", typeof(string));
            newdt.Columns.Add("CD支持MP3/WMA", typeof(string));
            newdt.Columns.Add("多媒体系统", typeof(string));
            newdt.Columns.Add("扬声器品牌", typeof(string));
            newdt.Columns.Add("扬声器数量", typeof(string));
            newdt.Columns.Add("近光灯", typeof(string));
            newdt.Columns.Add("远光灯", typeof(string));
            newdt.Columns.Add("日间行车灯", typeof(string));
            newdt.Columns.Add("自适应远近光", typeof(string));
            newdt.Columns.Add("自动头灯", typeof(string));
            newdt.Columns.Add("转向辅助灯", typeof(string));
            newdt.Columns.Add("转向头灯", typeof(string));
            newdt.Columns.Add("前雾灯", typeof(string));
            newdt.Columns.Add("大灯高度可调", typeof(string));
            newdt.Columns.Add("大灯清洗装置", typeof(string));
            newdt.Columns.Add("车内氛围灯", typeof(string));
            newdt.Columns.Add("前/后电动车窗", typeof(string));
            newdt.Columns.Add("车窗防夹手功能", typeof(string));
            newdt.Columns.Add("防紫外线/隔热玻璃", typeof(string));
            newdt.Columns.Add("后视镜电动调节", typeof(string));
            newdt.Columns.Add("后视镜加热", typeof(string));
            newdt.Columns.Add("内/外后视镜自动防眩目", typeof(string));
            newdt.Columns.Add("后视镜电动折叠", typeof(string));
            newdt.Columns.Add("后视镜记忆", typeof(string));
            newdt.Columns.Add("后风挡遮阳帘", typeof(string));
            newdt.Columns.Add("后排侧遮阳帘", typeof(string));
            newdt.Columns.Add("后排侧隐私玻璃", typeof(string));
            newdt.Columns.Add("遮阳板化妆镜", typeof(string));
            newdt.Columns.Add("后雨刷", typeof(string));
            newdt.Columns.Add("感应雨刷", typeof(string));
            newdt.Columns.Add("空调控制方式", typeof(string));
            newdt.Columns.Add("后排独立空调", typeof(string));
            newdt.Columns.Add("后座出风口", typeof(string));
            newdt.Columns.Add("温度分区控制", typeof(string));
            newdt.Columns.Add("车内空气调节/花粉过滤", typeof(string));
            newdt.Columns.Add("车载冰箱", typeof(string));
            newdt.Columns.Add("自动泊车入位", typeof(string));
            newdt.Columns.Add("发动机启停技术", typeof(string));
            newdt.Columns.Add("并线辅助", typeof(string));
            newdt.Columns.Add("车道偏离预警系统", typeof(string));
            newdt.Columns.Add("主动刹车/主动安全系统", typeof(string));
            newdt.Columns.Add("整体主动转向系统", typeof(string));
            newdt.Columns.Add("夜视系统", typeof(string));
            newdt.Columns.Add("中控液晶屏分屏显示", typeof(string));
            newdt.Columns.Add("自适应巡航", typeof(string));
            newdt.Columns.Add("全景摄像头", typeof(string));
            newdt.Columns.Add("外观颜色", typeof(string));
            newdt.Columns.Add("外观颜色码", typeof(string));
            newdt.Columns.Add("内饰颜色", typeof(string));
            newdt.Columns.Add("内饰颜色码", typeof(string));
            newdt.Columns.Add("后排车门开启方式", typeof(string));
            newdt.Columns.Add("货箱尺寸(mm)", typeof(string));
            newdt.Columns.Add("最大载重质量(kg)", typeof(string));
            newdt.Columns.Add("电动机总功率(kW)", typeof(string));
            newdt.Columns.Add("电动机总扭矩(N·m)", typeof(string));
            newdt.Columns.Add("前电动机最大功率(kW)", typeof(string));
            newdt.Columns.Add("前电动机最大扭矩(N·m)", typeof(string));
            newdt.Columns.Add("后电动机最大功率(kW)", typeof(string));
            newdt.Columns.Add("后电动机最大扭矩(N·m)", typeof(string));
            newdt.Columns.Add("工信部续航里程(km)", typeof(string));
            newdt.Columns.Add("电池容量(kWh)", typeof(string));
            newdt.Columns.Add("电池组质保", typeof(string));
            newdt.Columns.Add("电池充电时间", typeof(string));
            newdt.Columns.Add("充电桩价格", typeof(string));
            newdt.Columns.Add("VinCode", typeof(string));
            newdt.Columns.Add("PartsYearId", typeof(string));


            #endregion
            int n = tableDic.Values.First().Count();
            for (int i = 0; i < n; i++)
            {
                DataRow dr = newdt.NewRow();

                #region MyRegion

                dr["首字母"] = "";
                dr["品牌"] = carbrand;
                dr["车系"] = carseries;
                dr["销售状态"] = "";
                dr["Logo"] = "";
                dr["图片路径"] = "";
                dr["车型名称"] = tableDic.ContainsKey("车型名称") ? tableDic["车型名称"][i] : "";
                dr["年款"] = "";
                dr["厂商指导价(元)"] = tableDic.ContainsKey("商家指导价格") ? tableDic["商家指导价格"][i] : "";
                dr["排量"] = tableDic.ContainsKey("排量L") ? tableDic["排量L"][i] : "";
                dr["厂商"] = tableDic.ContainsKey("厂商") ? tableDic["厂商"][i] : "";
                dr["级别"] = tableDic.ContainsKey("级别") ? tableDic["级别"][i] : "";
                dr["发动机"] = tableDic.ContainsKey("发动机") ? tableDic["发动机"][i] : "";
                dr["变速箱"] = tableDic.ContainsKey("变速箱") ? tableDic["变速箱"][i] : "";
                dr["长*宽*高(mm)"] = tableDic.ContainsKey("长×宽×高(mm)") ? tableDic["长×宽×高(mm)"][i] : "";
                dr["车身结构"] = tableDic.ContainsKey("车身结构") ? tableDic["车身结构"][i] : "";
                dr["最高车速(km/h)"] = tableDic.ContainsKey("最高车速(km/h)") ? tableDic["最高车速(km/h)"][i] : "";
                dr["官方0-100km/h加速(s)"] = tableDic.ContainsKey("官方0-100加速(s)") ? tableDic["官方0-100加速(s)"][i] : "";
                dr["实测0-100km/h加速(s)"] = "";
                dr["实测100-0km/h制动(m)"] = tableDic.ContainsKey("官方100-0制动(m)") ? tableDic["官方100-0制动(m)"][i] : "";
                dr["实测油耗(L/100km)"] = tableDic.ContainsKey("官方综合路况油耗(L)") ? tableDic["官方综合路况油耗(L)"][i] : "";
                dr["工信部综合油耗(L/100km)"] = tableDic.ContainsKey("工信部综合油耗(L)") ? tableDic["工信部综合油耗(L)"][i] : "";
                dr["实测离地间隙(mm)"] = tableDic.ContainsKey("离地间隙(mm)") ? tableDic["离地间隙(mm)"][i] : "";
                dr["整车质保"] = "";
                dr["长度(mm)"] = tableDic.ContainsKey("长度(mm)") ? tableDic["长度(mm)"][i] : "";
                dr["宽度(mm)"] = tableDic.ContainsKey("宽度(mm)") ? tableDic["宽度(mm)"][i] : "";
                dr["高度(mm)"] = tableDic.ContainsKey("高度(mm)") ? tableDic["高度(mm)"][i] : "";
                dr["轴距(mm)"] = tableDic.ContainsKey("轴距(mm)") ? tableDic["轴距(mm)"][i] : "";
                dr["前轮距(mm)"] = tableDic.ContainsKey("前轮距(mm)") ? tableDic["前轮距(mm)"][i] : "";
                dr["后轮距(mm)"] = tableDic.ContainsKey("后轮距(mm)") ? tableDic["后轮距(mm)"][i] : "";
                dr["最小离地间隙(mm)"] = tableDic.ContainsKey("离地间隙(mm)") ? tableDic["离地间隙(mm)"][i] : "";
                dr["整备质量(kg)"] = "";
                dr["车门数(个)"] = tableDic.ContainsKey("车门数(个)") ? tableDic["车门数(个)"][i] : "";
                dr["座位数(个)"] = tableDic.ContainsKey("座位数(个)") ? tableDic["座位数(个)"][i] : "";
                dr["油箱容积(L)"] = tableDic.ContainsKey("油箱容积(L)") ? tableDic["油箱容积(L)"][i] : "";
                dr["行李厢容积(L)"] = tableDic.ContainsKey("行李箱容积(L)") ? tableDic["行李箱容积(L)"][i] : "";
                dr["发动机型号"] = "";
                dr["排量(mL)"] = "";
                dr["排量(L)"] = tableDic.ContainsKey("排量(L)") ? tableDic["排量(L)"][i] : "";
                dr["进气形式"] = tableDic.ContainsKey("工作方式") ? tableDic["工作方式"][i] : "";
                dr["气缸排列形式"] = tableDic.ContainsKey("气缸排列型式") ? tableDic["气缸排列型式"][i] : "";
                dr["气缸数(个)"] = tableDic.ContainsKey("汽缸数(个)") ? tableDic["汽缸数(个)"][i] : "";
                dr["每缸气门数(个)"] = tableDic.ContainsKey("每缸气门数(个)") ? tableDic["每缸气门数(个)"][i] : "";
                dr["压缩比"] = tableDic.ContainsKey("压缩比") ? tableDic["压缩比"][i] : "";
                dr["配气机构"] = tableDic.ContainsKey("气门结构") ? tableDic["气门结构"][i] : "";
                dr["缸径(mm)"] = tableDic.ContainsKey("缸径") ? tableDic["缸径"][i] : "";
                dr["行程(mm)"] = "";
                dr["最大马力(Ps)"] = tableDic.ContainsKey("最大马力(Ps)") ? tableDic["最大马力(Ps)"][i] : "";
                dr["最大功率(kW)"] = tableDic.ContainsKey("最大功率(Kw)") ? tableDic["最大功率(Kw)"][i] : "";
                dr["最大功率转速(rpm)"] = tableDic.ContainsKey("最大功率转速(rpm)") ? tableDic["最大功率转速(rpm)"][i] : "";
                dr["最大扭矩(N·m)"] = tableDic.ContainsKey("最大扭距(Nm)") ? tableDic["最大扭距(Nm)"][i] : "";
                dr["最大扭矩转速(rpm)"] = tableDic.ContainsKey("最大扭距转速(rpm)") ? tableDic["最大扭距转速(rpm)"][i] : "";
                dr["发动机特有技术"] = tableDic.ContainsKey("发动机特有技术") ? tableDic["发动机特有技术"][i] : "";
                dr["燃料形式"] = tableDic.ContainsKey("燃油") ? tableDic["燃油"][i] : "";
                dr["燃油标号"] = tableDic.ContainsKey("燃油标号") ? tableDic["燃油标号"][i] : "";
                dr["供油方式"] = tableDic.ContainsKey("供油方式") ? tableDic["供油方式"][i] : "";
                dr["缸盖材料"] = tableDic.ContainsKey("缸盖材料") ? tableDic["缸盖材料"][i] : "";
                dr["缸体材料"] = tableDic.ContainsKey("缸体材料") ? tableDic["缸体材料"][i] : "";
                dr["环保标准"] = tableDic.ContainsKey("环保标准") ? tableDic["环保标准"][i] : "";
                dr["简称"] = "";
                dr["挡位个数"] = tableDic.ContainsKey("档位个数") ? tableDic["档位个数"][i] : "";
                dr["变速箱类型"] = tableDic.ContainsKey("变速箱类型") ? tableDic["变速箱类型"][i] : "";
                dr["驱动方式"] = tableDic.ContainsKey("驱动方式") ? tableDic["驱动方式"][i] : "";
                dr["四驱形式"] = tableDic.ContainsKey("驱动方式") ? tableDic["驱动方式"][i] : "";
                dr["中央差速器结构"] = tableDic.ContainsKey("中央差速器锁止功能") ? tableDic["中央差速器锁止功能"][i] : "";
                dr["前悬架类型"] = tableDic.ContainsKey("前悬挂类型") ? tableDic["前悬挂类型"][i] : "";
                dr["后悬架类型"] = tableDic.ContainsKey("后悬挂类型") ? tableDic["后悬挂类型"][i] : "";
                dr["助力类型"] = tableDic.ContainsKey("助力类型") ? tableDic["助力类型"][i] : "";
                dr["车体结构"] = tableDic.ContainsKey("车体结构") ? tableDic["车体结构"][i] : "";
                dr["前制动器类型"] = tableDic.ContainsKey("前制动器类型") ? tableDic["前制动器类型"][i] : "";
                dr["后制动器类型"] = tableDic.ContainsKey("后制动器类型") ? tableDic["后制动器类型"][i] : "";
                dr["驻车制动类型"] = tableDic.ContainsKey("自动驻车制动系统") ? tableDic["自动驻车制动系统"][i] : "";
                dr["前轮胎规格"] = tableDic.ContainsKey("前轮胎规格") ? tableDic["前轮胎规格"][i] : "";
                dr["后轮胎规格"] = tableDic.ContainsKey("后轮胎规格") ? tableDic["后轮胎规格"][i] : "";
                dr["备胎规格"] = tableDic.ContainsKey("备胎") ? tableDic["备胎"][i] : "";
                dr["主/副驾驶座安全气囊"] = "主" + (tableDic.ContainsKey("驾驶座气囊") ? tableDic["驾驶座气囊"][i] : "-") + " / 副" + (tableDic.ContainsKey("副驾驶气囊") ? tableDic["副驾驶气囊"][i] : "-");
                dr["前/后排侧气囊"] = "前" + (tableDic.ContainsKey("前排侧气囊") ? tableDic["前排侧气囊"][i] : "-") + " / 后" + (tableDic.ContainsKey("后排侧气囊") ? tableDic["后排侧气囊"][i] : "-");
                dr["前/后排头部气囊(气帘)"] ="前" + (tableDic.ContainsKey("前头部气囊帘") ? tableDic["前头部气囊帘"][i] : "-") + " / 后" + (tableDic.ContainsKey("后头部气囊帘") ? tableDic["后头部气囊帘"][i] : "-");
                dr["膝部气囊"] =tableDic.ContainsKey("膝部气囊") ? tableDic["膝部气囊"][i] : "";
                dr["胎压监测装置"] = tableDic.ContainsKey("胎压监测装置") ? tableDic["胎压监测装置"][i] : "";
                dr["零胎压继续行驶"] = tableDic.ContainsKey("零胎压继续行驶") ? tableDic["零胎压继续行驶"][i] : "";
                dr["安全带未系提示"] = tableDic.ContainsKey("安全带未系提示") ? tableDic["安全带未系提示"][i] : "";
                dr["ISOFIX儿童座椅接口"] = tableDic.ContainsKey("ISO FIX儿童座椅接口") ? tableDic["ISO FIX儿童座椅接口"][i] : "";
                dr["发动机电子防盗"] = "";
                dr["车内中控锁"] = tableDic.ContainsKey("车内中控锁") ? tableDic["车内中控锁"][i] : "";
                dr["无钥匙启动系统"] = tableDic.ContainsKey("无钥匙启动系统") ? tableDic["无钥匙启动系统"][i] : "";
                dr["无钥匙进入系统"] = "";
                dr["ABS防抱死"] = tableDic.ContainsKey("ABS防抱死") ? tableDic["ABS防抱死"][i] : "";
                dr["制动力分配(EBD/CBC等)"] = tableDic.ContainsKey("制动力分配(EBD)") ? tableDic["制动力分配(EBD)"][i] : "";
                dr["刹车辅助(EBA/BAS/BA等)"] = tableDic.ContainsKey("刹车辅助(EBA / BAS / BA)") ? tableDic["刹车辅助(EBA / BAS / BA)"][i] : "";
                dr["牵引力控制(ASR/TCS/TRC等)"] =tableDic.ContainsKey("牵引力控制(ASR/TCS/TRC/ATC)") ? tableDic["牵引力控制(ASR/TCS/TRC/ATC)"][i] : "";
                dr["车身稳定控制(ESC/ESP/DSC等)"] = tableDic.ContainsKey("车身稳定控制(ESP/DSC/VSC)") ? tableDic["车身稳定控制(ESP/DSC/VSC)"][i] : "";
                dr["上坡辅助"] = "";
                dr["自动驻车"] =tableDic.ContainsKey("泊车辅助") ? tableDic["泊车辅助"][i] : "";
                dr["陡坡缓降"] = tableDic.ContainsKey("陡坡缓升/降") ? tableDic["陡坡缓升/降"][i] : "";
                dr["可变悬架"] =tableDic.ContainsKey("可调悬挂") ? tableDic["可调悬挂"][i] : "";
                dr["空气悬架"] = tableDic.ContainsKey("升降(空气)悬挂") ? tableDic["升降(空气)悬挂"][i] : "";
                dr["可变转向比"] =tableDic.ContainsKey("可变转向比") ? tableDic["可变转向比"][i] : "";
                dr["前桥限滑差速器/差速锁"] = tableDic.ContainsKey("前桥限滑差速器/差速锁") ? tableDic["前桥限滑差速器/差速锁"][i] : "";
                dr["中央差速器锁止功能"] =tableDic.ContainsKey("中央差速器锁止功能") ? tableDic["中央差速器锁止功能"][i] : "";
                dr["后桥限滑差速器/差速锁"] = tableDic.ContainsKey("后桥限滑差速器/差速锁") ? tableDic["后桥限滑差速器/差速锁"][i] : "";
                dr["电动天窗"] =tableDic.ContainsKey("电动天窗") ? tableDic["电动天窗"][i] : "";
                dr["全景天窗"] =tableDic.ContainsKey("全景天窗") ? tableDic["全景天窗"][i] : "";
                dr["运动外观套件"] =tableDic.ContainsKey("运动版包围") ? tableDic["运动版包围"][i] : "";
                dr["铝合金轮圈"] =tableDic.ContainsKey("铝合金轮毂") ? tableDic["铝合金轮毂"][i] : "";
                dr["电动吸合门"] =tableDic.ContainsKey("电动吸合门") ? tableDic["电动吸合门"][i] : "";
                dr["侧滑门"] = "";
                dr["电动后备厢"] =tableDic.ContainsKey("电动后备厢") ? tableDic["电动后备厢"][i] : "";
                dr["感应后备厢"] =tableDic.ContainsKey("电动后备厢") ? tableDic["电动后备厢"][i] : "";
                dr["车顶行李架"] =tableDic.ContainsKey("行李架") ? tableDic["行李架"][i] : "";
                dr["真皮方向盘"] =tableDic.ContainsKey("真皮方向盘") ? tableDic["真皮方向盘"][i] : "";
                dr["方向盘调节"] = (tableDic.ContainsKey("方向盘上下调节")&& tableDic["方向盘上下调节"][i]== "●" ? "上下调节" : "") + (tableDic.ContainsKey("方向盘前后调节")&& tableDic["方向盘前后调节"][i]== "●" ? "前后调节" : "");
                dr["方向盘电动调节"] = tableDic.ContainsKey("方向盘电动调节") ? tableDic["方向盘电动调节"][i] : "";
                dr["多功能方向盘"] = tableDic.ContainsKey("多功能方向盘") ? tableDic["多功能方向盘"][i] : "";
                dr["方向盘换挡"] = tableDic.ContainsKey("方向盘换档") ? tableDic["方向盘换档"][i] : "";
                dr["方向盘加热"] = "";
                dr["方向盘记忆"] = "";
                dr["定速巡航"] = tableDic.ContainsKey("定速巡航") ? tableDic["定速巡航"][i] : "";
                dr["前/后驻车雷达"] = "";
                dr["倒车视频影像"] =tableDic.ContainsKey("倒车影像") ? tableDic["倒车影像"][i] : "";
                dr["行车电脑显示屏"] = tableDic.ContainsKey("多功能显示屏") ? tableDic["多功能显示屏"][i] : "";
                dr["全液晶仪表盘"] = "";
                dr["HUD抬头数字显示"] = tableDic.ContainsKey("HUD数字显示") ? tableDic["HUD数字显示"][i] : "";
                dr["座椅材质"] = tableDic.ContainsKey("真皮座椅")&& tableDic["真皮座椅"][i]== "●" ? "真皮" : "";
                dr["运动风格座椅"] = tableDic.ContainsKey("运动座椅") ? tableDic["运动座椅"][i] : "";
                dr["座椅高低调节"] = tableDic.ContainsKey("座椅高低调节") ? tableDic["座椅高低调节"][i] : "";
                dr["腰部支撑调节"] = tableDic.ContainsKey("腰部支撑调节") ? tableDic["腰部支撑调节"][i] : "";
                dr["肩部支撑调节"] = tableDic.ContainsKey("肩部支撑调节") ? tableDic["肩部支撑调节"][i] : "";
                dr["主/副驾驶座电动调节"] = "";
                dr["第二排靠背角度调节"] = tableDic.ContainsKey("第二排靠背角度调节") ? tableDic["第二排靠背角度调节"][i] : "";
                dr["第二排座椅移动"] = tableDic.ContainsKey("第二排座椅移动") ? tableDic["第二排座椅移动"][i] : "";
                dr["后排座椅电动调节"] = tableDic.ContainsKey("后排座椅电动调节") ? tableDic["后排座椅电动调节"][i] : "";
                dr["电动座椅记忆"] = tableDic.ContainsKey("电动座椅记忆") ? tableDic["电动座椅记忆"][i] : "";

                dr["前/后排座椅加热"] = tableDic.ContainsKey("座椅加热") ? tableDic["座椅加热"][i] : "";
                dr["前/后排座椅通风"] = tableDic.ContainsKey("座椅按摩/通风") ? tableDic["座椅按摩/通风"][i] : "";
                dr["前/后排座椅按摩"] = tableDic.ContainsKey("座椅按摩/通风") ? tableDic["座椅按摩/通风"][i] : "";
                dr["第三排座椅"] = tableDic.ContainsKey("第三排座椅") ? tableDic["第三排座椅"][i] : "";
                if (tableDic.ContainsKey("后座椅整体放倒")&&tableDic["后座椅整体放倒"][i] == "●") { dr["后排座椅放倒方式"] = "整体放倒"; }
                else if (tableDic.ContainsKey("后座按比例放倒")&&tableDic["后座按比例放倒"][i]== "●") { dr["后排座椅放倒方式"] = "比例放倒"; }
                else { dr["后排座椅放倒方式"] = ""; }
                dr["前/后中央扶手"] = "前" +(tableDic.ContainsKey("前座中央扶手") ? tableDic["前座中央扶手"][i] : "-") + " / 后" +(tableDic.ContainsKey("后坐中央扶手") ? tableDic["后坐中央扶手"][i] : "-");
                dr["后排杯架"] = tableDic.ContainsKey("后排杯架") ? tableDic["后排杯架"][i] : "";
                dr["GPS导航系统"] = tableDic.ContainsKey("GPS导航系统") ? tableDic["GPS导航系统"][i] : "";
                dr["定位互动服务"] = tableDic.ContainsKey("定位互动服务") ? tableDic["定位互动服务"][i] : "";
                dr["中控台彩色大屏"] = tableDic.ContainsKey("中控台液晶屏") ? tableDic["中控台液晶屏"][i] : "";

                dr["蓝牙/车载电话"] = (tableDic.ContainsKey("蓝牙系统") ? tableDic["蓝牙系统"][i] : "-") + " / " + (tableDic.ContainsKey("车载电话") ? tableDic["车载电话"][i] : "-");
                dr["车载电视"] = tableDic.ContainsKey("车载电视") ? tableDic["车载电视"][i] : "";
                dr["后排液晶屏"] = tableDic.ContainsKey("后排液晶屏") ? tableDic["后排液晶屏"][i] : "";
                dr["220V/230V电源"] = "";
                dr["外接音源接口"] = tableDic.ContainsKey("外接音源输入(AUX、USB、iPod)") ? tableDic["外接音源输入(AUX、USB、iPod)"][i] : "";
                dr["CD支持MP3/WMA"] = tableDic.ContainsKey("支持MP3、WMA") ? tableDic["支持MP3、WMA"][i] : "";
                dr["多媒体系统"] = tableDic.ContainsKey("多媒体控制系统") ? tableDic["多媒体控制系统"][i] : "";
                dr["扬声器品牌"] = "";

                if (tableDic.ContainsKey("2-3扬声器系统")&&tableDic["2-3扬声器系统"][i] == "●") { dr["扬声器数量"] = "2-3喇叭"; }
                else if (tableDic.ContainsKey("4-5扬声器系统")&&tableDic["4-5扬声器系统"][i]== "●") { dr["扬声器数量"] = "4-5喇叭"; }
                else if (tableDic.ContainsKey("6-7扬声器系统")&&tableDic["6-7扬声器系统"][i]== "●") { dr["扬声器数量"] = "6-7喇叭"; }
                else if (tableDic.ContainsKey("8以上扬声器")&&tableDic["8以上扬声器"][i]== "●") { dr["扬声器数量"] = "≥8喇叭"; }
                dr["近光灯"] = "";
                dr["远光灯"] = "";
                dr["日间行车灯"] = tableDic.ContainsKey("日间行车灯") ? tableDic["日间行车灯"][i] : "";
                dr["自适应远近光"] = "";
                dr["自动头灯"] = tableDic.ContainsKey("自动头灯") ? tableDic["自动头灯"][i] : "";
                dr["转向辅助灯"] = "";
                dr["转向头灯"] = tableDic.ContainsKey("转向头灯") ? tableDic["转向头灯"][i] : "";
                dr["前雾灯"] = tableDic.ContainsKey("前雾灯") ? tableDic["前雾灯"][i] : "";
                dr["大灯高度可调"] = tableDic.ContainsKey("大灯高度可调") ? tableDic["大灯高度可调"][i] : "";
                dr["大灯清洗装置"] = tableDic.ContainsKey("大灯清洗装置") ? tableDic["大灯清洗装置"][i] : "";
                dr["车内氛围灯"] = tableDic.ContainsKey("车内氛围灯") ? tableDic["车内氛围灯"][i] : "";
                dr["前/后电动车窗"] = "前" + (tableDic.ContainsKey("前电动车窗")?tableDic["前电动车窗"][i]:"-") + " / 后" + (tableDic.ContainsKey("后电动车窗")?tableDic["后电动车窗"][i]:"-");

                dr["车窗防夹手功能"] = tableDic.ContainsKey("防夹手功能") ? tableDic["防夹手功能"][i] : "";
                dr["防紫外线/隔热玻璃"] = tableDic.ContainsKey("隔热玻璃") ? tableDic["隔热玻璃"][i] : "";
                dr["后视镜电动调节"] = tableDic.ContainsKey("电动后视镜") ? tableDic["电动后视镜"][i] : "";
                dr["后视镜加热"] = tableDic.ContainsKey("后视镜加热") ? tableDic["后视镜加热"][i] : "";
                dr["内/外后视镜自动防眩目"] = "";
                dr["后视镜电动折叠"] = tableDic.ContainsKey("后视镜电动折叠") ? tableDic["后视镜电动折叠"][i] : "";
                dr["后视镜记忆"] = tableDic.ContainsKey("后视镜记忆") ? tableDic["后视镜记忆"][i] : "";
                dr["后风挡遮阳帘"] = "";
                dr["后排侧遮阳帘"] = tableDic.ContainsKey("后排侧遮阳帘") ? tableDic["后排侧遮阳帘"][i] : "";
                dr["后排侧隐私玻璃"] = "";
                dr["遮阳板化妆镜"] = tableDic.ContainsKey("遮阳板化妆镜") ? tableDic["遮阳板化妆镜"][i] : "";
                dr["后雨刷"] = "";
                dr["感应雨刷"] = tableDic.ContainsKey("感应雨刷") ? tableDic["感应雨刷"][i] : "";

                if (tableDic.ContainsKey("手动空调") && tableDic["手动空调"][i] == "●") { dr["空调控制方式"] = "自动"; }
                else if (tableDic.ContainsKey("自动空调")&&tableDic["自动空调"][i] == "●") { dr["空调控制方式"] = "手动"; }
                else { dr["空调控制方式"] = ""; }

                dr["后排独立空调"] = tableDic.ContainsKey("后排独立空调") ? tableDic["后排独立空调"][i] : "";
                dr["后座出风口"] = tableDic.ContainsKey("后座出风口") ? tableDic["后座出风口"][i] : "";
                dr["温度分区控制"] = tableDic.ContainsKey("温度分区控制") ? tableDic["温度分区控制"][i] : "";
                dr["车内空气调节/花粉过滤"] = tableDic.ContainsKey("车内空气调节") ? tableDic["车内空气调节"][i] : "";
                dr["车载冰箱"] = tableDic.ContainsKey("车载冰箱") ? tableDic["车载冰箱"][i] : "";
                dr["自动泊车入位"] = tableDic.ContainsKey("自动泊车入位") ? tableDic["自动泊车入位"][i] : "";
                dr["发动机启停技术"] = "";
                dr["并线辅助"] = tableDic.ContainsKey("并线辅助") ? tableDic["并线辅助"][i] : "";
                dr["车道偏离预警系统"] = "";
                dr["主动刹车/主动安全系统"] = tableDic.ContainsKey("主动刹车/主动安全系统") ? tableDic["主动刹车/主动安全系统"][i] : "";
                dr["整体主动转向系统"] = tableDic.ContainsKey("整体主动转向系统") ? tableDic["整体主动转向系统"][i] : "";
                dr["夜视系统"] = tableDic.ContainsKey("夜视系统") ? tableDic["夜视系统"][i] : "";
                dr["中控液晶屏分屏显示"] = tableDic.ContainsKey("中控液晶屏分屏显示") ? tableDic["中控液晶屏分屏显示"][i] : "";
                dr["自适应巡航"] = tableDic.ContainsKey("自适应巡航") ? tableDic["自适应巡航"][i] : "";
                dr["全景摄像头"] = tableDic.ContainsKey("全景摄像头") ? tableDic["全景摄像头"][i] : "";
                dr["外观颜色"] = "";
                dr["外观颜色码"] = "";
                dr["内饰颜色"] = "";
                dr["内饰颜色码"] = "";
                dr["后排车门开启方式"] = "";
                dr["货箱尺寸(mm)"] = "";
                dr["最大载重质量(kg)"] = "";
                dr["电动机总功率(kW)"] = "";
                dr["电动机总扭矩(N·m)"] = "";
                dr["前电动机最大功率(kW)"] = "";
                dr["前电动机最大扭矩(N·m)"] = "";
                dr["后电动机最大功率(kW)"] = "";
                dr["后电动机最大扭矩(N·m)"] = "";
                dr["工信部续航里程(km)"] = "";
                dr["电池容量(kWh)"] = "";
                dr["电池组质保"] = "";
                dr["电池充电时间"] = "";
                dr["充电桩价格"] = "";
                dr["VinCode"] = "";
                dr["PartsYearId"] = "";

                #endregion

                newdt.Rows.Add(dr);
            }
            return newdt;
        }

        private string downloadHtml(string url,Encoding encode)
        {
            WebClient wc = new WebClient();
            wc.Encoding = encode;
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

        private void savefile(string[] arr, string filename, StringBuilder sbr)
        {
            string path = string.Join("\\", arr);
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + path;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText(path + "\\" + filename, sbr.ToString());
        }

        private void CatchQuanna2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (th != null)
            {
                th.Abort();
                Application.ExitThread();
                Application.Exit();
            }
        }

        private void btnCatch2_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(() =>
            {
                if (!string.IsNullOrEmpty(textBox1.Text.Trim()))
                {
                    catchFirstCar();
                }
                else
                {
                    MessageBox.Show("url不能为空");
                }
            });
            th.Start();
        }

        private void catchFirstCar()
        {
            List<Dictionary<string, string>> contentList = new List<Dictionary<string, string>>();
            List<string> columnList = new List<string>();
            int count = 0;
            List<string> urls = textBox1.Lines.ToList();
            urls.Remove("");           
            for (int i = 0; i < urls.Count; i++)
            {              
                if (Regex.IsMatch(urls[i], "http[s]?://.+"))
                {
                    Dictionary<string, string> contentDic = new Dictionary<string, string>();
                    string pageHtml = downloadHtml(urls[i].Trim(),Encoding.Default);
                    int st = pageHtml.IndexOf("<div id=\"bread\"", 0);
                    int ed = pageHtml.IndexOf("</div>", st);
                    string captionHtml = pageHtml.Substring(st, ed - st + 6);
                    captionHtml=captionHtml.Replace("您所在的位置：","").Trim();
                    captionHtml=RemoveElementTag(captionHtml,"div");
                    captionHtml=RemoveElementTag(captionHtml,"a");
                    string[] titles = captionHtml.Split(new string[]{"&gt;&gt;"},StringSplitOptions.RemoveEmptyEntries);
                    string factory = titles[2].Trim();//主机厂
                    string carseries = titles[3].Trim();//车型
                    string autocar = titles[4].Trim();//年款
                    st = pageHtml.IndexOf("<div class=\"jbxx\"", ed);
                    ed = pageHtml.IndexOf("<div class=\"gxhpz\"", st);
                    string content = pageHtml.Substring(st, ed - st);
                    List<string> trList = GetInnerTextList(content, "tr");
                    foreach(string tr in trList)
                    {
                        List<string> tdList = GetInnerTextList(tr, "td");
                        for (int j = 0; j < tdList.Count / 2;j++)
                        {
                            string str1 = tdList[j*2].Replace("&nbsp;", "").Trim();
                            string str2= tdList[j*2+1].Replace("&nbsp;", "").Trim();
                            if (!columnList.Contains(str1))
                            {
                                columnList.Add(str1);
                            }
                            if (contentDic.ContainsKey(str1))
                            {
                                contentDic[str1]=str2;
                            }
                            else
                            {
                                contentDic.Add(str1, str2);
                            }
                        }
                    }
                    contentList.Add(contentDic);
                }
            }
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("首字母", typeof(string)));
            dt.Columns.Add(new DataColumn("品牌", typeof(string)));
            foreach(string header in columnList)
            {
                string column = header;
                if(columnDic.ContainsKey(header))
                {
                    column = columnDic[header];
                }
                dt.Columns.Add(new DataColumn(column, typeof(string)));
            }
            dt.Columns.Add(new DataColumn("前轮距(mm)", typeof(string)));
            dt.Columns.Add(new DataColumn("后轮距(mm)", typeof(string)));
            dt.Columns.Add(new DataColumn("铝合金轮圈", typeof(string)));

            foreach(Dictionary<string,string> dic in contentList)
            {
                DataRow row = dt.NewRow();
                row["首字母"] = "手动填写";
                row["品牌"] = "手动填写";
                foreach(var obj in dic)
                {
                    if (columnDic.ContainsKey(obj.Key))
                    {
                        row[columnDic[obj.Key]] = obj.Value;
                        if(obj.Key=="款式名称")
                        {
                            if(dic.ContainsKey("车型年款")&&dic["车型年款"]!="")
                            {
                                row[columnDic[obj.Key]] +="("+ dic["车型年款"] + ")";
                            }
                        }
                    }
                    else if (obj.Key=="前/后轮距(mm)")
                    {
                        if(obj.Value.Trim()!=""&&obj.Value.Contains("/"))
                        {
                            string[] arr = obj.Value.Split('/');
                            row["前轮距(mm)"] = arr[0];
                            row["后轮距(mm)"] = arr[1];
                        }
                        row[obj.Key] = obj.Value;
                    }
                    else if(obj.Key=="发动机重要技术")
                    {
                        row[obj.Key] = obj.Value;
                        if(dic.ContainsKey("燃油供给方式"))
                        {
                            row[obj.Key] += dic["燃油供给方式"];
                        }
                    }
                    else if (obj.Key == "驱动方式")
                    {
                        row[obj.Key] = obj.Value;
                        if (dic.ContainsKey("发动机放置位置"))
                        {
                            row[obj.Key] = dic["发动机放置位置"] + row[obj.Key];
                        }
                    }
                    else if (obj.Key == "前轮毂材料")
                    {
                        if(obj.Value.Contains("铝"))
                        {
                            row["铝合金轮圈"] = "●";
                        }
                        row[obj.Key] = obj.Value;
                    }
                    else
                    {
                        row[obj.Key] = obj.Value;
                    }
                }
                dt.Rows.Add(row);
            }
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + string.Format("第一车网{0}.xls",DateTime.Now.ToString("yyyyMMddHHmmss"));
            ExcelHelper.Export(dt, "", path);
            label4.Text = "已保存到桌面(共" + urls.Count + "个有效链接)";
        }

        /// <summary>
        /// 获得元素中间的内容
        /// </summary>
        /// <param name="html">源文件</param>
        /// <param name="elem">标签</param>
        /// <param name="n">次数</param>
        /// <returns></returns>
        private List<string> GetInnerTextList(string html, string elem, int n = 9999)
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
                int ed2 = html.IndexOf(string.Format("</{0}>", elem), st);
                ed = html.IndexOf(string.Format("<{0}", elem), st);
                if(ed>=0 && ed<ed2)
                {
                        string inner = html.Substring(st + 1, ed - st - 1);
                        inner = inner.Replace(string.Format("</{0}>", elem), "");
                        list.Add(inner);
                        if (list.Count == n) break;
                }
                else if (ed2<0&&ed < 0)
                {
                    ed = html.LastIndexOf(">");
                    string inner = html.Substring(st + 1, ed - st);
                    inner = inner.Replace(string.Format("</{0}>", elem), "");
                    list.Add(inner);
                    break;
                }
                else if(ed2>=0)
                {
                    ed = ed2;
                    string inner = html.Substring(st + 1, ed - st-1);
                    list.Add(inner);
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
        private string GetInnerText2(string html, string elem)
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
        /// 获得元素中间的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        private List<string> GetInnerText(string html, string elem)
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
                ed = result.IndexOf(">", st + 1);
                result = result.Remove(st, ed - st + 1);
                st = ed + 1;
            }
            result = result.Replace(string.Format("</{0}>", elem), "");
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
                result = result.Remove(st, ed - st + len + 3);
                st = ed + 1;
            }
            return result;
        }
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace LjhTools.Utils
{
    public static class RequestHelper
    {
        /// <summary>
        /// 获得post请求后响应的数据
        /// </summary>
        /// <param name="postUrl">请求地址</param>
        /// <param name="referUrl">请求引用地址</param>
        /// <param name="data">请求带的数据</param>
        /// <returns>响应内容</returns>
        public static string PostLogin(string postUrl, string referUrl, string data)
        {
            //string postdata = "productDetail=productDetail&productDetail%3AproductDetailTabPanel-value=productDetail%3AproductDetailCompareTab&javax.faces.ViewState=-1496666384768135624%3A1125828418258458165&javax.faces.source=productDetail%3Aj_idt368&javax.faces.partial.execute=productDetail%3Aj_idt368%20%40component&javax.faces.partial.render=%40component&name=%E9%80%9A%E7%94%A8%E6%B1%BD%E8%BD%A6%20%2F%20GENERAL%20MOTORS%20(%E8%BF%9B%E5%8F%A3)&org.richfaces.ajax.component=productDetail%3Aj_idt368&productDetail%3Aj_idt368=productDetail%3Aj_idt368&rfExt=null&AJAX%3AEVENTS_COUNT=1&javax.faces.partial.ajax=true";
            string result = "";
            try
            {
                //命名空间System.Net下的HttpWebRequest类
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
                //参照浏览器的请求报文 封装需要的参数 这里参照ie9
                //浏览器可接受的MIME类型
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //request.Accept = "text/plain, */*; q=0.01";
                //包含一个URL，用户从该URL代表的页面出发访问当前请求的页面
                request.Referer = referUrl;
                //浏览器类型，如果Servlet返回的内容与浏览器类型有关则该值非常有用
                //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                //请求方式
                request.Method = "POST";
                //是否保持常连接
                //request.KeepAlive = false;
                request.KeepAlive = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.CookieContainer = new CookieContainer();
                //CookieCollection coll = new CookieCollection();
                //coll.Add(new Cookie("anonymousid", "86ceaf8a-2aaa-4496-9086-71a74136715e", "/", "catalog.mann-filter.com"));
                //coll.Add(new Cookie("JSESSIONID", "BDC28B62B4FE761F6A372870966CE70D", "/", "catalog.mann-filter.com"));
                //coll.Add(new Cookie("MannHummelSSL", "571571323.1.1868255656.596359776", "/", "catalog.mann-filter.com"));
                //request.CookieContainer.Add(coll);

                //表示请求消息正文的长度
                //request.ContentLength = data.Length*2;
                Stream postStream = request.GetRequestStream();
                byte[] postData = Encoding.UTF8.GetBytes(data);
                //将传输的数据，请求正文写入请求流
                postStream.Write(postData, 0, postData.Length);
                postStream.Dispose();
                //响应
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //判断响应的信息是否为压缩信息 若为压缩信息解压后返回
                if (response.ContentEncoding == "gzip")
                {
                    MemoryStream ms = new MemoryStream();
                    GZipStream zip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    byte[] buffer = new byte[1024];
                    int l = zip.Read(buffer, 0, buffer.Length);
                    while (l > 0)
                    {
                        ms.Write(buffer, 0, l);
                        l = zip.Read(buffer, 0, buffer.Length);
                    }
                    ms.Dispose();
                    zip.Dispose();
                    result = Encoding.UTF8.GetString(ms.ToArray());
                }
                return result;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
        }
        // 获取网页源码，如果启用了gzip压缩后页面获取会产生乱码，采用此方法可解决gzip压缩而产生的乱码情况
        public static string GetHtmlCode(string url,Encoding encode)
        {
            string htmlCode;
            HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            webRequest.Timeout = 30000;
            webRequest.Method = "GET";
            webRequest.UserAgent = "Mozilla/4.0";
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
            if (webResponse.ContentEncoding.ToLower() == "gzip")//如果使用了GZip则先解压            
            {
                using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                {
                    using (var zipStream =
                        new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                    {
                        using (StreamReader sr = new System.IO.StreamReader(zipStream, encode))
                        {
                            htmlCode = sr.ReadToEnd();
                        }
                    }
                }
            }
            else
            {
                using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive, encode))
                    {
                        htmlCode = sr.ReadToEnd();
                    }
                }
            }

            return htmlCode;
        }
        // WebClient
        public static string GetWebClient(string url, Encoding encoding)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, encoding);
            strHTML = sr.ReadToEnd();
            myStream.Close();
            return strHTML;
        }

        // WebRequest
        public static string GetWebRequest(string url, Encoding encoding)
        {
            Uri uri = new Uri(url);
            WebRequest myReq = WebRequest.Create(uri);
            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, encoding);
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }

        // HttpWebRequest
        public static string GetHttpWebRequest(string url)
        {
            return GetHttpWebRequest(url, "utf-8");
            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
                myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
                myReq.Accept = "*/*";
                myReq.KeepAlive = true;
                myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
                Stream receviceStream = result.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
                string strHTML = readerOfStream.ReadToEnd();
                readerOfStream.Close();
                receviceStream.Close();
                result.Close();

                return strHTML;
            }
            catch (Exception ex)
            {
                throw new Exception("采集指定网址异常，" + ex.Message);
            }
        }
        // HttpWebRequest
        public static string GetHttpWebRequest(string url, string encoding)
        {
            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
                myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
                myReq.Accept = "*/*";
                myReq.KeepAlive = true;
                myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
                Stream receviceStream = result.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding(encoding));
                string strHTML = readerOfStream.ReadToEnd();
                readerOfStream.Close();
                receviceStream.Close();
                result.Close();

                return strHTML;
            }
            catch (Exception ex)
            {
                throw new Exception("采集指定网址异常，" + ex.Message);
            }
        }

        public static string HttpGetMath(string url, string level)
        {
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                request.Headers.Add("X-Current-Language-Code", "zh-CN");

                request.Method = "get";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
                request.Host = "www.trwaftermarket.com";

                WebResponse wr = request.GetResponse();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Encoding encoding = Encoding.GetEncoding(response.CharacterSet);
                //result = GetResponseAsString(response, encoding);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);
                result = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

            }
            catch (Exception)
            {
                //update(string.Format("insert into dc_tinahe_errorurl2(errorurl,level) value('{0}','{1}');", url, level));
                goto aaaa;
            }

        aaaa: return result;
        }

        public static string HttpGetMath(string url, string host, string des)
        {
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");

                request.Method = "get";
                //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
                //request.Referer = "https://webcat.zf.com/nc2_dialog.asp?MODE=&SUCHEN2=&KAT_KZ=P&KAT_KZ=N&KHERNR=121&BJ=&MCODE=&HUBRAUM=&HUBRAUM_10=X&HUBRAUM_10x=10&LEISTUNG=&LEISTUNG_ART=KW&LEISTUNG_10=%23%23&LEISTUNG_10x=10&DBPAGESIZE=0";

                request.CookieContainer = new CookieContainer();
                CookieCollection coll = new CookieCollection();

                coll.Add(new Cookie("NC%5FSPR", "31", "/", "zf.com"));
                coll.Add(new Cookie("ASPSESSIONIDSUBCQQCB", "JHCFFPMAJCHNJHLIKDKACHOJ", "/", "zf.com"));
                coll.Add(new Cookie("ASPSESSIONIDQUDCQSAB", "LBKHDKPAPBOCIHDHNLBIPALI", "/", "zf.com"));
                coll.Add(new Cookie("ASPSESSIONIDQUDCTSAB", "LKPLBFCBHLLFADDIPBPPBBKI", "/", "zf.com"));
                coll.Add(new Cookie("ASPSESSIONIDSWBDQSBB", "IFFPCNEBHMNKJFPNICJEPDCJ", "/", "zf.com"));
                coll.Add(new Cookie("NC%5FKTYPNR%5FHISTORY", "%C2%A71%2433436%2477%248631%24%E4%B8%89%E8%8F%B1%24ASX%241%2E6", "/", "zf.com"));
                coll.Add(new Cookie("NC%5FARTNR%5FHISTORY", "%C2%A732%243000+096+003%24SACHS%24%C2%A732%243151+600+741%24SACHS%24", "/", "zf.com"));
                coll.Add(new Cookie("_et_coid", "01311271b24431b091e10a3ddc22ed3e", "/", "zf.com"));
                coll.Add(new Cookie("ASPSESSIONIDQUABSRCB", "AGPIBIHBAJFMIALPFNPONJCL", "/", "zf.com"));
                coll.Add(new Cookie("ASPSESSIONIDSUDCRQDA", "DGPJPCKBHLBIJNJGNBCPBPPF", "/", "zf.com"));
                coll.Add(new Cookie("ASPSESSIONIDQWBBTTAB", "DMMMNNMBGONNKELGIFBCPPLD", "/", "zf.com"));
                coll.Add(new Cookie("NC%5FLKZ", "TJ", "/", "zf.com"));
                coll.Add(new Cookie("NC%5FEINSPNR%5FTYP", "68%2C32%2C35%2C14%2C294%2C126", "/", "zf.com"));
                coll.Add(new Cookie("NC%5FKATKZ", "p%2Cn", "/", "zf.com"));
                coll.Add(new Cookie("TS01b0590b", "01d76f378614895e81c02b6f4a12399297e1795f69f15a01d0769fc243c953ab0e41542f1024a0093ff773a30782d4703adb06c1074f6c100b7f68cf28bb8ee3b7aa9361d6894c21647140c4490ebe7bf08ac1453d0ed454b62b2b91951673c26d4df9aebde7fc1f3118af021230e1fc6a4377386285e9e1e456defec44a050588d282d2cec6a1b1732126e4298e1a492ab98990cebc68b81e2312bdf7f9e644dc88ebad3285cd80369775b27da02825f9b6e87d40b054f951d6b0e657cdad972d078c48757bd062096f5f3733c6485a6293e272e1e7ea62a0157ab22f30fa914ea946e323052bddd8a9ffe53b4b33431342a05f653968320f4e897c1b9ea8564ca76cefce", "/", "zf.com"));

                request.CookieContainer.Add(coll);
                request.Host = host;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Encoding encoding = Encoding.UTF8;//Encoding.GetEncoding(response.CharacterSet);
                //result = GetResponseAsString(response, encoding);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);
                result = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

            }
            catch (Exception)
            {
                //update(string.Format("insert into dc_tinahe_errorurl2(errorurl,level) value('{0}','{1}');", url, des));
                goto aaaa;
            }

            result = Regex.Replace(result, @">\s+<", "><");
            result = Regex.Replace(result, @"\r\n", "");
            result = Regex.Replace(result, @"\r", "");
            result = Regex.Replace(result, @"\n", "");
            result = Regex.Replace(result, @"\t", "");


        aaaa: return result;
        }

        static CookieCollection pubcoll = new CookieCollection();
        //pagetype请求类型
        public static string GetTextarHtml(string posturl, string refere, string str, string posttype, string setCookis)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            // 准备请求,设置参数  
            request = WebRequest.Create(posturl) as HttpWebRequest;
            request.Method = posttype;//"POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = refere;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");

            request.Host = "textar.brakebook.com";
            request.Connection = "alive";

            if (setCookis == "setCookis")
            {
                request.AllowAutoRedirect = false;
            }

            if (posttype == "POST")
            {
                request.Headers.Add("Tr-XHR-Message", "true");
                request.CookieContainer = new CookieContainer();
                CookieCollection coll = new CookieCollection();

                ////JSESSIONID=0B4AABD947C3BA6574AA8C3320350ABB; mode=12; locale=en_CN; _pk_id.1.e52e=831c5929308342fa.1498208368.1.1498208369.1498208368.; _pk_ses.1.e52e=*
                //coll.Add(new Cookie("JSESSIONID", "0B4AABD947C3BA6574AA8C3320350ABB", "/", "brakebook.com"));
                //coll.Add(new Cookie("locale", "en_CN", "/", "brakebook.com"));
                //coll.Add(new Cookie("mode", "12", "/", "brakebook.com"));
                //coll.Add(new Cookie("_pk_id.1.e52e", "831c5929308342fa.1498208368.1.1498208369.1498208368.", "/", "brakebook.com"));
                //coll.Add(new Cookie("_pk_ses.1.e52e", "*", "/", "brakebook.com"));
                request.CookieContainer.Add(coll);

                byte[] data = Encoding.UTF8.GetBytes(str);
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Flush();
                outstream.Close();
            }
            System.Net.ServicePointManager.Expect100Continue = false;

            string result = "";
            //发送请求并获取相应回应数据  
            response = request.GetResponse() as HttpWebResponse;
            if (response.StatusCode == HttpStatusCode.Found)
            {
                string temp = ""; ;
                string location = response.Headers["Location"];
                string setcookis = response.Headers["Set-Cookie"];
                string[] cookis = setcookis.Split(';');
                CookieCollection newcoll = new CookieCollection();
                for (int i = 0; i < cookis.Length; i++)
                {
                    temp = cookis[i];
                    if (!temp.Contains(',') && !temp.Contains('/'))
                    {
                        string[] pms = temp.Split('=');
                        if (pms.Length == 2)
                        {
                            newcoll.Add(new Cookie(pms[0], pms[1], "/", "brakebook.com"));
                        }
                    }
                }
                pubcoll = newcoll;

                if (location != "")
                {
                    result = GetTextarHtml(location, posturl, str, "POST", "");
                }
            }

            if (response.ContentEncoding == "gzip")
            {
                MemoryStream ms = new MemoryStream();
                GZipStream zip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                byte[] buffer = new byte[1024];
                int l = zip.Read(buffer, 0, buffer.Length);
                while (l > 0)
                {
                    ms.Write(buffer, 0, l);
                    l = zip.Read(buffer, 0, buffer.Length);
                }
                ms.Dispose();
                zip.Dispose();
                result = Encoding.UTF8.GetString(ms.ToArray());
            }
            else
            {
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页(html)代码  
                result = sr.ReadToEnd();
            }
            result = Regex.Replace(result, @">\s+<", "><");
            result = Regex.Replace(result, @"\r\n", "");
            result = Regex.Replace(result, @"\r", "");
            result = Regex.Replace(result, @"\n", "");
            result = Regex.Replace(result, @"\t", "");
            return result;
        }


        public static string GetTextar(string posturl, string refere, string str, string method)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            // 准备请求,设置参数  
            request = WebRequest.Create(posturl) as HttpWebRequest;
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = refere;

            request.CookieContainer = new CookieContainer();
            CookieCollection coll = new CookieCollection();

            coll.Add(new Cookie("JSESSIONID", "B198439DB921BFC3E7BA8E32C6A79698", "/", "brakebook.com"));
            //coll.Add(new Cookie("mode", "12", "/", "brakebook.com"));
            coll.Add(new Cookie("locale", "en_CN", "/", "brakebook.com"));
            coll.Add(new Cookie("_pk_id.1.e52e", "51d8abf9ec8ab1c8.1498211698.3.1501660677.1501643978.", "/", "brakebook.com"));
            coll.Add(new Cookie("_pk_ses.1.e52e", "*", "/", "brakebook.com"));
            System.Net.ServicePointManager.Expect100Continue = false;

            request.CookieContainer.Add(coll);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Tr-XHR-Message", "true");
            request.Host = "textar.brakebook.com";
            request.Connection = "alive";

            if (method == "POST")
            {
                byte[] data = Encoding.UTF8.GetBytes(str);
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Flush();
                outstream.Close();
            }
            //发送请求并获取相应回应数据  

            response = request.GetResponse() as HttpWebResponse;
            string result = "";
            if (response.ContentEncoding == "gzip")
            {
                MemoryStream ms = new MemoryStream();
                GZipStream zip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                byte[] buffer = new byte[1024];
                int l = zip.Read(buffer, 0, buffer.Length);
                while (l > 0)
                {
                    ms.Write(buffer, 0, l);
                    l = zip.Read(buffer, 0, buffer.Length);
                }
                ms.Dispose();
                zip.Dispose();
                result = Encoding.UTF8.GetString(ms.ToArray());
            }
            else
            {
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页(html)代码  
                result = sr.ReadToEnd();
            }
            result = Regex.Replace(result, @">\s+<", "><");
            result = Regex.Replace(result, @"\r\n", "");
            result = Regex.Replace(result, @"\r", "");
            result = Regex.Replace(result, @"\n", "");
            result = Regex.Replace(result, @"\t", "");
            return result;
        }
        public static string HttpGetTextar(string url, string host, string des)
        {
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "get";
                request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";

                request.CookieContainer = new CookieContainer();
                CookieCollection coll = new CookieCollection();

                coll.Add(new Cookie("JSESSIONID", "1B7D34474436762C1DA650B52B3904BE", "/", "textar.brakebook.com"));
                coll.Add(new Cookie("locale", "en_CN", "/", "textar.brakebook.com"));
                coll.Add(new Cookie("_pk_id.1.e52e", "8621bc7f6e75fa91.1498110293.1.1498111160.1498110293.", "/", "textar.brakebook.com"));
                coll.Add(new Cookie("_pk_ses.1.e52e", "*", "/", "textar.brakebook.com"));
                //JSESSIONID=1B7D34474436762C1DA650B52B3904BE; locale=en_CN; _pk_id.1.e52e=8621bc7f6e75fa91.1498110293.1.1498111105.1498110293.; _pk_ses.1.e52e=*

                request.CookieContainer.Add(coll);
                request.Host = host;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Encoding encoding = Encoding.Default;//Encoding.GetEncoding(response.CharacterSet);
                Stream instream = null;
                StreamReader sr = null;
                if (response.ContentEncoding == "gzip")
                {
                    MemoryStream ms = new MemoryStream();
                    GZipStream zip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    byte[] buffer = new byte[1024];
                    int l = zip.Read(buffer, 0, buffer.Length);
                    while (l > 0)
                    {
                        ms.Write(buffer, 0, l);
                        l = zip.Read(buffer, 0, buffer.Length);
                    }
                    ms.Dispose();
                    zip.Dispose();
                    result = Encoding.UTF8.GetString(ms.ToArray());
                }
                else
                {
                    //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                    instream = response.GetResponseStream();
                    sr = new StreamReader(instream, encoding);
                    //返回结果网页(html)代码  
                    result = sr.ReadToEnd();
                }

                //result = GetResponseAsString(response, encoding);
                //Stream myResponseStream = response.GetResponseStream();
                //StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);
                //result = myStreamReader.ReadToEnd();
                //myStreamReader.Close();
                //myResponseStream.Close();

            }
            catch (Exception)
            {
                goto aaaa;
            }

            result = Regex.Replace(result, @">\s+<", "><");
            result = Regex.Replace(result, @"\r\n", "");
            result = Regex.Replace(result, @"\r", "");
            result = Regex.Replace(result, @"\n", "");
            result = Regex.Replace(result, @"\t", "");


        aaaa: return result;
        }


        public static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        public static string GetResponse(string posturl, string refere, string str)
        {
            Encoding encoding = Encoding.GetEncoding("UTF-8");
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            // 准备请求,设置参数  
            request = WebRequest.Create(posturl) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = refere;

            request.CookieContainer = new CookieContainer();
            CookieCollection coll = new CookieCollection();
            coll.Add(new Cookie("PHPSESSID", "b15f6e1e8bd8f6f84ea4fab5c376516a", "/", "torchsparkplug.com"));
            //coll.Add(new Cookie("JSESSIONID", "A371349942A28264591C5C498B7D8032", "/", "jm8008.com"));
            request.CookieContainer.Add(coll);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Host = "www.torchsparkplug.com";

            byte[] data = encoding.GetBytes(str);
            request.ContentLength = data.Length;
            outstream = request.GetRequestStream();
            outstream.Write(data, 0, data.Length);
            outstream.Flush();
            outstream.Close();

            //发送请求并获取相应回应数据  
            response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求  
            instream = response.GetResponseStream();
            sr = new StreamReader(instream, encoding);
            //返回结果网页(html)代码  
            string content = sr.ReadToEnd();
            return content;
        }

        public static string Client(string posturl, string refere, string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("ContentLength", str.Length.ToString());
            client.Headers.Add("PHPSESSID", "9dbe843a77a57bf93ba2e2f5b30ded32");

            byte[] responseData = client.UploadData(posturl, "POST", bytes);
            string srcString = Encoding.UTF8.GetString(responseData);
            return srcString;
        }

        /// <summary>
        /// 精米账号：rbz4655433 密码：pay890813
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="refere"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string getJingMiResponse(string posturl, string refere, string str, string method)
        {
            Encoding encoding = Encoding.GetEncoding("UTF-8");
            Stream instream = null;
            StreamReader sr = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            // 准备请求,设置参数  
            request = WebRequest.Create(posturl) as HttpWebRequest;
            request.Method = method;// "GET";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = refere;

            request.CookieContainer = new CookieContainer();
            CookieCollection coll = new CookieCollection();

            coll.Add(new Cookie("JSESSIONID", "34719B267C793672E8A05CA8B6423618.jvm1", "/", "jm8008.com"));
            coll.Add(new Cookie("JYPJUSERNAME", "rbz4655433", "/", "jm8008.com"));
            coll.Add(new Cookie("JYPJPASSWORD", "pay890813", "/", "jm8008.com"));
            coll.Add(new Cookie("JYPJREMEMBER", "remember", "/", "jm8008.com"));
            coll.Add(new Cookie("Hm_lvt_44345fc09e1d1e652105adbbe25d0ff3", "1495245787", "/", "jm8008.com"));
            coll.Add(new Cookie("Hm_lpvt_44345fc09e1d1e652105adbbe25d0ff3", "1495265722", "/", "jm8008.com"));
            coll.Add(new Cookie("JM_SHOP_CART", "", "/", "jm8008.com"));

            request.CookieContainer.Add(coll);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request.Host = "www.jm8008.com";
            request.Connection = "alive";

            //发送请求并获取相应回应数据  
            response = request.GetResponse() as HttpWebResponse;
            string result = "";
            if (response.ContentEncoding == "gzip")
            {
                MemoryStream ms = new MemoryStream();
                GZipStream zip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                byte[] buffer = new byte[1024];
                int l = zip.Read(buffer, 0, buffer.Length);
                while (l > 0)
                {
                    ms.Write(buffer, 0, l);
                    l = zip.Read(buffer, 0, buffer.Length);
                }
                ms.Dispose();
                zip.Dispose();
                result = Encoding.UTF8.GetString(ms.ToArray());
            }
            else
            {
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页(html)代码  
                result = sr.ReadToEnd();
            }
            return result;
        }

        public static string UnicodeToGB(string text)
        {
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(text, "\\\\u([\\w]{4})");
            if (mc != null && mc.Count > 0)
            {
                foreach (System.Text.RegularExpressions.Match m2 in mc)
                {
                    string v = m2.Value;
                    string word = v.Substring(2);
                    byte[] codes = new byte[2];
                    int code = Convert.ToInt32(word.Substring(0, 2), 16);
                    int code2 = Convert.ToInt32(word.Substring(2), 16);
                    codes[0] = (byte)code2;
                    codes[1] = (byte)code;
                    text = text.Replace(v, Encoding.Unicode.GetString(codes));
                }
            }
            else
            {
            }
            return text;
        }

        //在C#后台实现JavaScript的函数escape()的字符串转换  
        //些方法支持汉字  
        public static string Escape(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                sb.Append((Char.IsLetterOrDigit(c)
                || c == '-' || c == '_' || c == '\\'
                || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
            }
            //return sb.ToString();
            return sb.ToString().Replace("%2B", "+");
        }
        //把JavaScript的escape()转换过去的字符串解释回来  
        //些方法支持汉字  
        public static string UnEscape(string str)
        {
            StringBuilder sb = new StringBuilder();
            int len = str.Length;
            int i = 0;
            while (i != len)
            {
                if (Uri.IsHexEncoding(str, i))
                    sb.Append(Uri.HexUnescape(str, ref i));
                else
                    sb.Append(str[i++]);
            }
            return sb.ToString();
        }

        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
    }
}

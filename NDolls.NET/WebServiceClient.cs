using System.Collections;
using System.Xml;
using System.Net;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace NDolls.NET
{
    public class WebServiceClient
    {
        private static Hashtable _xmlNamespaces = new Hashtable();//缓存xmlNamespace，避免重复调用GetNamespace
        private String url;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">Webservice's URL</param>
        public WebServiceClient(String url)
        {
            this.url = url;
            if (!this.url.Contains("?wsdl"))
            {
                this.url += "?wsdl";
            }
        }

        /// <summary>
        /// 需要WebService支持Post调用
        /// </summary>
        //public XmlDocument InvokePostSrv(String MethodName, Hashtable Pars)
        //{
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url + "/" + MethodName);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    SetWebRequest(request);
        //    byte[] data = EncodePars(Pars);
        //    WriteRequestData(request, data);

        //    return ReadXmlResponse(request.GetResponse());
        //}

        /// <summary>
        /// 需要WebService支持Get调用
        /// </summary>
        //public XmlDocument InvokeGetSrv(String MethodName, Hashtable Pars)
        //{
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url + "/" + MethodName + "?" + ParsToString(Pars));
        //    request.Method = "GET";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    SetWebRequest(request);
        //    return ReadXmlResponse(request.GetResponse());
        //}

        /// <summary>
        /// 通用WebService调用(Soap),参数Pars为String类型的参数名、参数值
        /// </summary>
        public XmlDocument Invoke(String MethodName, Hashtable Pars)
        {
            if (_xmlNamespaces.ContainsKey(url))
            {
                return InvokeSoapSrv(MethodName, Pars, _xmlNamespaces[url].ToString());
            }
            else
            {
                return InvokeSoapSrv(MethodName, Pars, GetNamespace(url));
            }
        }

        private XmlDocument InvokeSoapSrv(String MethodName, Hashtable Pars, string XmlNs)
        {
            _xmlNamespaces[url] = XmlNs;//加入缓存，提高效率
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "\"" + XmlNs + (XmlNs.EndsWith("/") ? "" : "/") + MethodName + "\"");
            SetWebRequest(request);
            byte[] data = EncodeParsToSoap(Pars, XmlNs, MethodName);
            WriteRequestData(request, data);
            XmlDocument doc = new XmlDocument(), doc2 = new XmlDocument();
            doc = ReadXmlResponse(request.GetResponse());

            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            String RetXml = doc.SelectSingleNode("//soap:Body/*/*", mgr).InnerXml;
            doc2.LoadXml("<root>" + RetXml + "</root>");
            AddDelaration(doc2);
            return doc2;
        }

        #region 辅助函数
        private static string GetNamespace(String url)
        {
            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = Encoding.UTF8;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(c.DownloadString(url));
            return doc.SelectSingleNode("//@targetNamespace").Value;
        }

        private static byte[] EncodeParsToSoap(Hashtable Pars, String XmlNs, String MethodName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"></soap:Envelope>");
            AddDelaration(doc);
            XmlElement soapBody = doc.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            XmlElement soapMethod = doc.CreateElement(MethodName);
            soapMethod.SetAttribute("xmlns", XmlNs);
            foreach (string k in Pars.Keys)
            {
                XmlElement soapPar = doc.CreateElement(k);
                soapPar.InnerXml = ObjectToSoapXml(Pars[k]);
                soapMethod.AppendChild(soapPar);
            }
            soapBody.AppendChild(soapMethod);
            doc.DocumentElement.AppendChild(soapBody);
            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }

        private static string ObjectToSoapXml(object o)
        {
            XmlSerializer mySerializer = new XmlSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            mySerializer.Serialize(ms, o);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(ms.ToArray()));
            if (doc.DocumentElement != null)
            {
                return doc.DocumentElement.InnerXml;
            }
            else
            {
                return o.ToString();
            }
        }

        private static void SetWebRequest(HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
        }

        private static void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }

        private static byte[] EncodePars(Hashtable Pars)
        {
            return Encoding.UTF8.GetBytes(ParsToString(Pars));
        }

        private static String ParsToString(Hashtable Pars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in Pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
            }
            return sb.ToString();
        }

        private static XmlDocument ReadXmlResponse(WebResponse response)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }

        private static void AddDelaration(XmlDocument doc)
        {
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.InsertBefore(decl, doc.DocumentElement);
        }
        #endregion
    }
}